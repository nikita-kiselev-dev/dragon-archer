namespace Infrastructure.Service.View.ViewManager
{
    public class WindowViewTypeManager : IViewTypeManager
    {
        public bool Open(IViewWrapper viewWrapper, bool viewIsOpen)
        {
            viewWrapper.ViewAnimator.Show();
            return true;
        }

        public bool Close(IViewWrapper viewWrapper)
        {
            viewWrapper.ViewAnimator.Hide();
            return false;
        }
    }
}