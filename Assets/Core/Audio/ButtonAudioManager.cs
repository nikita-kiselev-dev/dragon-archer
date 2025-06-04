using Core.Initialization.Scripts;
using Core.Initialization.Scripts.InitOrder;
using Core.Initialization.Scripts.Scopes;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Audio
{
    [ControlEntityOrder(nameof(StartScope), (int)StartSceneInitOrder.Last)]
    [ControlEntityOrder(nameof(CoreScope), (int)CoreSceneInitOrder.Last)]
    [ControlEntityOrder(nameof(MetaScope), (int)MetaSceneInitOrder.Last)]
    public class ButtonAudioManager : ControlEntity
    {
        protected override UniTask PostInit()
        {
            var sceneButtons = Object.FindObjectsByType<Button>(
                FindObjectsInactive.Include, 
                FindObjectsSortMode.None);
            
            foreach (var button in sceneButtons)
            {
                button.onClick.AddListener(PlayButtonSound);
            }
            
            return UniTask.CompletedTask;
        }

        private void PlayButtonSound()
        {
            AudioController.Instance.PlaySound(SoundList.ClickSound0);
        }
    }
}