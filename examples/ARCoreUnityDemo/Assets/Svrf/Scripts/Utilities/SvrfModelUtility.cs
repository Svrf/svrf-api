using Svrf.Models.Media;
using System.Reflection;
using System.Threading.Tasks;
using Svrf.Unity.Models;
using UnityEngine;
using UnityGLTF;

// Analytics call is performed in a separate thread and we don't wait it.
// Therefore the warning about not awaiting for a task is disabled.
#pragma warning disable CS4014

namespace Svrf.Unity.Utilities
{
    internal static class SvrfModelUtility
    {
        internal static async Task AddSvrfModel(GameObject gameObject, MediaModel model, SvrfModelOptions options)
        {
            var gltfComponent = gameObject.AddComponent<GLTFComponent>();

            // Do not load the model automatically: let us call .Load() method manually and await it.
            SetGltfComponentField(gltfComponent, "loadOnStart", false);
            gltfComponent.GLTFUri = model.Files.GltfMain;

            SetGltfComponentField(gltfComponent, "shaderOverride", options.ShaderOverride);

            if (!Application.isEditor)
            {
                Task.Run(() => SegmentTracking.TrackModelRequest(model));
            }

            await gltfComponent.Load();

            var gltfRoot = gameObject.transform.GetChild(0);

            var integratedOccluder = FindDescendant(gltfRoot, "Occluder");
            integratedOccluder?.gameObject.SetActive(false); // TODO: Remove it when we remove occluder from all models

            if (options.WithOccluder)
            {
                var occluder = Object.Instantiate(Resources.Load<GameObject>("Occluder"));
                occluder.transform.parent = gltfRoot;
            }

            // GLTF models are right-handed, but the Unity coordinates are left-handed,
            // so rotating the model around Y axis.
            gltfRoot.transform.Rotate(Vector3.up, 180);
        }

        private static void SetGltfComponentField(GLTFComponent component, string name, object value)
        {
            var field = typeof(GLTFComponent).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null)
            {
                field.SetValue(component, value);
            }
        }

        private static Transform FindDescendant(Transform transform, string name)
        {
            foreach (Transform child in transform)
            {
                if (child.name == name) return child;

                var result = FindDescendant(child, name);
                if (result != null) return result;
            }

            return null;
        }
    }
}
