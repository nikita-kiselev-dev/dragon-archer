namespace Content.DailyBonus.Scripts.Presenter
{
    public interface IDailyBonusAnalytics
    {
        public void LogPopupOpen(int currentDay);
        public void LogStreakLose(int streakDay);
    }
}