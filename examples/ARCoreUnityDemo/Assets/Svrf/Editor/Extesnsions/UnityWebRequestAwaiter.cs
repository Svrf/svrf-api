using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace Svrf.Unity.Editor.Extensions
{
    public class UnityWebRequestAwaiter : INotifyCompletion
    {
        private readonly UnityWebRequestAsyncOperation _asyncOperation;
        private Action _continuation;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOperation)
        {
            _asyncOperation = asyncOperation;
            asyncOperation.completed += OnRequestCompleted;
        }

        public bool IsCompleted => _asyncOperation.isDone;

        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;
        }

        private void OnRequestCompleted(AsyncOperation obj)
        {
            _continuation();
        }
    }

    public static class ExtensionMethods
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOperation)
        {
            return new UnityWebRequestAwaiter(asyncOperation);
        }
    }
}
