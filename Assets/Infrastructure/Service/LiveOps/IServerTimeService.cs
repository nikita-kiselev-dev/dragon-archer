using System;
using System.Threading.Tasks;

namespace Infrastructure.Service.LiveOps
{
    public interface IServerTimeService
    {
        public Task<DateTime> GetServerTime();
    }
}