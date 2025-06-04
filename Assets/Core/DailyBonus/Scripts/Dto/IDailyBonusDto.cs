using System.Collections.Generic;
using Core.Dto;

namespace Core.DailyBonus.Scripts.Dto
{
    public interface IDailyBonusDto : IDto
    {
        public IReadOnlyList<DailyBonusDayDto> GetDays();
        public DailyBonusDayDto GetDay(int streakDay);
        public DailyBonusDayDto GetLastDay();
    }
}