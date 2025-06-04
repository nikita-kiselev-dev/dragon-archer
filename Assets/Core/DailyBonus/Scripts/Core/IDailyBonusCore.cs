using Cysharp.Threading.Tasks;

namespace Core.DailyBonus.Scripts.Core
{
    public interface IDailyBonusCore
    {
        public UniTask<bool> NeedToShowPopup();
        public UniTask GiveReward();
    }
}