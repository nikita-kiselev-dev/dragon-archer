using System.Collections.Generic;

namespace Core.Analytics
{
    public class AnalyticsEvent : IAnalyticsEvent
    {
        public string EventName { get; }
        public Dictionary<string, object> EventParameters { get; }

        public AnalyticsEvent(string eventName, Dictionary<string, object> eventParameters)
        {
            EventName = eventName;
            EventParameters = eventParameters;
        }
    }
}