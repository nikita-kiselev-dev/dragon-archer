using System;

namespace Content.DailyBonus.Scripts.Model
{
    public interface IDailyBonusModel
    {
        public bool IsFirstLaunch();
        public int GetStreakDay();
        public void AddStreakDay();
        public DateTime GetStartStreakTime();
        public void ResetData(DateTime startDate);
    }
}