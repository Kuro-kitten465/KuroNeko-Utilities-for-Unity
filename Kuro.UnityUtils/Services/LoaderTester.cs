using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kuro.UnityUtils.Services
{
    public class LoaderTester : MonoBehaviour
    {
        [SerializeField] private TMP_Text _debugText;
        [SerializeField] private string[] _files;
        [SerializeField] private AudioSource _audioSource;

        private CancellationTokenSource _tokenSource;

        private void Awake() => _tokenSource = new();
        private void OnDisable() => _tokenSource.Cancel();

        private async void Start()
        {
            var clip = await FileConverterService.WAV_FromDataPath(_tokenSource, _files[0]);

            if (clip == null) throw new ArgumentNullException(nameof(clip), "Audio clip could not be loaded from the specified file path.");

            _audioSource.clip = clip;
            _audioSource.Play();
            _debugText.text = $"Clip Length: {FormatTime(clip.length)}";
            StartCoroutine(PlayAudio(_audioSource.clip.length));
        }

        private IEnumerator PlayAudio(float length)
        {
            var startedTime = Time.time;

            while (Time.time - startedTime < length)
            {
                _debugText.text = $"Clip Length: {FormatTime(length)}\nElapsed: {FormatTime(Time.time - startedTime)}";
                yield return null;
            }

            Debug.Log("Finished Playing...");
        }

        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}
