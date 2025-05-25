using System;
using GamePush;

namespace Infrastructure.Service.LiveOps.GamePush
{
    public class GamePushServerTimeService 
    {
        public DateTime GetServerTime()
        {
            return GP_Server.Time();
        }
    }
}