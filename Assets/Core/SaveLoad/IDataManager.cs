namespace Core.SaveLoad
{
    public interface IDataManager
    {
        public IDataRepository DataRepository { get; }
        public void SetDataRepository(IDataRepository dataRepository);
        public void PrepareNewData();
    }
}