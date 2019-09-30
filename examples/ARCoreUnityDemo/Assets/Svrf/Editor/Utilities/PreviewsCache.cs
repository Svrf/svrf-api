using System.Collections.Generic;

namespace Svrf.Unity.Editor.Utilities
{
    internal static class PreviewsCache
    {
        private static readonly IDictionary<string, SvrfPreview> Previews = new Dictionary<string, SvrfPreview>();

        internal static void Add(SvrfPreview preview)
        {
            Previews.TryGetValue(preview.Id, out var existingPreview);
            if (existingPreview != null)
            {
                Previews.Remove(preview.Id);
            }

            Previews.Add(preview.Id, preview);
        }

        internal static SvrfPreview Get(string id)
        {
            Previews.TryGetValue(id, out var preview);

            // Texture can be destroyed after playing mode, so need to check it as well.
            if (preview != null && preview.Texture == null)
            {
                Previews.Remove(id);
                return null;
            }

            return preview;
        }
    }
}
