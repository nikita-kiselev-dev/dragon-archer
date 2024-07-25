using System;
using Content.DailyBonus.Scripts.Data;
using Infrastructure.Service.Date;

namespace Content.DailyBonus.Scripts.Model
{
    public class DailyBonusModel : IDailyBonusModel
    {
        private readonly DailyBonusData _data;
        private readonly IDateConverter _dateConverter;
        
        public DailyBonusModel(DailyBonusData data, IDateConverter dateConverter)
        {
            _data = data;
            _dateConverter = dateConverter;
        }

        public int GetStreakDay()
        {
            return _data.StreakDay;
        }

        public void AddStreakDay()
        {
            _data.AddStreakDayData();
        }

        public DateTime GetStartStreakData()
        {
            var convertedDate = _dateConverter.UnixTimeStampToDateTime(_data.StartStreakDate);
            return convertedDate;
        }

        public void ResetData(DateTime startDate)
        {
            _data.ResetStreak();
            SetStartStreakData(startDate);
        }
        
        private void SetStartStreakData(DateTime startDate)
        {
            var convertedDate = _dateConverter.DateTimeToUnixTimeStamp(startDate);
            _data.SetStartStreakDateData(convertedDate);
        }
    }
}