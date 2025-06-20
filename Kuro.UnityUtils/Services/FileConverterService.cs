using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using static UnityEngine.Debug;

namespace Kuro.UnityUtils.Services
{
    public static partial class FileConverterService
    {
        #region Wav File Conversion
        private const int WAV_HEADER_INDEX = 44;

        public static async Task<AudioClip> WAV_FromDataPath(CancellationTokenSource tokenSource, string relativePath)
        {
            return await FileLoaderService.LoadFromDataPath(tokenSource, relativePath, static (rawFile, fileInfo) =>
            {
                var wav = WAV_Reader(rawFile);
                var clip = WAV_ToClip(wav, rawFile, fileInfo);
                return clip;
            });
        }

        public static async Task<AudioClip> WAV_FromFullPath(CancellationTokenSource tokenSource, string fullPath)
        {
            return await FileLoaderService.LoadFromDataPath(tokenSource, fullPath, static (rawFile, fileInfo) =>
            {
                var wav = WAV_Reader(rawFile);
                var clip = WAV_ToClip(wav, rawFile, fileInfo);
                return clip;
            });
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct WAV
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ChunkID;           // "RIFF"
            public uint ChunkSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] Format;           // "WAVE"

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] Subchunk1ID;      // "fmt "
            public uint Subchunk1Size;      // 16 for PCM
            public ushort AudioFormat;      // 1 = PCM
            public ushort NumChannels;
            public uint SampleRate;
            public uint ByteRate;
            public ushort BlockAlign;
            public ushort BitsPerSample;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] Subchunk2ID;      // "data"
            public uint Subchunk2Size;      // NumSamples * NumChannels * BitsPerSample/8
        }

        private static WAV WAV_Reader(byte[] raw)
        {
            using var mem = new MemoryStream(raw);
            using var reader = new BinaryReader(mem);

            var wav = new WAV
            {
                ChunkID = reader.ReadBytes(4),
                ChunkSize = reader.ReadUInt32(),
                Format = reader.ReadBytes(4),
                Subchunk1ID = reader.ReadBytes(4),
                Subchunk1Size = reader.ReadUInt32(),
                AudioFormat = reader.ReadUInt16(),
                NumChannels = reader.ReadUInt16(),
                SampleRate = reader.ReadUInt32(),
                ByteRate = reader.ReadUInt32(),
                BlockAlign = reader.ReadUInt16(),
                BitsPerSample = reader.ReadUInt16(),
                Subchunk2ID = reader.ReadBytes(4),
                Subchunk2Size = reader.ReadUInt32()
            };

            return wav;
        }

        private static AudioClip WAV_ToClip(WAV header, byte[] rawFile, FileLoaderService.FileInfo fileInfo)
        {
            int bytesPerSample = header.BitsPerSample / 8;
            int totalSampleCount = (int)(header.Subchunk2Size / bytesPerSample);
            int sampleCountPerChannel = totalSampleCount / header.NumChannels;
            float[] samples = new float[totalSampleCount];

            using (var mem = new MemoryStream(rawFile))
            {
                mem.Position = WAV_HEADER_INDEX; // Start reading after the WAV header
                using var reader = new BinaryReader(mem);

                for (int i = 0; i < sampleCountPerChannel; i++)
                {
                    for (int ch = 0; ch < header.NumChannels; ch++)
                    {
                        int sampleIndex = i * header.NumChannels + ch;
                        switch (header.BitsPerSample)
                        {
                            case 16:
                                samples[sampleIndex] = reader.ReadInt16() / 32768f;
                                break;
                            case 24:
                                int sample24 = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
                                if ((sample24 & 0x800000) != 0)
                                    sample24 |= unchecked((int)0xFF000000);
                                samples[sampleIndex] = sample24 / 8388608f;
                                break;
                            case 32:
                                samples[sampleIndex] = reader.ReadInt32() / 2147483648f;
                                break;
                            default:
                                throw new NotSupportedException($"Unsupported BitsPerSample: {header.BitsPerSample}");
                        }
                    }
                }
            }

            var audioClip = AudioClip.Create(fileInfo.FileName ?? "Unknown", sampleCountPerChannel, header.NumChannels, (int)header.SampleRate, false);
            if (!audioClip.SetData(samples, 0))
            {
                LogError("Failed to set audio clip data.");
                return null;
            }
            return audioClip;
        }


        /* public static async Task<AudioClip> Load_WAV(CancellationTokenSource tokenSource, string relativePath)
        {
            return await FileLoaderService.LoadFromDataPath(tokenSource, relativePath, static (wavFile, fileName) =>
            {
                var parser = WavParser.Parse(wavFile);
                if (parser == null || parser.SampleCount == 0)
                {
                    LogError("Failed to parse WAV file or no samples found.");
                    return null;
                }

                Log($"WAV file loaded: Channels: {parser.Channels}, Sample Rate: {parser.SampleRate}, Sample Count: {parser.SampleCount}");

                var audioClip = AudioClip.Create(fileName, parser.SamplePerChannel, parser.Channels, parser.SampleRate, false);
                audioClip.SetData(parser.Samples, 0);
                return audioClip;
            });
        }

        public class WavParser
        {
            public int Channels { get; private set; }
            public int SampleRate { get; private set; }
            public float[] Samples { get; private set; }
            public int BitsPerSample { get; private set; }
            public int DataSize { get; private set; }
            public int SampleCount => Samples?.Length ?? 0;
            public int SamplePerChannel => SampleCount / Channels;
            public int Length => SampleCount * Channels * (BitsPerSample / 8);

            public static WavParser Parse(byte[] wavFile)
            {
                if (wavFile == null || wavFile.Length < 44)
                    throw new Exception("Invalid WAV file: too short");

                int channels = BitConverter.ToInt16(wavFile, 22);
                int sampleRate = BitConverter.ToInt32(wavFile, 24);
                int bitsPerSample = BitConverter.ToInt16(wavFile, 34);
                int dataSize = BitConverter.ToInt32(wavFile, 40);
                int dataStart = 44;

                if (bitsPerSample != 16 && bitsPerSample != 24 && bitsPerSample != 32)
                    throw new NotSupportedException($"Only 16-bit, 24-bit, and 32-bit PCM supported, got {bitsPerSample}");

                int bytesPerSample = bitsPerSample / 8;
                int totalSamples = dataSize / bytesPerSample;

                float[] floatData = new float[totalSamples];

                for (int i = 0; i < totalSamples; i++)
                {
                    int sample = 0;

                    switch (bitsPerSample)
                    {
                        case 16:
                            sample = BitConverter.ToInt16(wavFile, dataStart + i * bytesPerSample);
                            floatData[i] = sample / 32768f;
                            break;
                        case 24:
                            sample = (wavFile[dataStart + i * bytesPerSample + 2] << 16) |
                                     (wavFile[dataStart + i * bytesPerSample + 1] << 8) |
                                     wavFile[dataStart + i * bytesPerSample];
                            if ((sample & 0x800000) != 0)
                                sample |= unchecked((int)0xFF000000);
                            floatData[i] = sample / 8388608f;
                            break;
                        case 32:
                            sample = BitConverter.ToInt32(wavFile, dataStart + i * bytesPerSample);
                            floatData[i] = sample / 2147483648f;
                            break;
                    }
                }

                return new WavParser
                {
                    Channels = channels,
                    SampleRate = sampleRate,
                    Samples = floatData,
                    BitsPerSample = bitsPerSample,
                    DataSize = dataSize
                };
            }
        } */
        #endregion

        #region Texture2D File Conversion
        public static async Task<Texture2D> Texture2D_FromDataPath(CancellationTokenSource tokenSource, string relativePath)
        {
            return await FileLoaderService.LoadFromDataPath(tokenSource, relativePath, static (rawFile, fileInfo) =>
            {
                var tex2D = new Texture2D(2, 2);
                tex2D.LoadRawTextureData(rawFile);
                return tex2D;
            });
        }

        public static async Task<Texture> Texture2D_FromFullPath(CancellationTokenSource tokenSource, string fullPath)
        {
            return await FileLoaderService.LoadFrom(tokenSource, fullPath, static (rawFile, fileInfo) =>
            {
                var tex2D = new Texture2D(2, 2);
                tex2D.LoadRawTextureData(rawFile);
                return tex2D;
            });
        }
        #endregion

        #region JSON File Conversion
        public static async Task<T> JSON_FromDataPath<T>(CancellationTokenSource tokenSource, string relativePath, JsonSerializerSettings settings = null) where T : class
        {
            return await FileLoaderService.LoadFromDataPath<T>(tokenSource, relativePath, (rawFile, fileInfo) =>
            {
                var rawJSON = Encoding.UTF8.GetString(rawFile);
                var json = JsonConvert.DeserializeObject<T>(rawJSON, settings);
                return json;
            });
        }

        public static async Task<T> JSON_FromFullPath<T>(CancellationTokenSource tokenSource, string fullPath, JsonSerializerSettings settings = null) where T : class
        {
            return await FileLoaderService.LoadFrom<T>(tokenSource, fullPath, (rawFile, fileInfo) =>
            {
                var rawJSON = Encoding.UTF8.GetString(rawFile);
                var json = JsonConvert.DeserializeObject<T>(rawJSON, settings);
                return json;
            });
        }

        public static async Task<T> JSON_FromPersistentDataPath<T>(CancellationTokenSource tokenSource, string relativePath, JsonSerializerSettings settings = null) where T : class
        {
            return await FileLoaderService.LoadFromPersistentDataPath<T>(tokenSource, relativePath, (rawFile, fileInfo) =>
            {
                var rawJSON = Encoding.UTF8.GetString(rawFile);
                var json = JsonConvert.DeserializeObject<T>(rawJSON, settings);
                return json;
            });
        }
        #endregion
    }
}
