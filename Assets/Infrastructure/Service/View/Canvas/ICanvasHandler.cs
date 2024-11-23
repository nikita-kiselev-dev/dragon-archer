namespace Infrastructure.Service.View.Canvas
{
    public interface ICanvasHandler
    {
        public IWindowCanvas WindowCanvas { get; }
        public IPopupCanvas PopupCanvas { get; }
    }
}