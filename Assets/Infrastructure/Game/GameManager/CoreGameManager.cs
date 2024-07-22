using Infrastructure.Service.Audio;

namespace Infrastructure.Game.GameManager
{
    public class CoreGameManager : ICoreGameManager
    {
        
        public void OnSceneStart()
        {
            AudioService.Instance.PlayMusic(MusicList.CoreSceneMusic);
        }

        public void OnSceneExit()
        {
            
        }
    }
}