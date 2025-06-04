

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Core.Initialization.Scripts.Decorators;
using Core.Initialization.Scripts.Scopes;
using Core.Initialization.Scripts.Signals;
using Core.Logger;
using Core.Scene;
using Core.SignalBus;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Core.Initialization.Scripts
{
    public class SceneStarter : UnityEngine.MonoBehaviour, IAsyncStartable
    {
        [Inject] private readonly ISignalBus _signalBus;
        [Inject] private readonly IReadOnlyList<ControlEntity> _controlEntities;
        [Inject] private readonly IReadOnlyList<IControlEntityDecorator> _controlEntityDecorators;

        private ILogManager _logger;
        private List<ControlEntity> _orderedControlEntities;

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
                DecorateEntities();
                await StartExecution(sceneName);
            }
            else
            {
                _logger.Log($"{sceneName} - no control entities on scene, skip execution.");
            }

            _signalBus.Trigger<OnSceneInitCompletedSignal, string>(sceneName);
        }

        private void DecorateEntities()
        {
            for (var index = 0; index < _orderedControlEntities.Count; index++)
            {
                var controlEntity = _orderedControlEntities[index];
                
                foreach (var decorator in _controlEntityDecorators)
                {
                    controlEntity = decorator.Decorate(controlEntity);
                }
                
                _orderedControlEntities[index] = controlEntity;
            }
        }

        private async UniTask StartExecution(string sceneName)
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