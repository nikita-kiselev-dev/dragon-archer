using Infrastructure.Service;
using Infrastructure.Service.View;
using Infrastructure.Service.View.ViewManager;

namespace Content.DailyBonus.Scripts.View
{
    public interface IDailyBonusView : IView, IDestroyable
    {
        public RewardRowsManager RewardRowsManager { get; }
    }
}