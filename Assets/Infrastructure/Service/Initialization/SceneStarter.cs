using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Initialization.Scopes;
using Infrastructure.Service.Initialization.Signals;
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
        [Inject] private readonly ISignalBus _signalBus;
        [Inject] private readonly IReadOnlyList<ControlEntity> _controlEntities;

        private ILogManager _logger;
        private IReadOnlyList<ControlEntity> _orderedControlEntities;

        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            _logger = new LogManager(nameof(SceneStarter));
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

            if (_orderedControlEntities.Any())
            {
                var phases = CreatePhases();
                _logger.Log($"{sceneName} - start executing phases.");
            
                foreach (var phase in phases)
                {
                    _logger.Log($"{sceneName} - {phase.Name} phase execution.");
                    await ExecutePhase(phase);
                    phase.CompletionAction?.Invoke();
                    _logger.Log($"{sceneName} - {phase.Name} phase completed.");
                }
            }
            else
            {
                _logger.Log($"{sceneName} - no control entities on scene, skip execution.");
            }

            _signalBus.Trigger<OnSceneInitCompletedSignal>();
        }

        private List<ControlEntityPhase> CreatePhases()
        {
            var phases = new List<ControlEntityPhase>
            {
                new ControlEntityPhase()
                    .SetName(ControlEntityConstants.LoadPhaseName)
                    .SetFunction(entity => entity.LoadPhase())
                    .SetCompletionAction(_signalBus.Trigger<OnLoadPhaseCompletedSignal>)
                    .SetParallelMode(true),
                
                new ControlEntityPhase()
                    .SetName(ControlEntityConstants.InitPhaseName)
                    .SetFunction(entity => entity.InitPhase())
                    .SetCompletionAction(_signalBus.Trigger<OnInitPhaseCompletedSignal>),
                
                new ControlEntityPhase()
                    .SetName(ControlEntityConstants.PostInitPhaseName)
                    .SetFunction(entity => entity.PostInitPhase())
                    .SetCompletionAction(_signalBus.Trigger<OnPostInitPhaseCompletedSignal>)
            };

            return phases;
        }

        private async UniTask ExecutePhase(ControlEntityPhase phase)
        {
            if (phase.RunInParallel)
            {
                var tasks = _orderedControlEntities.Select(entity => phase.Function(entity));
                await UniTask.WhenAll(tasks);
            }
            else
            {
                foreach (var controlEntity in _orderedControlEntities)
                {
                    await phase.Function(controlEntity);
                }
            }
        }

        private string GetScopeName(string sceneName)
        {
            return sceneName switch
            {
                SceneConstants.BootstrapScene => nameof(BootstrapScope),
                SceneConstants.StartScene => nameof(StartScope),
                SceneConstants.CoreScene => nameof(CoreScope),
                SceneConstants.MetaScene => nameof(MetaScope),
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