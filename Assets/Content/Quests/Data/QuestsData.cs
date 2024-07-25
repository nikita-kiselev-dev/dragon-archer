using System;
using Newtonsoft.Json;

namespace Content.Quests.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class QuestsData : Infrastructure.Service.SaveLoad.Data
    {
        public override void WhenDataIsNew()
        {
            throw new System.NotImplementedException();
        }
    }
}