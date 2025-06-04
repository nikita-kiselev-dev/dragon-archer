using VContainer.Unity;

namespace Core.View.Canvas.Scripts
{
    public class ServiceCanvas : UnityEngine.MonoBehaviour, IStartable
    {
        public static ServiceCanvas Instance;

        public void Start()
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
}