using Cysharp.Threading.Tasks;

namespace Content.DailyBonus.Scripts.Presenter
{
    public interface IDailyBonusPresenter
    {
        public bool IsInited { get; }
        public UniTaskVoid Init();
    }
}