using System.Threading.Tasks;
using UnityEngine;

namespace Svrf.Unity.Coroutines
{
    public class TaskBasedCoroutine : CustomYieldInstruction
    {
        public override bool keepWaiting => !IsCompleted;

        protected bool IsCompleted;

        internal TaskBasedCoroutine(Task task)
        {
            task.ContinueWith(t => { IsCompleted = t.IsCompleted; });
        }
    }
}
