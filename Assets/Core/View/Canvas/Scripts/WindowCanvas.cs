using System.Linq;
using Core.Initialization.Scripts;
using UnityEngine;

namespace Core.View.Canvas.Scripts
{
    public class WindowCanvas : UnityEngine.MonoBehaviour, IWindowCanvas
    {
        [SerializeField] private UnityEngine.Canvas m_Canvas;
        
        public Transform ViewParentTransform => gameObject.transform;
        
        public void Init()
        {
            SetCamera(GameInfo.MainCameraKey);
        }
        
        private void SetCamera(string cameraName)
        {
            var mainCamera = GameObject.FindGameObjectsWithTag(cameraName).First().GetComponent<Camera>();
            m_Canvas.worldCamera = mainCamera;
        }
    }
}