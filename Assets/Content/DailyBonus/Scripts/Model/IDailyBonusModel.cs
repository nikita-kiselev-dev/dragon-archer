using System;

namespace Content.DailyBonus.Scripts.Model
{
    public interface IDailyBonusModel
    {
        public int GetStreakDay();
        public void AddStreakDay();
        public void SetStartStreakData(DateTime startDate);
        public DateTime GetStartStreakData();
        public void ResetData(DateTime startDate);
    }
}