using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Content.DailyBonus.Scripts.View
{
    public class DailyBonusDayView : MonoBehaviour, IDailyBonusDayView
    {
        [SerializeField] private TextMeshProUGUI m_DayText;
        [SerializeField] private Image m_RewardImage;

        public void SetDayText(string text)
        {
            m_DayText.text = text;
        }

        public void SetRewardImage(Image image)
        {
            m_RewardImage = image;
        }
    }
}