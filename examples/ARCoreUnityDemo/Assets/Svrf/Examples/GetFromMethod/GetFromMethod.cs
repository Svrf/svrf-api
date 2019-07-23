using System.Linq;
using Svrf.Models.Http;
using Svrf.Models.Media;
using UnityEngine;

namespace Svrf.Unity.Examples
{
    public class GetFromMethod : MonoBehaviour
    {
        async void Start()
        {
            SvrfApi api = new SvrfApi();

            MediaRequestParams options = new MediaRequestParams {IsFaceFilter = true};
            MultipleMediaResponse trendingResponse = await api.Media.GetTrendingAsync(options);
            MediaModel model = trendingResponse.Media.First();

            await SvrfModel.GetSvrfModelAsync(model);

            Destroy(GameObject.Find("Loading"));
        }
    }
}
