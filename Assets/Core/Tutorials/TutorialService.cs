using System;
using System.Collections.Generic;
using Core.SaveLoad;
using VContainer;

namespace Core.Tutorials
{
    public class TutorialService : ITutorialService
    {
        private readonly Dictionary<Type, TutorialData> _dataRepository = new();

        [Inject]
        public TutorialService(IEnumerable<TutorialData> injectedDatas)
        {
            foreach (var data in injectedDatas)
            {
                _dataRepository.Add(data.GetType(), data);
            }
        }
        
        public bool IsTutorialCompleted<T>() where T : TutorialData
        {
            var tutorialType = typeof(T);
            var tutorial = _dataRepository[tutorialType];
            return tutorial.IsTutorialCompleted;
        }
    }
}