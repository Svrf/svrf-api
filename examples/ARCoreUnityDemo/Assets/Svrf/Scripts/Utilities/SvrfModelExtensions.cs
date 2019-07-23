using Svrf.Models.Media;
using System.Linq;

namespace Svrf.Unity.Utilities
{
    internal static class SvrfModelExtensions
    {
        internal static string GetMainGltfFile(this MediaModel model)
        {
            var gltfFiles = model.Files.Gltf;
            var fileName = gltfFiles.Keys.First(k => k.EndsWith(".gltf"));
            return gltfFiles[fileName];
        }
    }
}
