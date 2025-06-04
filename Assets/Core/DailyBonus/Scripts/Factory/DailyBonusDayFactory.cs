using Core.Asset;
using Core.DailyBonus.Scripts.Config;
using Core.DailyBonus.Scripts.View;
using Cysharp.Threading.Tasks;

namespace Core.DailyBonus.Scripts.Factory
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