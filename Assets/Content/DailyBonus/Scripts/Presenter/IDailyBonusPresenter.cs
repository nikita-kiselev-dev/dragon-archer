using Infrastructure.Service;

namespace Content.DailyBonus.Scripts.Presenter
{
    public interface IDailyBonusPresenter : IController
    {
        public void Open();
        public void Close();
    }
}