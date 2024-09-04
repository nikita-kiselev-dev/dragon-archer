using System;
using Infrastructure.Service.SaveLoad;
using MemoryPack;

namespace Infrastructure.Game.Data
{
    [MemoryPackable]
    public partial class MainData : Service.SaveLoad.Data
    {
        [DataProperty] public DateTime FirstSessionLocalTime { get; private set; }
        [DataProperty] public DateTime LastSessionLocalTime { get; private set; }
        [DataProperty] public DateTime FirstSessionServerTime { get; private set; }
        [DataProperty] public DateTime LastSessionServerTime { get; private set; }
        
        
        public override void PrepareNewData()
        {
            FirstSessionLocalTime = DateTime.UnixEpoch;
            LastSessionLocalTime = DateTime.UnixEpoch;
            FirstSessionServerTime = DateTime.UnixEpoch;
            LastSessionServerTime = DateTime.UnixEpoch;
        }

        public void SetLocalTime(DateTime time)
        {
            if (FirstSessionLocalTime == DateTime.UnixEpoch)
            {
                FirstSessionLocalTime = time;
            }
            
            LastSessionLocalTime = time;
        }

        public void SetServerTime(DateTime time)
        {
            if (FirstSessionServerTime == DateTime.UnixEpoch)
            {
                FirstSessionServerTime = time;
            }
            
            LastSessionServerTime = time;
        }
    }
}