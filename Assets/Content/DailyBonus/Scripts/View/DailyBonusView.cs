﻿using Infrastructure.Service.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Content.DailyBonus.Scripts.View
{
    public class DailyBonusView : IDailyBonusView
    {
        [SerializeField] private Button m_CloseButton;
        [SerializeField] private RewardRowsManager m_RewardRowsManager;

        public override RewardRowsManager RewardRowsManager => m_RewardRowsManager;
        
        public override void Init(UnityAction onCloseButtonClicked)
        {
            ConfigureButtons(onCloseButtonClicked);
        }

        private void ConfigureButtons(UnityAction onCloseButtonClicked)
        {
            m_CloseButton.onClick.RemoveAllListeners();
            m_CloseButton.onClick.AddListener(onCloseButtonClicked);
        }
    }
}