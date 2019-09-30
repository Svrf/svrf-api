using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Segment;
using Segment.Model;
using Svrf.Api;
using Svrf.Models.Media;

namespace Svrf.Unity.Utilities
{
    internal static class SegmentTracking
    {
        private const string LibraryName = "Unity";
        private const string SdkVersion = "1.2.0";
        private const string SegmentWriteKey = "VNcUInbeQ5UX2uM2hcXONRfwQqivs7CT";

        private const string FaceFilterEventName = "Face Filter Node Requested";
        private const string Model3DEventName = "3D Node Requested";

        private static string _appId;

        static SegmentTracking()
        {
            Analytics.Initialize(SegmentWriteKey);
        }

        internal static async Task TrackModelRequest(MediaModel model)
        {
            if (string.IsNullOrEmpty(_appId))
            {
                await GetAppId();
            }

            var properties = new Properties
            {
                {"media_id", model.Id},
                {"sdk_version", SdkVersion},
                {"svrf_library", LibraryName}
            };

            var trackName = model.Metadata.IsFaceFilter ?? false
                ? FaceFilterEventName
                : Model3DEventName;

            Analytics.Client.Track(_appId, trackName, properties);
        }

        private static async Task GetAppId()
        {
            var token = await GetAppToken(SvrfModel.SvrfApi);
            token = FormatToken(token);
            
            var convertedFromBase64 = Convert.FromBase64String(token);
            var base64Decoded = Encoding.ASCII.GetString(convertedFromBase64);
            var deserializedToken = JsonConvert.DeserializeObject<JObject>(base64Decoded);

            _appId = deserializedToken["appId"].ToString();
        }

        private static async Task<string> GetAppToken(SvrfApi api)
        {
            var httpClient = typeof(MediaApi)
                .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)?
                .GetValue(api.Media);

            var authApi = typeof(SvrfClient)
                .GetProperty("Auth", BindingFlags.NonPublic | BindingFlags.Instance)?
                .GetValue(api);

            var authTask = authApi?
                .GetType()
                .GetMethod("AuthenticateAsync", BindingFlags.Public | BindingFlags.Instance)?
                .Invoke(authApi, null);

            if (authTask != null) await (Task)authTask;

            var tokenService = httpClient?
                .GetType()
                .GetField("_tokenService", BindingFlags.NonPublic | BindingFlags.Instance)?
                .GetValue(httpClient);

            var token = tokenService?
                .GetType()
                .GetMethod("GetAppToken", BindingFlags.Public | BindingFlags.Instance)?
                .Invoke(tokenService, null)
                .ToString();

            return token;
        }

        private static string FormatToken(string token)
        {
            const int divider = 4;
            const char delimiter = '.';
            const char paddingCharacter = '=';

            // Extracting the middle part of JWT with application info.
            var startIndex = token.IndexOf(delimiter) + 1;
            var endIndex = token.LastIndexOf(delimiter);
            var tokenSubString = token.Substring(startIndex, (endIndex - startIndex));

            // Base64 string should be divisible by the divider.
            var remainder = tokenSubString.Length % divider;
            if (remainder == 0)
            {
                return tokenSubString;
            }

            var padding = new string(paddingCharacter, divider - remainder);
            return tokenSubString + padding;
        }
    }
}
