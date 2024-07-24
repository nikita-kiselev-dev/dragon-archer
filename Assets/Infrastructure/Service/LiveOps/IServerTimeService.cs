using System;

namespace Infrastructure.Service.LiveOps
{
    public interface IServerTimeService
    {
        public void GetServerTime(Action<DateTime> callback);
    }
}