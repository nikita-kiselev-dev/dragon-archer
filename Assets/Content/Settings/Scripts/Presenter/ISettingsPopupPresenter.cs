using Cysharp.Threading.Tasks;

namespace Content.Settings.Scripts.Presenter
{
    public interface ISettingsPopupPresenter
    {
        public bool IsInited { get; }
        public UniTaskVoid Init();
        public void Open();
        public void Close();
    }
}