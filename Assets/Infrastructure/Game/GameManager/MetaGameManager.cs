using Content.Meta.DailyBonus;
using UnityEngine;
using VContainer;

namespace Infrastructure.Game.GameManager
{
    public class MetaGameManager : IMetaGameManager
    {
        [Inject] private readonly IDailyBonusController _dailyBonusController;
        
        public void OnSceneStart()
        {
            _dailyBonusController.Init();
            Debug.Log($"{GetType().Name}: start");
        }

        public void OnSceneExit()
        {
            Debug.Log($"{GetType().Name}: exit");
        }
    }
}