using System;
using System.Collections.Generic;
using Core.Logger;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using VContainer;

namespace Core.Asset.IconController
{
    public class IconController : IIconController, IDisposable
    {
        [Inject] private readonly IAssetLoader _assetLoader;
        
        private ILogManager _logger = new LogManager(nameof(IconController));

        private Dictionary<string, SpriteAtlas> _icons = new();

        public IconController()
        {
            SpriteAtlasManager.atlasRequested += OnAtlasRequested;
        }

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

        private void OnAtlasRequested(string atlasName, Action<SpriteAtlas> callback)
        {
            if (_icons.TryGetValue(atlasName, out var cachedAtlas))
            {
                callback?.Invoke(cachedAtlas);
                return;
            }
            
            _assetLoader.LoadAsync<SpriteAtlas>(atlasName).ContinueWith(loadedAtlas =>
            {
                if (loadedAtlas != null)
                {
                    _icons[atlasName] = loadedAtlas;
                    callback?.Invoke(loadedAtlas);
                }
                else
                {
                    _logger.LogError($"Failed to load sprite atlas: {atlasName}.");
                }
            }).Forget();
        }

        void IDisposable.Dispose()
        {
            SpriteAtlasManager.atlasRequested -= OnAtlasRequested;

            foreach (var icon in _icons)
            {
                _assetLoader.Release(icon.Key);
            }

            _icons.Clear();
            _icons = null;
        }
    }
}
