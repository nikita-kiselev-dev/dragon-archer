using System.Collections.Generic;

namespace Infrastructure.Service.Analytics
{
    public interface IAnalyticsEvent
    {
        public string EventName { get; }
        public Dictionary<string, object> EventParameters { get; }
    }
}