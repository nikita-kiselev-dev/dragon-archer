using Infrastructure.Service.View.ViewSignalManager;
using UnityEngine;

namespace Infrastructure.Service.View.ViewManager
{
    public abstract class MonoView : MonoBehaviour
    {
        public abstract void Init(IViewSignalManager viewSignalManager);
    }
}