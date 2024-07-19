using System;

namespace Infrastructure.Service.View.ViewManager
{
    public interface IViewWrapper
    {
        public string ViewKey { get; set; }
        public string ViewType { get; set; }
        public IMonoBehaviour View { get; set; }
        public bool IsEnabledOnStart { get; set; }
        public Action CustomOpenAnimation { get; set; }
        public Action CustomCloseAnimation { get; set; }
    }
}