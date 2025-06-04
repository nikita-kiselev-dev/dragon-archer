using Core.Controller;
using Cysharp.Threading.Tasks;

namespace Core.DailyBonus.Scripts.Presenter
{
    public interface IDailyBonusPresenter : IController
    {
        UniTask Init();
    }
}