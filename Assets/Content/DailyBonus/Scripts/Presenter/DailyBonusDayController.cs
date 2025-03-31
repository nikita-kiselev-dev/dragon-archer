using Content.DailyBonus.Scripts.Config;
using Content.DailyBonus.Scripts.View;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Localization;

namespace Content.DailyBonus.Scripts.Presenter
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
                ? await "congratulations".LocalizeAsync() + "!"
                : _config.DayType != DailyBonusConstants.Today
                    ? await "time/day".LocalizeAsync() + $" {_config.DayNumber}"
                    : await "time/today".LocalizeAsync();

            _view.SetDayText(dayText);
            _view.SetItemCount($"x" + $"{_config.ItemCount}");
            _view.SetItemSprite(_config.ItemSprite);
        }
    }
}