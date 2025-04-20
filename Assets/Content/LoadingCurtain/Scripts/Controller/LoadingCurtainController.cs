using Content.LoadingCurtain.Scripts.View;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Initialization.Signals;
using Infrastructure.Service.Localization;
using Infrastructure.Service.Scene.Signals;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.View.ViewManager.ViewAnimation;
using UnityEngine;
using VContainer;

namespace Content.LoadingCurtain.Scripts.Controller
{
    public class LoadingCurtainController : MonoBehaviour, ILoadingCurtainController
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
            _signalBus.Subscribe<OnSceneInitCompletedSignal>(this, Hide);

            if (_isInited)
            {
                return;
            }

            _animator = new LoadingCurtainGradientColorAnimator(m_View, OnShowed);
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

        private void Hide()
        {
            if (m_View.gameObject.activeSelf)
            {
                _animator.Hide();
            }
        }

        private void OnShowed()
        {
            _signalBus.Trigger<StartSceneChangeSignal>();
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