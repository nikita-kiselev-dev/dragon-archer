using UnityEngine;

namespace Infrastructure.Service.View
{
    public interface IMonoBehaviour
    {
        public MonoBehaviour MonoBehaviour { get; }
        public void SetActive(bool isActive);
    }
}