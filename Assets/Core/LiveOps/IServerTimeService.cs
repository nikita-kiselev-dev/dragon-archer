using System;
using Cysharp.Threading.Tasks;

namespace Core.LiveOps
{
    public interface IServerTimeService
    {
        public UniTask<DateTime> GetServerTime();
    }
}