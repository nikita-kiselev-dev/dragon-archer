using Core.SaveLoad;
using UnityEngine;

namespace Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ServiceConfig", menuName = "ScriptableObjects")]
    public class ServiceConfig : ScriptableObject
    {
        public SaveLoadServices SaveLoadService;
    }
}