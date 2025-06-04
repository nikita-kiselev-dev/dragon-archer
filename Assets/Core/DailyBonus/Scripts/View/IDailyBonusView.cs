using Core.View;
using Core.View.ViewManager;
using UnityEngine.Events;

namespace Core.DailyBonus.Scripts.View
{
    public abstract class IDailyBonusView : MonoView
    {
        public abstract void Init(UnityAction onCloseButtonClicked);
        public abstract RewardRowsManager RewardRowsManager { get; }
    }
}