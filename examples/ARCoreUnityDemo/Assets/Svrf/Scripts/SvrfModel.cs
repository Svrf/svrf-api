using Svrf.Models.Media;
using System.Threading.Tasks;
using Svrf.Unity.Models;
using Svrf.Unity.Utilities;
using UnityEngine;

namespace Svrf.Unity
{
    public class SvrfModel : MonoBehaviour
    {
        public string SvrfModelId;

        public bool WithOccluder = DefaultOptions.WithOccluder;
        public Shader ShaderOverride = DefaultOptions.ShaderOverride;

        private static SvrfApi _svrfApi;
        internal static SvrfApi SvrfApi => _svrfApi ?? (_svrfApi = new SvrfApi());

        private static readonly SvrfModelOptions DefaultOptions = new SvrfModelOptions
        {
            ShaderOverride = null,
            WithOccluder = true,
        };

        public bool IsLoading { get; set; } = true;

        public bool IsChanged { get; set; }

        public async void Start()
        {
            var model = (await SvrfApi.Media.GetByIdAsync(SvrfModelId)).Media;
            var options = new SvrfModelOptions
            {
                ShaderOverride = ShaderOverride,
                WithOccluder = WithOccluder
            };

            await SvrfModelUtility.AddSvrfModel(gameObject, model, options);

            IsLoading = false;
        }

        public void OnValidate()
        {
            IsChanged = true;
        }

        public static async Task<GameObject> GetSvrfModelAsync(MediaModel model, SvrfModelOptions options = null, GameObject gameObject = null)
        {
            // It's impossible to use null coalescing operator with Unity objects.
            gameObject = gameObject == null ? new GameObject("Svrf Model") : gameObject;
            options = options ?? DefaultOptions;

            await SvrfModelUtility.AddSvrfModel(gameObject, model, options);

            return gameObject;
        }
    }
}
