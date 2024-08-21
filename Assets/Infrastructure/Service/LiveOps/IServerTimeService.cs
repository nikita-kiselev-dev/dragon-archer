using System;

namespace Infrastructure.Service.LiveOps
{
    public interface IServerTimeService
    {
        public DateTime ServerTime { get; }
    }
}