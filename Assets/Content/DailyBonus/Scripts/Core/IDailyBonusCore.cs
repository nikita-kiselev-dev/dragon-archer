namespace Content.DailyBonus.Scripts.Core
{
    public interface IDailyBonusCore
    {
        public bool NeedToShowPopup();
        public void GetStreakReward();
    }
}