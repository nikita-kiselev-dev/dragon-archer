using UnityEngine;
using VContainer;

namespace Infrastructure.Service.View.Canvas
{
    public class ServiceCanvas : MonoBehaviour, ICanvas
    {
        [Inject]
        private void Init()
        {
            DontDestroyOnLoad(this);
        }
    }
}