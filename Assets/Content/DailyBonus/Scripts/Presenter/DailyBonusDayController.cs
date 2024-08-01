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
        
        private async void ConfigureView()
        {
            var dayText = _config.DayType != DailyBonusInfo.DailyBonusToday
                ? await "time/day".LocalizeAsync() + $" {_config.DayNumber}"
                : await "time/today".LocalizeAsync();
            
            _view.SetDayText(dayText);
            _view.SetItemCount($"x" + $"{_config.ItemCount}");
        }
    }
}