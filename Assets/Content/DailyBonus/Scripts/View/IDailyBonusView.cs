using Infrastructure.Service.View;
using Infrastructure.Service.View.ViewManager;

namespace Content.DailyBonus.Scripts.View
{
    public abstract class IDailyBonusView : MonoView
    {
        public abstract RewardRowsManager RewardRowsManager { get; }
    }
}