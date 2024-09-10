using Cysharp.Threading.Tasks;

namespace Content.DailyBonus.Scripts
{
    public interface IDailyBonus
    {
        public bool IsInited { get; }
        public UniTaskVoid Init();
    }
}