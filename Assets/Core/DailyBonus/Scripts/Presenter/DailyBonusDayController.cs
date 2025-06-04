using Core.DailyBonus.Scripts.Config;
using Core.DailyBonus.Scripts.View;
using Core.Localization;
using Cysharp.Threading.Tasks;

namespace Core.DailyBonus.Scripts.Presenter
{
    public class DailyBonusDayController : IDailyBonusDayController
    {
        private readonly IDailyBonusDayView _view;
        private readonly IDailyBonusDayConfig _config;
        
        public DailyBonusDayController(IDailyBonusDayView view, IDailyBonusDayConfig config)
        {
            _view = view;
            _config = config;
        }

        public void Init()
        {
            ConfigureView().Forget();
        }
        
        private async UniTaskVoid ConfigureView()
        {
            var dayText = _config.DayType == DailyBonusConstants.TodayLastDay
                ? await "congratulations".Localize() + "!"
                : _config.DayType != DailyBonusConstants.Today
                    ? await "time/day".Localize() + $" {_config.DayNumber}"
                    : await "time/today".Localize();

            _view.SetDayText(dayText);
            _view.SetItemCount($"x" + $"{_config.ItemCount}");
            _view.SetItemSprite(_config.ItemSprite);
        }
    }
}