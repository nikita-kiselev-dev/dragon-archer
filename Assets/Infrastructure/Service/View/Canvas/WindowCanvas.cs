using System.Linq;
using Infrastructure.Game;
using UnityEngine;

namespace Infrastructure.Service.View.Canvas
{
    public class WindowCanvas : MonoBehaviour, IWindowCanvas
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