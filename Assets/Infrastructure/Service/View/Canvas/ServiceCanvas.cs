using UnityEngine;
using VContainer;

namespace Infrastructure.Service.View.Canvas
{
    public class ServiceCanvas : MonoBehaviour
    {
        public static ServiceCanvas Instance;
        
        [Inject]
        private void Init()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
}