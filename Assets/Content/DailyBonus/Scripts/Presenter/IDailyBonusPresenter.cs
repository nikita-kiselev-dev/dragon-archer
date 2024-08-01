using Infrastructure.Service;

namespace Content.DailyBonus.Scripts.Presenter
{
    public interface IDailyBonusPresenter : IInitiable
    {
        public void Open();
        public void Close();
    }
}