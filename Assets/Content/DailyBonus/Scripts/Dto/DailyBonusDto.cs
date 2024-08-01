using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Content.DailyBonus.Scripts.Dto
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DailyBonusDto : IDailyBonusDto
    {
        [JsonProperty("streak_days")] private List<DailyBonusDayDto> _days;

        public IReadOnlyList<DailyBonusDayDto> Days => _days;
    }
}