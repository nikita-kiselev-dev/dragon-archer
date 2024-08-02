using UnityEngine;

namespace Content.DailyBonus.Scripts.Config
{
    public class DailyBonusDayConfig : IDailyBonusDayConfig
    {
        public string DayType { get; }
        public int DayNumber { get; }
        public string ItemName { get; }
        public int ItemCount { get; }
        public Sprite ItemSprite { get; }
        public Transform Parent { get; }

        public DailyBonusDayConfig(
            string dayType,
            int dayNumber,
            string itemName,
            int itemCount,
            Sprite itemSprite,
            Transform parent)
        {
            DayType = dayType;
            DayNumber = dayNumber;
            ItemName = itemName;
            ItemCount = itemCount;
            ItemSprite = itemSprite;
            Parent = parent;
        }
    }
}