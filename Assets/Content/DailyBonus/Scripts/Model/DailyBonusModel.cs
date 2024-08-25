using System;
using Content.DailyBonus.Scripts.Data;

namespace Content.DailyBonus.Scripts.Model
{
    public class DailyBonusModel : IDailyBonusModel
    {
        private readonly DailyBonusData _data;
        
        public DailyBonusModel(DailyBonusData data)
        {
            _data = data;
        }

        public bool IsFirstLaunch()
        {
            return _data.IsFirstLaunch();
        }

        public int GetStreakDay()
        {
            return _data.StreakDay;
        }

        public void AddStreakDay()
        {
            _data.AddStreakDayData();
        }

        public DateTime GetStartStreakTime()
        {
            return _data.StartStreakDate;
        }

        public void ResetData(DateTime startDate)
        {
            _data.ResetStreak(startDate);
        }
    }
}