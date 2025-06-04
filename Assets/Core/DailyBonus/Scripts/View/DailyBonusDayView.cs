using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.DailyBonus.Scripts.View
{
    public class DailyBonusDayView : UnityEngine.MonoBehaviour, IDailyBonusDayView
    {
        [SerializeField] private TextMeshProUGUI m_DayText;
        
        [SerializeField] private Image m_ItemIcon;
        [SerializeField] private TextMeshProUGUI m_ItemCount;

        public void SetDayText(string text)
        {
            m_DayText.text = text;
        }

        public void SetItemSprite(Sprite sprite)
        {
            m_ItemIcon.sprite = sprite;
        }

        public void SetItemCount(string text)
        {
            m_ItemCount.text = text;
        }
    }
}