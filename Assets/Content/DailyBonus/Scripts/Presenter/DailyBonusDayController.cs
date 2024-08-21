using Content.DailyBonus.Scripts.Config;
using Content.DailyBonus.Scripts.View;
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
            ConfigureView();
        }
        
        private void ConfigureView()
        {
            var dayText = _config.DayType == DailyBonusInfo.DailyBonusTodayLastDay
                ? "congratulations".LocalizeAsync() + "!"
                : _config.DayType != DailyBonusInfo.DailyBonusToday
                    ? "time/day".LocalizeAsync() + $" {_config.DayNumber}"
                    : "time/today".LocalizeAsync();

            _view.SetDayText(dayText);
            _view.SetItemCount($"x" + $"{_config.ItemCount}");
            _view.SetItemSprite(_config.ItemSprite);
        }
    }
}