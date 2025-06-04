using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using VContainer;

namespace Core.Asset.IconController
{
    public class IconController : IIconController, IDisposable
    {
        [Inject] private readonly IAssetLoader _assetLoader;
        
        private Dictionary<string, SpriteAtlas> _icons = new();
        
        public async UniTask<Sprite> GetIcon(string iconName, string iconTypeName = null)
        {
            if (string.IsNullOrEmpty(iconTypeName))
            {
                iconTypeName = IconControllerConstants.DefaultIconType;
            }

            var atlasName = string.Format(IconControllerConstants.AtlasNameFormat, iconTypeName);
            
            if (_icons.TryGetValue(atlasName, out var iconAtlas))
            {
                return iconAtlas.GetSprite(iconName);
            }

            iconAtlas = await _assetLoader.LoadAsync<SpriteAtlas>(atlasName);
            _icons.Add(atlasName, iconAtlas);
            return iconAtlas.GetSprite(iconName);
        }

        void IDisposable.Dispose()
        {
            foreach (var icon in _icons)
            {
                _assetLoader.Release(icon.Key);
            }

            _icons.Clear();
            _icons = null;
        }
    }
}