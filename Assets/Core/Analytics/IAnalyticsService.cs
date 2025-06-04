namespace Core.Analytics
{
    public interface IAnalyticsService
    {
        public void Init();
        public void LogEvent(IAnalyticsEvent analyticsEvent);
    }
}