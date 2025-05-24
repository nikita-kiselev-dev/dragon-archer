using Infrastructure.Service.View.ViewManager;
using UnityEngine.Events;

namespace Content.SettingsPopup.Scripts.View
{
    public abstract class ISettingsPopupView : MonoView
    {
        public abstract void Init(UnityAction<float> onSoundsVolumeChanged, UnityAction<float> onMusicVolumeChanged);
        public abstract void SetSoundsSliderValue(float value);
        public abstract void SetMusicSliderValue(float value);
    }
}