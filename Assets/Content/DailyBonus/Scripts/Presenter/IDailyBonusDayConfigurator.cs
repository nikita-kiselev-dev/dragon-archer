using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Content.DailyBonus.Scripts.Presenter
{
    public interface IDailyBonusDayConfigurator
    {
        public UniTask<List<IDailyBonusDayController>> GetConfiguredDayControllers();
    }
}