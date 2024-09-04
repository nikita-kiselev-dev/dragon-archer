using System.Collections.Generic;
using Infrastructure.Service.Dto;

namespace Content.DailyBonus.Scripts.Dto
{
    public interface IDailyBonusDto : IDto
    {
        public IReadOnlyList<DailyBonusDayDto> GetDays();
        public DailyBonusDayDto GetDay(int streakDay);
        public DailyBonusDayDto GetNextDay(int streakDay);
        public DailyBonusDayDto GetLastDay();
    }
}