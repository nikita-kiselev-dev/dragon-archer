namespace Infrastructure.Service.LiveOps
{
    public interface IServerConnectionService
    {
        public bool IsConnectedToServer { get; }
    }
}