namespace Core.Analytics
{
    public interface IAnalyticsManager
    {
        public void LogEvent(IAnalyticsEvent analyticsEvent);
        public void LogEvent<T>(IAnalyticsEvent analyticsEvent) where T : IAnalyticsService;
    }
}