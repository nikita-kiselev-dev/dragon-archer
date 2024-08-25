namespace Content.DailyBonus.Scripts
{
    public static class DailyBonusInfo
    {
        public const string Popup = "DailyBonusPopup";
        
        public const string PreviousDay = "DailyBonusPreviousDay";
        public const string Today = "DailyBonusToday";
        public const string NextDay = "DailyBonusNextDay";
        public const string LastDay = "DailyBonusLastDay";
        public const string TodayLastDay = "DailyBonusTodayLastDay";
        
        public const string Config = "DailyBonusConfig";

        public const string PopupOpenEventName = "daily_bonus_popup_open";
        public const string PopupOpenEventParameterCurrentDay = "current_day";

        public const string StreakLoseEventName = "daily_bonus_streak_lose";
        public const string StreakLoseEventParameterStreakLoseDay = "streak_lose_day";
    }
}