﻿using UnityEngine;

namespace Content.DailyBonus.Scripts.View
{
    public interface IDailyBonusDayView
    {
        public void SetDayText(string text);
        public void SetItemSprite(Sprite sprite);
        public void SetItemCount(string text);
    }
}