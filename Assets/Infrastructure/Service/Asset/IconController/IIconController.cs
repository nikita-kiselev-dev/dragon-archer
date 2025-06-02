using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Service.Asset.IconController
{
    public interface IIconController
    {
        UniTask<Sprite> GetIcon(string iconName, string iconTypeName = null);
    }
}