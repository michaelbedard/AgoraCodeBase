using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace _src.Code.App.Services
{
    public class AudioService : IAudioService
    {
        private readonly AudioSource _backgroundMusicSource;
        private readonly AudioSource _soundEffectSource;

        private string _currentBackgroundMusicAddress;

        [Inject]
        public AudioService(
            [Inject(Id = "BackgroundMusic")] AudioSource backgroundMusicSource,
            [Inject(Id = "SoundEffects")] AudioSource soundEffectSource)
        {
            _backgroundMusicSource = backgroundMusicSource;
            _soundEffectSource = soundEffectSource;
        }
        
        public async Task PlayBackgroundMusicAsync(string backgroundMusicAddress)
        {
            if (_backgroundMusicSource.isPlaying)
            {
                // return if already playing this
                if (_currentBackgroundMusicAddress == backgroundMusicAddress)
                {
                    return;
                }
                
                _backgroundMusicSource.Stop();
            }

            var handle = Addressables.LoadAssetAsync<AudioClip>(backgroundMusicAddress);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var clip = handle.Result;
                _backgroundMusicSource.clip = clip;
                _backgroundMusicSource.loop = true;
                _backgroundMusicSource.Play();
                
                _currentBackgroundMusicAddress = backgroundMusicAddress;
            }
            else
            {
                Debug.LogError($"Failed to load audio clip from address: {handle.OperationException}");
            }
        }
        
        public void PlaySoundEffectAsync(AudioClip soundEffect)
        {
            _soundEffectSource.PlayOneShot(soundEffect);
        }

        public void SetBackgroundMusicVolume(float volume)
        {
            // volume is between 0 and 100
            _backgroundMusicSource.volume = volume / 100;
        }
        
        public void SetSoundEffectsVolume(float volume)
        {
            _soundEffectSource.volume = volume / 100f;
        }

        public void StopMusic()
        {
            _backgroundMusicSource.Stop();
            _currentBackgroundMusicAddress = string.Empty;
        }
    }
}