using UnityEngine;

namespace Infrastructure.Service.View
{
    public interface IRewardRowsManager
    {
        public Transform GetRewardParent(int rewardIndex, int rewardCount);
        public Transform GetLastRewardParent();
    }
}