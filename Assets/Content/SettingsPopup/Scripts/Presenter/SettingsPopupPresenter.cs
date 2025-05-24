using Content.SettingsPopup.Scripts.Model;
using Content.SettingsPopup.Scripts.View;
using Infrastructure.Service.Audio;

namespace Content.SettingsPopup.Scripts.Presenter
{
    public class SettingsPopupPresenter : ISettingsPopupPresenter
    {
        private readonly ISettingsPopupModel _model;
        private readonly ISettingsPopupView _view;
        
        public bool IsInited { get; private set;}

        public SettingsPopupPresenter(ISettingsPopupModel model, ISettingsPopupView view)
        {
            _model = model;
            _view = view;
        }
        
        public void Init()
        {
            ConfigureView();
            IsInited = true;
        }
        
        public void Open()
        {
            _view.ViewInteractor.Open();
        }

        private void ConfigureView()
        {
            _view.Init(SetSoundsVolume, SetMusicVolume);
            
            var soundsVolume = _model.GetSoundsVolume();
            SetSoundsVolume(soundsVolume);
            _view.SetSoundsSliderValue(soundsVolume);
            
            var musicVolume = _model.GetMusicVolume();
            SetMusicVolume(musicVolume);
            _view.SetMusicSliderValue(musicVolume);
        }

        private void SetSoundsVolume(float volume)
        {
            _model.SetSoundsVolume(volume);
            AudioController.Instance.SetSoundsVolume(volume);
        }

        private void SetMusicVolume(float volume)
        {
            _model.SetMusicVolume(volume);
            AudioController.Instance.SetMusicVolume(volume);
        }
    }
}