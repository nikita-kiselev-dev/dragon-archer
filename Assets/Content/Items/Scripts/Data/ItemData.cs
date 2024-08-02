using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Content.Items.Scripts.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class ItemData : Infrastructure.Service.SaveLoad.Data
    {
        [SerializeField] private int m_ItemCount;

        public int ItemCount => m_ItemCount;
        
        public override void WhenDataIsNew()
        {
            m_ItemCount = 0;
        }

        public bool AddItem(int itemCount)
        {
            m_ItemCount += itemCount;
            return true;
        }

        public bool RemoveItem(int itemCount)
        {
            var result = m_ItemCount - itemCount > 0;
            m_ItemCount = result ? m_ItemCount - itemCount : 0;
            return true;
        }
    }
}