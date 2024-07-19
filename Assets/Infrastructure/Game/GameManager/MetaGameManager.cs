using UnityEngine;

namespace Infrastructure.Game.GameManager
{
    public class MetaGameManager : IMetaGameManager
    {
        public void OnSceneStart()
        {
            Debug.Log("Meta Game Start");
        }

        public void OnSceneExit()
        {
            Debug.Log("Meta Game Exit");
        }
    }
}