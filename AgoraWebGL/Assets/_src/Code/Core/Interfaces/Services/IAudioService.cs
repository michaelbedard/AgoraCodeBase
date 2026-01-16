using System.Threading.Tasks;
using UnityEngine;

namespace _src.Code.Core.Interfaces.Services
{
    public interface IAudioService
    {
        Task PlayBackgroundMusicAsync(string backgroundMusicAddress);
        void PlaySoundEffectAsync(AudioClip soundEffect);
        void SetBackgroundMusicVolume(float volume);
        void SetSoundEffectsVolume(float volume);
        void StopMusic();
    }
}