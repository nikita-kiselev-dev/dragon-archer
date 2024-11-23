using UnityEngine;
using VContainer.Unity;

namespace Infrastructure.Service.View.Canvas
{
    public class ServiceCanvas : MonoBehaviour, IStartable
    {
        public static ServiceCanvas Instance;

        public void Start()
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
}