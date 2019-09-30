using System.Threading.Tasks;

namespace Svrf.Unity.Coroutines
{
    public static class TaskExtensions
    {
        public static TaskBasedCoroutine<T> AsCoroutine<T>(this Task<T> task)
        {
            return new TaskBasedCoroutine<T>(task);
        }
    }
}
