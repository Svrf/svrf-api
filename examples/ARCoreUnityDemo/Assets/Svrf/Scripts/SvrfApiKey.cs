using UnityEngine;
using System;

namespace Svrf.Unity
{
    [ExecuteAlways]
    [Serializable]
    public sealed class SvrfApiKey : MonoBehaviour
    {
        [SerializeField]
        private string _apiKey = string.Empty;

        public static string Value { get; set; }

        public void Awake()
        {
            UpdateValue();
        }
        public void OnEnable()
        {
            UpdateValue();
        }

        public void OnValidate()
        {
            UpdateValue();
        }

        private void UpdateValue()
        {
            Value = _apiKey;
        }
    }
}
