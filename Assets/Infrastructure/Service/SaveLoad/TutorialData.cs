using MemoryPack;

namespace Infrastructure.Service.SaveLoad
{
    [MemoryPackable]
    public partial class TutorialData : Data
    { 
        [DataProperty] public bool IsTutorialCompleted { get; internal set; }

        public sealed override void PrepareNewData()
        {
            IsTutorialCompleted = false;
        }

        public void SetTutorialCompleted()
        {
            IsTutorialCompleted = true;
        }
    }
}