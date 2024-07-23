using System;
using System.Collections.Generic;
using Infrastructure.Service.SaveLoad;
using VContainer;

namespace Infrastructure.Game.Tutorials
{
    public class TutorialService : ITutorialService
    {
        [Inject] private IReadOnlyList<TutorialData> _injectedDatas;

        private readonly Dictionary<Type, TutorialData> _dataRepository = new();

        public bool IsTutorialCompleted<T>() where T : TutorialData
        {
            var tutorialType = typeof(T);
            var tutorial = _dataRepository[tutorialType];
            return tutorial.IsTutorialCompleted;
        }
        
        [Inject]
        private void Init()
        {
            foreach (var data in _injectedDatas)
            {
                _dataRepository.Add(data.GetType(), data);
            }
        }
    }
}