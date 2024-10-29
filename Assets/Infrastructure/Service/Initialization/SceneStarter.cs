using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Initialization.Scopes;
using Infrastructure.Service.Logger;
using Infrastructure.Service.Scene;
using Infrastructure.Service.SignalBus;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Service.Initialization
{
    public class SceneStarter : MonoBehaviour, IAsyncStartable
    {
        [Inject] private readonly IReadOnlyList<ControlEntity> _controlEntities;
        [Inject] private readonly ISignalBus _signalBus;

        private readonly ILogManager _logger = new LogManager(nameof(SceneStarter));

        private IReadOnlyList<ControlEntity> _orderedControlEntities;

        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            var sceneName = SceneManager.GetActiveScene().name;
            var scopeName = GetScopeName(sceneName);

            _orderedControlEntities = _controlEntities
                .Select(entity => new
                {
                    Entity = entity,
                    Order = GetControlEntityOrder(entity.GetType(), scopeName)
                })
                .Where(x => x.Order != null)
                .OrderBy(x => x.Order.InitOrder)
                .Select(x => x.Entity)
                .ToList();

            if (!_orderedControlEntities.Any())
            {
                _logger.Log($"{sceneName} - no control entities on scene, skip.");
                return;
            }

            _logger.Log($"{sceneName} - start executing phases.");

            _logger.Log($"{sceneName} - load phase execution.");
            await ExecutePhase(entity => entity.LoadPhase());
            _signalBus.Trigger<OnLoadPhaseCompletedSignal>();
            _logger.Log($"{sceneName} - load phase completed.");

            _logger.Log($"{sceneName} - init phase execution.");
            await ExecutePhase(entity => entity.InitPhase());
            _signalBus.Trigger<OnInitPhaseCompletedSignal>();
            _logger.Log($"{sceneName} - init phase completed.");

            _logger.Log($"{sceneName} - post-init phase execution.");
            await ExecutePhase(entity => entity.PostInitPhase());
            _signalBus.Trigger<OnPostInitPhaseCompletedSignal>();
            _logger.Log($"{sceneName} - post-init phase completed.");
        }

        private async UniTask ExecutePhase(Func<ControlEntity, UniTask> phaseFunc)
        {
            foreach (var controlEntity in _orderedControlEntities)
            {
                await phaseFunc(controlEntity);
            }
        }

        private string GetScopeName(string sceneName)
        {
            return sceneName switch
            {
                SceneInfo.BootstrapScene => nameof(BootstrapScope),
                SceneInfo.StartScene => nameof(StartScope),
                SceneInfo.CoreScene => nameof(CoreScope),
                SceneInfo.MetaScene => nameof(MetaScope),
                _ => string.Empty
            };
        }
        
        private ControlEntityOrder GetControlEntityOrder(MemberInfo entityType, string scopeName)
        {
            return entityType
                .GetCustomAttributes<ControlEntityOrder>(false)
                .FirstOrDefault(controlEntityOrder => controlEntityOrder.ScopeName == scopeName);
        }
    }
}