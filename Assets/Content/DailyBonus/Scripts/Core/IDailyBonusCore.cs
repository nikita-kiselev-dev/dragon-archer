using System.Threading.Tasks;

namespace Content.DailyBonus.Scripts.Core
{
    public interface IDailyBonusCore
    {
        public Task<bool> NeedToShowPopup();
        public void GetStreakReward();
    }
}