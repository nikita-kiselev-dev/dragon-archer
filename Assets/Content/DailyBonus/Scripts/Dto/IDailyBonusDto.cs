﻿using System.Collections.Generic;
using Infrastructure.Service.Dto;

namespace Content.DailyBonus.Scripts.Dto
{
    public interface IDailyBonusDto : IDto
    {
        public IReadOnlyList<DailyBonusDayDto> GetDays();
    }
}