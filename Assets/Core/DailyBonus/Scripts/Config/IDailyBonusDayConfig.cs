using UnityEngine;

namespace Core.DailyBonus.Scripts.Config
{
    public interface IDailyBonusDayConfig
    {
        public string DayType { get; }
        public int DayNumber { get; }
        public string ItemName { get; }
        public int ItemCount { get; }
        public Sprite ItemSprite { get; }
        public Transform Parent { get; }
    }
}