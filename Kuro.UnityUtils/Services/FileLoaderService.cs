using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Debug;

namespace Kuro.UnityUtils.Services
{
    public static partial class FileLoaderService
    {
        public readonly struct FileInfo
        {
            public string FileName { get; }
            public string FullPath { get; }
            public string RelativePath => Path.GetRelativePath(Application.dataPath, FullPath);
            public string Extension => Path.GetExtension(FileName).ToLowerInvariant();

            public FileInfo(string fileName, string fullPath)
            {
                FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
                FullPath = fullPath ?? throw new ArgumentNullException(nameof(fullPath));
            }
        }

        public static async Task<T> LoadFrom<T>(CancellationTokenSource tokenSource, string fullPath, Func<byte[], FileInfo, T> converter) where T : class
        {
            if (tokenSource == null)
                throw new ArgumentNullException(nameof(tokenSource));

            if (string.IsNullOrEmpty(fullPath))
                throw new ArgumentException("Relative path cannot be null or empty.", nameof(fullPath));

            var fileName = Path.GetFileName(fullPath);
            var fileInfo = new FileInfo(Path.GetFileName(fileName), fullPath);

            byte[] fileContent = await BytesReader(tokenSource, fullPath);

            return converter(fileContent, fileInfo);
        }

        public static async Task<T> LoadFromDataPath<T>(CancellationTokenSource tokenSource, string relativePath, Func<byte[], FileInfo, T> converter) where T : class
        {
            if (tokenSource == null)
                throw new ArgumentNullException(nameof(tokenSource));

            if (string.IsNullOrEmpty(relativePath))
                throw new ArgumentException("Relative path cannot be null or empty.", nameof(relativePath));

            var fullPath = Path.Combine(Application.dataPath, relativePath);
            var fileInfo = new FileInfo(Path.GetFileName(relativePath), fullPath);

            byte[] fileContent = await BytesReader(tokenSource, fullPath);;

            return converter(fileContent, fileInfo);
        }

        public static async Task<T> LoadFromPersistentDataPath<T>(CancellationTokenSource tokenSource, string relativePath, Func<byte[], FileInfo, T> converter) where T : class
        {
            if (tokenSource == null)
                throw new ArgumentNullException(nameof(tokenSource));

            if (string.IsNullOrEmpty(relativePath))
                throw new ArgumentException("Relative path cannot be null or empty.", nameof(relativePath));

            string fullPath = Path.Combine(Application.persistentDataPath, relativePath);
            var fileInfo = new FileInfo(Path.GetFileName(relativePath), fullPath);

            byte[] fileContent = await BytesReader(tokenSource, fullPath);;

            return converter(fileContent, fileInfo);
        }

        public static async Task<T> LoadFromStreamingAssetsPath<T>(CancellationTokenSource tokenSource, string relativePath, Func<byte[], FileInfo, T> converter) where T : class
        {
            if (tokenSource == null)
                throw new ArgumentNullException(nameof(tokenSource));

            if (string.IsNullOrEmpty(relativePath))
                throw new ArgumentException("Relative path cannot be null or empty.", nameof(relativePath));

            string fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);
            var fileInfo = new FileInfo(Path.GetFileName(relativePath), fullPath);

            byte[] fileContent = await BytesReader(tokenSource, fullPath);;

            return converter(fileContent, fileInfo);
        }

        private static async Task<byte[]> BytesReader(CancellationTokenSource tokenSource, string path)
        {
            if (!File.Exists(path))
            {
                LogError($"File not found at path: {path}");
                return null;
            }

            try
            {
                using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                var temp = new byte[fileStream.Length];
                await fileStream.ReadAsync(temp, 0, (int)fileStream.Length, tokenSource.Token);
                return temp;
            }
            catch (Exception ex)
            {
                LogError($"Error loading file from path {path}: {ex.Message}");
                return null;
            }
        }
    }
}

