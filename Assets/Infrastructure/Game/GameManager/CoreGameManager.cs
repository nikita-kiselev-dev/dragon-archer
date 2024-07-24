using Infrastructure.Service.Audio;
using UnityEngine;

namespace Infrastructure.Game.GameManager
{
    public class CoreGameManager : ICoreGameManager
    {
        
        public void OnSceneStart()
        {
            AudioService.Instance.PlayMusic(MusicList.CoreSceneMusic);
            Debug.Log($"{GetType().Name}: start");
        }

        public void OnSceneExit()
        {
            Debug.Log($"{GetType().Name}: exit");
        }
    }
}