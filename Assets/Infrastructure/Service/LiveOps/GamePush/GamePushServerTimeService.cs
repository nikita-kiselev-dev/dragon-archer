using System;
using GamePush;

namespace Infrastructure.Service.LiveOps.GamePush
{
    public class GamePushServerTimeService 
    {
        public DateTime GetServerTime()
        {
            var serverTime = GP_Server.Time().ToUniversalTime();
            return serverTime;
        }
    }
}