using System;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Interfaces.UI;
using _src.Code.UI.Common;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Additive.SettingsWindow
{
    public class AudioPanel : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<AudioPanel, UxmlTraits> { }

        // Events for external logic to subscribe to
        public event Action<float> MusicVolumeChanged;
        public event Action<float> SfxVolumeChanged;

        // Fields
        private Slider _musicSlider;
        private Slider _sfxSlider;

        protected override void InitializeCore()
        {
            // 1. Set Title
            Get<SubTitle>("AudioSubTitle").Label.text = "Audio";

            // 2. Query Sliders
            _musicSlider = Get<Slider>("MusicSlider");
            _sfxSlider = Get<Slider>("SfxSlider");

            // 3. Forward UI events to public C# events
            if (_musicSlider != null)
            {
                _musicSlider.RegisterValueChangedCallback(evt => MusicVolumeChanged?.Invoke(evt.newValue));
            }

            if (_sfxSlider != null)
            {
                _sfxSlider.RegisterValueChangedCallback(evt => SfxVolumeChanged?.Invoke(evt.newValue));
            }

            // for now
            var clientDataService = ServiceLocator.GetService<IClientDataService>();
            MusicVolumeChanged += f =>
            {
                ServiceLocator.GetService<IClientDataService>().MusicVolume = f;
                ServiceLocator.GetService<IAudioService>().SetBackgroundMusicVolume(f);
            };

            SfxVolumeChanged += f =>
            {
                ServiceLocator.GetService<IClientDataService>().SoundEffectVolume = f;
                ServiceLocator.GetService<IAudioService>().SetSoundEffectsVolume(f);
            };
            
            Setup(clientDataService.MusicVolume, clientDataService.SoundEffectVolume);
        }

        /// <summary>
        /// Set initial values without triggering the events
        /// </summary>
        public void Setup(float currentMusicVol, float currentSfxVol)
        {
            if (_musicSlider != null)
            {
                _musicSlider.SetValueWithoutNotify(currentMusicVol);
            }

            if (_sfxSlider != null)
            {
                _sfxSlider.SetValueWithoutNotify(currentSfxVol);
            }
        }
    }
}