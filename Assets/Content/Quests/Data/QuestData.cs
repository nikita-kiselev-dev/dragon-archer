using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Content.Quests.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class QuestData : Infrastructure.Service.SaveLoad.Data
    {
        [SerializeField] private bool m_IsCompleted;

        public bool IsCompleted => m_IsCompleted;
        
        public override void WhenDataIsNew()
        {
            m_IsCompleted = false;
        }
    }
}