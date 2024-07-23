using Infrastructure.Service.SaveLoad;

namespace Infrastructure.Game.Tutorials
{
    public interface ITutorialService
    {
        public bool IsTutorialCompleted<T>() where T : TutorialData;
    }
}