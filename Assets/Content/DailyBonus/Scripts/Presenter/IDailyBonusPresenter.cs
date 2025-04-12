using Cysharp.Threading.Tasks;
using Infrastructure.Service.Controller;

namespace Content.DailyBonus.Scripts.Presenter
{
    public interface IDailyBonusPresenter : IController
    {
        UniTask Init();
    }
}