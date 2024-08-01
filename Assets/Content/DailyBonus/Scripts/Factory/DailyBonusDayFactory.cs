using Content.DailyBonus.Scripts.Config;
using Content.DailyBonus.Scripts.View;
using Infrastructure.Service.Asset;

namespace Content.DailyBonus.Scripts.Factory
{
    public class DailyBonusDayFactory : IDailyBonusDayFactory
    {
        private readonly IAssetLoader _assetLoader;
        
        public DailyBonusDayFactory(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public IDailyBonusDayView CreateDayView(IDailyBonusDayConfig config)
        {
            var view = _assetLoader.Instantiate<IDailyBonusDayView>(config.DayType, config.Parent).Result;
            return view;
        }
    }
}