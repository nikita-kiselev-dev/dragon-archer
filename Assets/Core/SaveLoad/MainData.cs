using System;
using Core.Logger;
using MemoryPack;

namespace Core.SaveLoad
{
    [MemoryPackable]
    public partial class MainData : Data
    {
        [DataProperty] public DateTime FirstSessionLocalTime { get; private set; }
        [DataProperty] public DateTime LastSessionLocalTime { get; private set; }
        [DataProperty] public bool IsFirstLocalLaunch { get; private set; }
        [DataProperty] public DateTime FirstSessionServerTime { get; private set; }
        [DataProperty] public DateTime LastSessionServerTime { get; private set; }
        [DataProperty] public bool IsFirstServerLaunch { get; private set; }

        private readonly ILogManager _logger = new LogManager(nameof(MainData));
        
        public override void PrepareNewData()
        {
            FirstSessionLocalTime = DateTime.UnixEpoch;
            LastSessionLocalTime = DateTime.UnixEpoch;
            FirstSessionServerTime = DateTime.UnixEpoch;
            LastSessionServerTime = DateTime.UnixEpoch;
        }

        public void SetLocalTime(DateTime time)
        {
            IsFirstLocalLaunch = false;
            
            if (FirstSessionLocalTime == DateTime.UnixEpoch)
            {
                FirstSessionLocalTime = time;
                _logger.Log($"{nameof(FirstSessionLocalTime)}: {time}");
            }
            
            if (LastSessionLocalTime != time)
            {
                LastSessionLocalTime = time;
                _logger.Log($"{nameof(LastSessionLocalTime)}: {time}");
            }
        }

        public void SetServerTime(DateTime time)
        {
            IsFirstServerLaunch = false;
            
            if (FirstSessionServerTime == DateTime.UnixEpoch)
            {
                FirstSessionServerTime = time;
                _logger.Log($"{nameof(FirstSessionServerTime)}: {time}");
            }

            if (LastSessionServerTime != time)
            {
                LastSessionServerTime = time;
                _logger.Log($"{nameof(LastSessionServerTime)}: {time}");
            }
        }
    }
}