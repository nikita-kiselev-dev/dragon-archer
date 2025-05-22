using Content.SettingsPopup.Scripts.Model;
using Content.SettingsPopup.Scripts.View;
using Infrastructure.Service.Audio;
using Infrastructure.Service.View.ViewManager;

namespace Content.SettingsPopup.Scripts.Presenter
{
    public class SettingsPopupPresenter : ISettingsPopupPresenter
    {
        private readonly ISettingsPopupModel _model;
        private readonly ISettingsPopupView _view;
        private readonly IViewManager _viewManager;
        
        //private IViewInteractor _viewInteractor;
        
        public bool IsInited { get; private set;}

        public SettingsPopupPresenter(ISettingsPopupModel model, ISettingsPopupView view, IViewManager viewManager)
        {
            _model = model;
            _view = view;
            _viewManager = viewManager;
        }
        
        public void Init()
        {
            //RegisterAndInitView();
            ConfigureView();
            IsInited = true;
        }
        
        public void Open()
        {
            _viewManager.Open(ViewInfo.SettingsPopup);
            //_viewInteractor.Open();
        }

        public void Close()
        {
            //_viewInteractor.Close();
        }

        private void RegisterAndInitView()
        {
            /*var viewSignalManager = new ViewSignalManager()
                .AddSignal<float>(SettingsPopupSignals.SoundsVolumeChangedSignal, SetSoundsVolume)
                .AddSignal<float>(SettingsPopupSignals.MusicVolumeChangedSignal, SetMusicVolume)
                .AddCloseSignal(Close);
            
            _viewInteractor = new ViewRegistrar(_viewManager)
                .SetViewKey(ViewInfo.SettingsPopup)
                .SetViewType(ViewType.Popup)
                .SetView(_view)
                .SetViewSignalManager(viewSignalManager)
                .RegisterAndInit();*/
        }

        private void ConfigureView()
        {
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