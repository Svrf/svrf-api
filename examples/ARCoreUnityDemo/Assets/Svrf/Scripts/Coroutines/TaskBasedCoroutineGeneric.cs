using System.Threading.Tasks;

namespace Svrf.Unity.Coroutines
{
    public class TaskBasedCoroutine<T> : TaskBasedCoroutine
    {
        public T Result;

        internal TaskBasedCoroutine(Task<T> task) : base(task)
        {
            task.ContinueWith(t => { Result = t.Result; });
        }
    }
}
