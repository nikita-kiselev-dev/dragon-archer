using Core.LiveOps;
using Core.SaveLoad;
using UnityEngine;

namespace Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ServiceConfig", menuName = "ScriptableObjects")]
    public class ServiceConfig : ScriptableObject
    {
        public SaveLoadServices m_SaveLoadService;
        public LiveOpsServices m_LiveOpsService;
    }
}