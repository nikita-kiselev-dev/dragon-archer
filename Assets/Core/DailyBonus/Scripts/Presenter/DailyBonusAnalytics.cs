using System.Collections.Generic;
using Core.Analytics;

namespace Core.DailyBonus.Scripts.Presenter
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
                {
                    DailyBonusConstants.PopupOpenEventParameterCurrentDay, 
                    currentDay
                },
            };

            var analyticsEvent = new AnalyticsEvent(
                DailyBonusConstants.PopupOpenEventName, 
                eventParameters);
            
            _analyticsManager.LogEvent(analyticsEvent);
        }

        public void LogStreakLose(int streakDay)
        {
            var eventParameters = new Dictionary<string, object>
            {
                { DailyBonusConstants.StreakLoseEventParameterStreakLoseDay, 
                    streakDay },
            };

            var analyticsEvent = new AnalyticsEvent(
                DailyBonusConstants.StreakLoseEventName, 
                eventParameters);
            
            _analyticsManager.LogEvent(analyticsEvent);
        }
    }
}