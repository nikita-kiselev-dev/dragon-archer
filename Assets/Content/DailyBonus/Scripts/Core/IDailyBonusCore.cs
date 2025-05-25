using Cysharp.Threading.Tasks;

namespace Content.DailyBonus.Scripts.Core
{
    public interface IDailyBonusCore
    {
        public UniTask<bool> NeedToShowPopup();
        public UniTask GiveReward();
    }
}