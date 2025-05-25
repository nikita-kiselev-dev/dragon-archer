using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Game;
using Infrastructure.Service.View.ViewManager;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Service.View.Canvas
{
    public class MainCanvas : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Canvas m_Canvas;
        [SerializeField] private Transform m_WindowParent;
        [SerializeField] private Transform m_PopupParent;
        [SerializeField] private Image m_PopupBackground;
        [SerializeField] private Button m_PopupBackgroundButton;

        private Action _onBackgroundClicked;
        private Dictionary<string, Transform> _viewParents;

        public Image PopupBackground => m_PopupBackground;

        public void Init(Action onBackgroundClicked)
        {
            _onBackgroundClicked = onBackgroundClicked;
            
            SetCamera(GameInfo.MainCameraKey);
            SetViewParents();
            
            m_PopupBackgroundButton.onClick.RemoveListener(CloseLastView);
            m_PopupBackgroundButton.onClick.AddListener(CloseLastView);
        }
        
        public Transform GetParent(string viewType)
        {
            return _viewParents.GetValueOrDefault(viewType);
        }

        private void SetCamera(string cameraName)
        {
            var mainCamera = GameObject.FindGameObjectsWithTag(cameraName).First().GetComponent<Camera>();
            m_Canvas.worldCamera = mainCamera;
        }

        private void SetViewParents()
        {
            _viewParents = new Dictionary<string, Transform>()
            {
                { ViewType.Window, m_WindowParent },
                { ViewType.Popup, m_PopupParent }
            };
        }

        private void CloseLastView()
        {
            _onBackgroundClicked?.Invoke();
        }
    }
}