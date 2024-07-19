using Content.SettingsPopup.Scripts.Data;
using Content.SettingsPopup.Scripts.Model;
using Content.SettingsPopup.Scripts.View;
using Infrastructure.Service.Audio;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewSignalManager;
using VContainer;

namespace Content.SettingsPopup.Scripts.Presenter
{
    public class SettingsPopupPresenter : ISettingsPopupPresenter
    {
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        [Inject] private readonly SettingsPopupData _data;

        private ISettingsPopupModel _model;
        private ISettingsPopupView _view;
        private IViewInteractor _viewInteractor;
        
        public void Init()
        {
            CreateModel();
            CreateView();
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
        
        private void CreateModel()
        {
            _model = new SettingsPopupModel(_data);
        }
        
        private void CreateView()
        {
            _view = _viewFactory.CreateView<ISettingsPopupView>(ViewInfo.SettingsPopupKey, ViewType.Popup);
        }

        private void RegisterAndInitView()
        {
            var viewSignalManager = new ViewSignalManager()
                .AddSignal<float>(SettingsPopupSignals.SoundsVolumeChangedSignal, SetSoundsVolume)
                .AddSignal<float>(SettingsPopupSignals.MusicVolumeChangedSignal, SetMusicVolume)
                .AddCloseSignal(Close);
            
            _viewInteractor = new ViewRegistrar(_viewManager)
                .SetViewKey(ViewInfo.SettingsPopupKey)
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