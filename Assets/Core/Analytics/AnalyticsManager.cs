using System;
using System.Collections.Generic;
using Core.Analytics.Amplitude;
using VContainer;

namespace Core.Analytics
{
    public class AnalyticsManager : IAnalyticsManager
    {
        private readonly Dictionary<Type, IAnalyticsService> _analyticsServices = new();

        [Inject]
        private AnalyticsManager(IReadOnlyList<IAnalyticsService> injectedAnalyticsServices)
        {
            foreach (var analyticsService in injectedAnalyticsServices)
            {
                _analyticsServices[analyticsService.GetType()] = analyticsService;
            }

            Init();
        }

        public void LogEvent(IAnalyticsEvent analyticsEvent)
        {
            foreach (var analyticsService in _analyticsServices.Values)
            {
                analyticsService.LogEvent(analyticsEvent);
            }
        }

        public void LogEvent<T>(IAnalyticsEvent analyticsEvent) where T : IAnalyticsService
        {
            var type = typeof(T);
            _analyticsServices[type]?.LogEvent(analyticsEvent);
        }

        private void Init()
        {
            _analyticsServices[typeof(AmplitudeAnalytics)].Init();
        }
    }
}