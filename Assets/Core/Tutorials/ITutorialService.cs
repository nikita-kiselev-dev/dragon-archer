using Core.SaveLoad;

namespace Core.Tutorials
{
    public interface ITutorialService
    {
        public bool IsTutorialCompleted<T>() where T : TutorialData;
    }
}