using System.Collections.Generic;

namespace Content.DailyBonus.Scripts.Presenter
{
    public interface IDailyBonusDayConfigurator
    {
        public List<IDailyBonusDayController> GetConfiguredDayControllers();
    }
}