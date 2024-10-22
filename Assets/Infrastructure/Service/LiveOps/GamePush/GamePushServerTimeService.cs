using System;

namespace Infrastructure.Service.LiveOps.GamePush
{
    public class GamePushServerTimeService 
    {
        public DateTime GetServerTime()
        {
            //var serverTime = GamePushService.Time().ToUniversalTime();
            return DateTime.Today;
        }
    }
}