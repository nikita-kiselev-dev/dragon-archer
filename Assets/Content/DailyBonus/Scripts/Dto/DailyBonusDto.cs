﻿using System;
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
            if (_sortedDays == null)
            {
                _sortedDays = _days.OrderBy(config => config.StreakDay).ToList();
                return _sortedDays;
            }
            else
            {
                return _sortedDays;
            }
        }
    }
}