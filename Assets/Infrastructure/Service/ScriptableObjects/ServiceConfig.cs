using Infrastructure.Service.LiveOps;
using Infrastructure.Service.SaveLoad;
using UnityEngine;

namespace Infrastructure.Service.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ServiceConfig", menuName = "ScriptableObjects")]
    public class ServiceConfig : ScriptableObject
    {
        public SaveLoadServices m_SaveLoadService;
        public LiveOpsServices m_LiveOpsService;
    }
}