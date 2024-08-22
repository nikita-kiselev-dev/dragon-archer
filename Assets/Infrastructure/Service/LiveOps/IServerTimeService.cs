using System;
using Cysharp.Threading.Tasks;

namespace Infrastructure.Service.LiveOps
{
    public interface IServerTimeService
    {
        public UniTask<DateTime> GetServerTime();
    }
}