using Content.DailyBonus.Scripts.Config;
using Content.DailyBonus.Scripts.View;
using Cysharp.Threading.Tasks;
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

        public async UniTask<IDailyBonusDayView> CreateDayView(IDailyBonusDayConfig config)
        {
            var view = await _assetLoader.InstantiateAsync<IDailyBonusDayView>(config.DayType, config.Parent);
            return view;
        }
    }
}