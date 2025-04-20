using UnityEngine;

namespace Content.LoadingCurtain.Scripts.View
{
    public abstract class ILoadingCurtainView : MonoBehaviour
    {
        public abstract void SetLoadingText(string text);
    }
}