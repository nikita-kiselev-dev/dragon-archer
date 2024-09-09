using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Content.DailyBonus.Scripts.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DailyBonusDto : IDailyBonusDto
    {
        [JsonProperty("streak_days")] private List<DailyBonusDayDto> _days;
        
        [NonSerialized] private List<DailyBonusDayDto> _sortedDays;

        public IReadOnlyList<DailyBonusDayDto> GetDays()
        {
            return _sortedDays ??= _days.OrderBy(config => config.StreakDay).ToList();
        }

        public DailyBonusDayDto GetDay(int streakDay)
        {
            GetDays();
            var dayDto = _sortedDays.FirstOrDefault(day => day.StreakDay == streakDay);
            return dayDto;
        }

        public DailyBonusDayDto GetLastDay()
        {
            GetDays();
            return _sortedDays.Last();
        }
    }
}