namespace Infrastructure.Service.Asset
{
    public interface IOperationStatus
    {
        bool IsSuccessful { get; }
        bool IsCompleted { get; }
    }
}