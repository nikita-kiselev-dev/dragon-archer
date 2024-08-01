using UnityEngine.UI;

namespace Content.DailyBonus.Scripts.View
{
    public interface IDailyBonusDayView
    {
        public void SetDayText(string text);
        public void SetItemIcon(Image image);
        public void SetItemCount(string text);
    }
}