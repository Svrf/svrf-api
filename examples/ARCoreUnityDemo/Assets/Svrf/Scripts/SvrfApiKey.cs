using UnityEngine;
using System;

namespace Svrf.Unity
{
    [Serializable]
    public sealed class SvrfApiKey : MonoBehaviour
    {
        [SerializeField]
        private string _apiKey = string.Empty;

        public static string Value { get; set; }

        public void Awake()
        {
            Value = _apiKey;
        }
    }
}
