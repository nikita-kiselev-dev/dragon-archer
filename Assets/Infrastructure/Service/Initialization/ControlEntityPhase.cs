using System;
using Cysharp.Threading.Tasks;

namespace Infrastructure.Service.Initialization
{
    public class ControlEntityPhase
    {
        public string Name { get; private set; }
        public Func<ControlEntity, UniTask> Function { get; private set; }
        public Action CompletionAction { get; private set; }
        public bool RunInParallel { get; private set; }

        public ControlEntityPhase SetName(string name)
        {
            Name = name;
            return this;
        }

        public ControlEntityPhase SetFunction(Func<ControlEntity, UniTask> function)
        {
            Function = function;
            return this;
        }

        public ControlEntityPhase SetCompletionAction(Action completionAction)
        {
            CompletionAction = completionAction;
            return this;
        }

        public ControlEntityPhase SetParallelMode(bool isParallelEnabled)
        {
            RunInParallel = isParallelEnabled;
            return this;
        }
    }
}