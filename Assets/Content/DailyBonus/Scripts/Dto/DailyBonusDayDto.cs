using System;
using Newtonsoft.Json;

namespace Content.DailyBonus.Scripts.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DailyBonusDayDto
    {
        [JsonProperty("streak_day")] private int _streakDay;
        [JsonProperty("item")] private string _item;
        [JsonProperty("item_icon")] private string _itemIcon;
        [JsonProperty("item_count")] private int _itemCount;

        public int StreakDay => _streakDay;
        public string Item => _item;
        public string ItemIcon => _itemIcon;
        public int ItemCount => _itemCount;
    }
}