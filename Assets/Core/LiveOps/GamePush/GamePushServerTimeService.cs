using System;
using GamePush;

namespace Core.LiveOps.GamePush
{
    public class GamePushServerTimeService 
    {
        public DateTime GetServerTime()
        {
            return GP_Server.Time();
        }
    }
}