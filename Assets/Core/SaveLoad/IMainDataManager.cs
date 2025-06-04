using System;

namespace Core.SaveLoad
{
    public interface IMainDataManager
    {
        public DateTime LastSessionServerTime { get; }
        public bool IsFirstServerLaunch();
        public void SetLocalTime(DateTime time);
    }
}