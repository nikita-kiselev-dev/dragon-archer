using System;
using Infrastructure.Service.LiveOps;
using UnityEngine;
using VContainer;

namespace Content.Meta.DailyBonus
{
    public class DailyBonusController : IDailyBonusController
    {
        [Inject] private IServerTimeService _serverTimeService;
        
        private bool isLoggedIn = false;
        
        public void Init()
        {
            _serverTimeService.GetServerTime(TimeLoaded);
        }

        public void TimeLoaded(DateTime time)
        {
            Debug.Log($"{time}");
        }
    }
}