using System.Collections.Generic;
using System.Linq;
using Infrastructure.Game;
using Infrastructure.Service.View.ViewManager;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Service.View.Canvas
{
    public class MainCanvas : MonoBehaviour, ICanvas
    {
        [SerializeField] private UnityEngine.Canvas m_Canvas;
        [SerializeField] private Transform m_WindowParent;
        [SerializeField] private Transform m_PopupParent;
        [SerializeField] private Image m_PopupBackground;

        private Dictionary<string, Transform> m_ViewParents;

        public Image PopupBackground => m_PopupBackground;

        public Transform GetParent(string viewType)
        {
            return m_ViewParents.GetValueOrDefault(viewType);
        }
        
        public void Init()
        {
            SetCamera(GameInfo.MainCameraKey);
            SetViewParents();
        }

        private void SetCamera(string cameraName)
        {
            var mainCamera = GameObject.FindGameObjectsWithTag(cameraName).First().GetComponent<Camera>();
            m_Canvas.worldCamera = mainCamera;
        }

        private void SetViewParents()
        {
            m_ViewParents = new Dictionary<string, Transform>()
            {
                { ViewType.Window, m_WindowParent },
                { ViewType.Popup, m_PopupParent }
            };
        }
    }
}