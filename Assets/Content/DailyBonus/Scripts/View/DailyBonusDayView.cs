using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Content.DailyBonus.Scripts.View
{
    public class DailyBonusDayView : MonoBehaviour, IDailyBonusDayView
    {
        [SerializeField] private TextMeshProUGUI m_DayText;
        
        [SerializeField] private Image m_ItemIcon;
        [SerializeField] private TextMeshProUGUI m_ItemCount;

        public void SetDayText(string text)
        {
            m_DayText.text = text;
        }

        public void SetItemIcon(Image icon)
        {
            m_ItemIcon = icon;
        }

        public void SetItemCount(string text)
        {
            m_ItemCount.text = text;
        }
    }
}