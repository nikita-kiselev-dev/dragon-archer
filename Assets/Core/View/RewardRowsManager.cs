using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.View
{
    [Serializable]
    public class RewardRowsManager : IRewardRowsManager
    {
        [SerializeField, BoxGroup("Runtime Configuration")] private bool m_AllowRuntimeConfiguration;
        [SerializeField, BoxGroup("Runtime Configuration"), ShowIf("m_AllowRuntimeConfiguration")] private float m_ScaleFactor = 0.75f;
        [SerializeField, BoxGroup("Runtime Configuration"), ShowIf("m_AllowRuntimeConfiguration"), Range(1, 10)] private int m_ConfigurationStartAtRowCount;
        [Space]
        [SerializeField, BoxGroup("Reward Management"), Range(1, 10)] private int m_MaxRewardsInRow;
        [SerializeField, BoxGroup("Reward Management"), Required] private GameObject m_RowTemplate;
        [SerializeField, BoxGroup("Reward Management"), Required] private Transform m_RowsParent;
        [SerializeField, BoxGroup("Reward Management"), Required] private Transform m_LastRewardParent;

        private List<Transform> _rows;
        private int _rewardsInRow;

        public Transform GetRewardParent(int rewardIndex, int rewardCount)
        {
            _rows ??= ConfigureRows(rewardCount);
            _rewardsInRow = GetRewardsInRowCount(rewardCount);
            var rowIndex = GetRowIndex(rewardIndex);
            var row = _rows[rowIndex];
            
            if (row)
            {
                return row;
            }
            
            throw new NullReferenceException($"Can't find row in {GetType().Name}!");
        }

        public Transform GetLastRewardParent()
        {
            return m_LastRewardParent;
        }
        
        private List<Transform> ConfigureRows(int rewardCount)
        {
            var rowsNumber = CalculateRowsNumber(rewardCount);

            if (m_AllowRuntimeConfiguration)
            {
                ConfigureAtRuntime(rowsNumber);
                rowsNumber = CalculateRowsNumber(rewardCount);
            }
            
            _rows = new List<Transform> { m_RowTemplate.transform };

            for (var index = 0; index < rowsNumber - 1; index++)
            {
                _rows.Add(Object.Instantiate(m_RowTemplate, m_RowsParent).transform);
            }

            return _rows;
        }

        private int CalculateRowsNumber(int rewardCount)
        {
            var rowsNumber = (int)Math.Ceiling((float)rewardCount / m_MaxRewardsInRow);
            return rowsNumber;
        }

        private int GetRewardsInRowCount(int rewardCount)
        {
            if (_rewardsInRow != 0)
            {
               return _rewardsInRow;
            }
            
            var minRewardsInRow = (int)Math.Ceiling((float)rewardCount / _rows.Count);
            var rewardsInRow = Math.Min(minRewardsInRow, m_MaxRewardsInRow);
            _rewardsInRow = rewardsInRow == 0 ? 1 : rewardsInRow;

            return _rewardsInRow;
        }
        
        private int GetRowIndex(int rewardIndex)
        {
            var rowIndex = rewardIndex / _rewardsInRow;
            return rowIndex;
        }

        private void ConfigureAtRuntime(int rowsNumberBeforeConfiguration)
        {
            var value = rowsNumberBeforeConfiguration - m_ConfigurationStartAtRowCount;

            if (rowsNumberBeforeConfiguration < m_ConfigurationStartAtRowCount)
            {
                return;
            }
            
            for (var index = 0; index <= value; index++)
            {
                m_MaxRewardsInRow++;
                m_RowsParent.localScale *= m_ScaleFactor;
            }
        }
    }
}