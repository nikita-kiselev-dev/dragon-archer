using System.Collections.Generic;

namespace Core.Analytics
{
    public interface IAnalyticsEvent
    {
        public string EventName { get; }
        public Dictionary<string, object> EventParameters { get; }
    }
}