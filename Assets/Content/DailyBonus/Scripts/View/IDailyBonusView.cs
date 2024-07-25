using Infrastructure.Service.View;
using Infrastructure.Service.View.ViewManager;

namespace Content.DailyBonus.Scripts.View
{
    public interface IDailyBonusView : IView
    {
        public RewardRowsManager RewardRowsManager { get; }
    }
}