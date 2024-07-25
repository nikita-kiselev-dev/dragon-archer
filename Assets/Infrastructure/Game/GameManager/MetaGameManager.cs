using Content.DailyBonus.Scripts;
using UnityEngine;
using VContainer;

namespace Infrastructure.Game.GameManager
{
    public class MetaGameManager : IMetaGameManager
    {
        [Inject] private readonly IDailyBonus _dailyBonus;
        
        public void OnSceneStart()
        {
            _dailyBonus.Init();
            Debug.Log($"{GetType().Name}: start");
        }

        public void OnSceneExit()
        {
            Debug.Log($"{GetType().Name}: exit");
        }
    }
}