using Content.Settings.Scripts.Model;
using Content.Settings.Scripts.View;
using Infrastructure.Service.Audio;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewSignalManager;

namespace Content.Settings.Scripts.Presenter
{
    public class SettingsPopupPresenter : ISettingsPopupPresenter
    {
        private readonly ISettingsPopupModel _model;
        private readonly IViewFactory _viewFactory;
        private readonly IViewManager _viewManager;

        private ISettingsPopupView _view;
        private IViewInteractor _viewInteractor;

        public SettingsPopupPresenter(ISettingsPopupModel model, IViewFactory viewFactory, IViewManager viewManager)
        {
            _model = model;
            _viewFactory = viewFactory;
            _viewManager = viewManager;
        }
        
        public void Init()
        {
            RegisterAndInitView();
            ConfigureView();
        }
        
        public void Open()
        {
            _viewInteractor.Open();
        }

        public void Close()
        {
            _viewInteractor.Close();
        }

        private void RegisterAndInitView()
        {
            _view = _viewFactory.CreateView<ISettingsPopupView>(ViewInfo.SettingsPopup, ViewType.Popup);
            
            var viewSignalManager = new ViewSignalManager()
                .AddSignal<float>(SettingsPopupSignals.SoundsVolumeChangedSignal, SetSoundsVolume)
                .AddSignal<float>(SettingsPopupSignals.MusicVolumeChangedSignal, SetMusicVolume)
                .AddCloseSignal(Close);
            
            _viewInteractor = new ViewRegistrar(_viewManager)
                .SetViewKey(ViewInfo.SettingsPopup)
                .SetViewType(ViewType.Popup)
                .SetView(_view)
                .SetViewSignalManager(viewSignalManager)
                .RegisterAndInit();
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
            AudioService.Instance.SetSoundsVolume(volume);
        }

        private void SetMusicVolume(float volume)
        {
            _model.SetMusicVolume(volume);
            AudioService.Instance.SetMusicVolume(volume);
        }
    }
}