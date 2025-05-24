using Infrastructure.Service.View;
using Infrastructure.Service.View.ViewManager;
using UnityEngine.Events;

namespace Content.DailyBonus.Scripts.View
{
    public abstract class IDailyBonusView : MonoView
    {
        public abstract void Init(UnityAction onCloseButtonClicked);
        public abstract RewardRowsManager RewardRowsManager { get; }
    }
}