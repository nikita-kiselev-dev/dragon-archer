using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Infrastructure.Service.SaveLoad
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class TutorialData : Data
    {
        [SerializeField] private bool m_IsTutorialCompleted;

        public bool IsTutorialCompleted => m_IsTutorialCompleted;

        public sealed override void WhenDataIsNew()
        {
            m_IsTutorialCompleted = false;
        }
    }
}