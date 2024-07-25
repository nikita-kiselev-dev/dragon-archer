using System;
using Infrastructure.Service.Date;
using Infrastructure.Service.LiveOps;
using Newtonsoft.Json;
using UnityEngine;
using VContainer;

namespace Infrastructure.Game.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class MainData : Service.SaveLoad.Data
    {
        [Inject] private IServerTimeService _serverTimeService;
        [Inject] private IDateConverter _dateConverter;
        
        [SerializeField] private long m_FirstLaunchDateUtc;

        public long FirstLaunchDateUtc => m_FirstLaunchDateUtc;
        
        //TODO: transfer to main playfab data model
        public override async void WhenDataIsNew()
        {
            var serverTime = await _serverTimeService.GetServerTime();
            var convertedServerTime = _dateConverter.DateTimeToUnixTimeStamp(serverTime);
            m_FirstLaunchDateUtc = convertedServerTime;
        }
    }
}