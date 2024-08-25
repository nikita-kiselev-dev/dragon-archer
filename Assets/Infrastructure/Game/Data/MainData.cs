using Infrastructure.Service.LiveOps;
using MemoryPack;
using VContainer;

namespace Infrastructure.Game.Data
{
    [MemoryPackable]
    public partial class MainData : Service.SaveLoad.Data
    {
        [Inject] private IServerTimeService _serverTimeService;
        
        /*[MemoryPackInclude] public long FirstLaunchDateUtc { get; private set; }*/

        //TODO: transfer to main playfab data model
        public override void PrepareNewData()
        {
            /*var serverTime = _serverTimeService.ServerTime.GetAwaiter().GetResult();
            var convertedServerTime = _dateConverter.DateTimeToUnixTimeStamp(serverTime);
            m_FirstLaunchDateUtc = convertedServerTime;*/
        }
    }
}