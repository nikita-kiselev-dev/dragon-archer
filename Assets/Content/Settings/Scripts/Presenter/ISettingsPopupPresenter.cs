using Infrastructure.Service;

namespace Content.Settings.Scripts.Presenter
{
    public interface ISettingsPopupPresenter : IInitiable
    {
        public void Open();
        public void Close();
    }
}