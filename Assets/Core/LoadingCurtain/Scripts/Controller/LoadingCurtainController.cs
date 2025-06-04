using Core.Initialization.Scripts.Signals;
using Core.LoadingCurtain.Scripts.View;
using Core.Localization;
using Core.Scene;
using Core.Scene.Signals;
using Core.SignalBus;
using Core.View.ViewManager.ViewAnimation;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Core.LoadingCurtain.Scripts.Controller
{
    public class LoadingCurtainController : UnityEngine.MonoBehaviour, ILoadingCurtainController
    {
        [SerializeField] private LoadingCurtainView m_View;
        
        private ISignalBus _signalBus;
        private IViewAnimator _animator;

        private bool _isInited;
        
        [Inject]
        private void Init(ISignalBus signalBus)
        {
            _signalBus = signalBus;
            
            _signalBus.Unsubscribe<OnChangeSceneRequestSignal>(this);
            _signalBus.Subscribe<OnChangeSceneRequestSignal>(this, Show);
            
            _signalBus.Unsubscribe<OnSceneInitCompletedSignal>(this);
            _signalBus.Subscribe<OnSceneInitCompletedSignal, string>(this, Hide);

            if (_isInited)
            {
                return;
            }

            _animator = new LoadingCurtainGradientColorAnimator(m_View, AfterShow, AfterHide);
            _signalBus = signalBus;
            ConfigureView().Forget();

            _isInited = true;
        }
        
        private void Show()
        {
            if (!m_View.gameObject.activeSelf)
            {
                _animator.Show();
            }
        }

        private void Hide(string sceneName)
        {
            if (m_View.gameObject.activeSelf && sceneName != SceneConstants.BootstrapScene)
            {
                _animator.Hide();
            }
        }

        private void AfterShow()
        {
            _signalBus.Trigger<StartSceneChangeSignal>();
        }

        private void AfterHide()
        {
            _signalBus.Trigger<LoadingCurtainHiddenSignal>();
        }

        private async UniTaskVoid ConfigureView()
        {
            var loadingLocalizedString = await "loading".Localize();
            m_View.SetLoadingText(loadingLocalizedString);
        }

        private void Unsubscribe()
        {
            _signalBus.Unsubscribe<OnChangeSceneRequestSignal>(this);
            _signalBus.Unsubscribe<OnSceneInitCompletedSignal>(this);
        }

        private void OnDestroy()
        {
            if (_signalBus == null)
            {
                return;
            }

            Unsubscribe();
        }
    }
}