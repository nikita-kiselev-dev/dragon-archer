using System.Collections.Generic;

namespace Infrastructure.Service.LiveOps
{
    public interface IDtoService
    {
        public Dictionary<string, string> GetDto();
    }
}