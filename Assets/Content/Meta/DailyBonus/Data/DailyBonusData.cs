using System;
using Newtonsoft.Json;

namespace Content.Meta.DailyBonus.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DailyBonusData : Infrastructure.Service.SaveLoad.Data
    {
        public override void WhenDataIsNew()
        {
            throw new System.NotImplementedException();
        }
    }
}