using Content.Items.Scripts;
using UnityEngine;

namespace Content.DailyBonus.Scripts.Config
{
    public class DailyBonusDayConfig : IDailyBonusDayConfig
    {
        public string DayType { get; }
        public int DayNumber { get; }
        public IItem Item { get; }
        public int ItemCount { get; }
        public Transform Parent { get; }

        public DailyBonusDayConfig(
            string dayType,
            int dayNumber,
            IItem item,
            int itemCount,
            Transform parent)
        {
            DayType = dayType;
            DayNumber = dayNumber;
            Item = item;
            ItemCount = itemCount;
            Parent = parent;
        }
    }
}