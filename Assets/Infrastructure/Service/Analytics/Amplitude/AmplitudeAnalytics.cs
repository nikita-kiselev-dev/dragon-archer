using UnityEngine;

namespace Infrastructure.Service.Analytics.Amplitude
{
    public class AmplitudeAnalytics : IAnalyticsService
    {
        private readonly global::Amplitude _instance = global::Amplitude.getInstance();

        private bool _isInited;
        
        public void Init()
        {
            _instance.setServerUrl("https://api2.amplitude.com");
            _instance.logging = true;
            _instance.trackSessionEvents(true);
            _instance.init(AnalyticsInfo.AmplitudeApiKey);
            _isInited = true;
        }
        
        public void LogEvent(IAnalyticsEvent analyticsEvent)
        {
            if (_isInited)
            {
                _instance.logEvent(analyticsEvent.EventName, analyticsEvent.EventParameters);
            }
            else
            {
                Debug.LogError($"{GetType().Name}: initialize status: {_isInited}. " +
                               $"'{analyticsEvent.EventName}' event is not logged!");
            }
        }
    }
}