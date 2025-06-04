using UnityEngine;

namespace Core.View.Canvas.Scripts
{
    public interface ICanvas
    {
        Transform ViewParentTransform { get; }
    }
}