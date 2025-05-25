using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Service.Asset.IconController
{
    public interface IIconController : IDisposable
    {
        UniTask<Sprite> GetIcon(string iconName, string iconTypeName = null);
    }
}