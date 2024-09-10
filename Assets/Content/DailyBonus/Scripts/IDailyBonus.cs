namespace Content.DailyBonus.Scripts
{
    public interface IDailyBonus
    {
        public bool IsInited { get; }
        public void Init();
    }
}