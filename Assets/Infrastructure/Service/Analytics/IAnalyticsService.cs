namespace Infrastructure.Service.Analytics
{
    public interface IAnalyticsService
    {
        public void Init();
        public void LogEvent(IAnalyticsEvent analyticsEvent);
    }
}