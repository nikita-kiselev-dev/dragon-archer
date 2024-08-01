using System.Collections.Generic;
using Content.DailyBonus.Scripts.Presenter;

namespace Content.DailyBonus.Scripts.Config
{
    public interface IDailyBonusDayConfigurator
    {
        public List<IDailyBonusDayController> GetConfiguredDayControllers();
    }
}