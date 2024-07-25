using Content.DailyBonus.Scripts.View;
using Infrastructure.Service.Asset;
using UnityEngine;

namespace Content.DailyBonus.Scripts.Presenter
{
    public class DailyBonusDayController : IDailyBonusDayController
    {
        private readonly IAssetLoader _assetLoader;
        private readonly Transform _parent;

        private IDailyBonusDayView _view;

        public DailyBonusDayController(IAssetLoader assetLoader, Transform parent)
        {
            _assetLoader = assetLoader;
            _parent = parent;
        }

        public void Init()
        {
            CreateView();
        }

        private void CreateView()
        {
            _view = _assetLoader.Instantiate<IDailyBonusDayView>(DailyBonusInfo.DailyBonusDay, _parent).Result;
        }
    }
}