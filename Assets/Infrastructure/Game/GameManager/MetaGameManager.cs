using Content.DailyBonus.Scripts;
using Infrastructure.Service.LiveOps;
using UnityEngine;
using VContainer;

namespace Infrastructure.Game.GameManager
{
    public class MetaGameManager : IMetaGameManager
    {
        [Inject] private readonly IServerConnectionService _serverConnectionService;
        [Inject] private readonly IDailyBonus _dailyBonus;
        
        public void OnSceneStart()
        {
            if (_serverConnectionService.IsConnectedToServer)
            {
                _dailyBonus.Init();
            }
            
            Debug.Log($"{GetType().Name}: start");
        }

        public void OnSceneExit()
        {
            Debug.Log($"{GetType().Name}: exit");
        }
    }
}