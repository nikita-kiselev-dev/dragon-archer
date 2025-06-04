namespace Core.View.Canvas.Scripts
{
    public interface ICanvasHandler
    {
        public IWindowCanvas WindowCanvas { get; }
        public IPopupCanvas PopupCanvas { get; }
    }
}