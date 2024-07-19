using UnityEngine;
using VContainer;

namespace Infrastructure.Service
{
    public class MonoCoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        [Inject]
        private void Init()
        {
            DontDestroyOnLoad(this);
        }
    }
}