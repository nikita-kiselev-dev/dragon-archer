using Content.Items.Scripts;
using UnityEngine;

namespace Content.DailyBonus.Scripts.Config
{
    public interface IDailyBonusDayConfig
    {
        public string DayType { get; }
        public int DayNumber { get; }
        public IItem Item { get; }
        public int ItemCount { get; }
        public Transform Parent { get; }
    }
}