using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Service.View
{
    [Serializable]
    public class RewardRowsManager : IRewardRowsManager
    {
        [SerializeField] private int m_RewardsInRow;
        [SerializeField] private GameObject m_RowTemplate;
        [SerializeField] private Transform m_RowsParent;

        private List<Transform> _rows;

        public Transform GetRewardParent(int rewardIndex, int rewardCount)
        {
            _rows ??= ConfigureRows(rewardCount);
            var row = _rows[GetRowIndex(rewardIndex)];
            
            if (row)
            {
                return row;
            }
            
            throw new NullReferenceException("Can't find row in RewardRowManager!");
        }
        
        private List<Transform> ConfigureRows(int rewardCount)
        {
            var rowsNumber = Math.Ceiling((float)rewardCount / m_RewardsInRow);

            _rows = new List<Transform> { m_RowTemplate.transform };

            for (var index = 0; index < rowsNumber - 1; index++)
            {
                _rows.Add(Object.Instantiate(m_RowTemplate, m_RowsParent).transform);
            }

            return _rows;
        }

        private int GetRowIndex(int rewardIndex)
        {
            return rewardIndex / m_RewardsInRow;
        }
    }
}