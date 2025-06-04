using UnityEngine;

namespace Core.View
{
    public interface IRewardRowsManager
    {
        public Transform GetRewardParent(int rewardIndex, int rewardCount);
        public Transform GetLastRewardParent();
    }
}