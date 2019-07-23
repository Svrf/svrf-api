using System.Linq;
using Svrf.Models.Http;
using Svrf.Models.Media;
using UnityEngine;

namespace Svrf.Unity.Examples
{
    public class SetApiKeyInCode : MonoBehaviour
    {
        async void Start()
        {
            SvrfApiKey.Value = "your key";

            SvrfApi api = new SvrfApi();

            MediaRequestParams options = new MediaRequestParams {IsFaceFilter = true};
            MultipleMediaResponse trendingResponse = await api.Media.GetTrendingAsync(options);
            MediaModel model = trendingResponse.Media.First();

            GameObject svrfModel = await SvrfModel.GetSvrfModelAsync(model);

            Destroy(GameObject.Find("Loading"));
        }
    }
}
