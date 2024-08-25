using System.Collections.Generic;
using Infrastructure.Service.Analytics;

namespace Content.DailyBonus.Scripts.Presenter
{
    public class DailyBonusAnalytics : IDailyBonusAnalytics
    {
        private readonly IAnalyticsManager _analyticsManager;
        
        public DailyBonusAnalytics(IAnalyticsManager analyticsManager)
        {
            _analyticsManager = analyticsManager;
        }

        public void LogPopupOpen(int currentDay)
        {
            var eventParameters = new Dictionary<string, object>
            {
                { "current_day", currentDay },
            };

            var analyticsEvent = new AnalyticsEvent(
                "daily_bonus_popup_open", 
                eventParameters);
            
            _analyticsManager.LogEvent(analyticsEvent);
        }

        public void LogStreakLose(int streakDay)
        {
            var eventParameters = new Dictionary<string, object>
            {
                { "streak_lose_day", streakDay },
            };


            var analyticsEvent = new AnalyticsEvent(
                "daily_bonus_streak_lose", 
                eventParameters);
            
            _analyticsManager.LogEvent(analyticsEvent);
        }
    }
}