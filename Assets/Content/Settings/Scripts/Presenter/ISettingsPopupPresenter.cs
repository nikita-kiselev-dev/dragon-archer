using Infrastructure.Service;

namespace Content.Settings.Scripts.Presenter
{
    public interface ISettingsPopupPresenter : IController
    {
        public void Open();
        public void Close();
    }
}