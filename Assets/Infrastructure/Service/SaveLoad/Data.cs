namespace Infrastructure.Service.SaveLoad
{
    public abstract class Data
    {
        public string Name()
        {
            return GetType().Name;
        }

        public abstract void WhenDataIsNew();
    }
}