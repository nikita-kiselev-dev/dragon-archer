using System;
using Cysharp.Threading.Tasks;

namespace Infrastructure.Game.Data
{
    public interface IMainDataManager
    {
        public DateTime LastSessionServerTime { get; }
        public bool IsFirstServerLaunch();
        public void SetLocalTime(DateTime time);
    }
}