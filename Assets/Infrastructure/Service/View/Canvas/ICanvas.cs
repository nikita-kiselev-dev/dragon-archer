using UnityEngine;

namespace Infrastructure.Service.View.Canvas
{
    public interface ICanvas
    {
        Transform ViewParentTransform { get; }
    }
}