using Cysharp.Threading.Tasks;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Initialization.InitOrder;
using Infrastructure.Service.Initialization.Scopes;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Service.Audio
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