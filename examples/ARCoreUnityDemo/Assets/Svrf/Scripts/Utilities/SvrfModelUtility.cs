using Svrf.Models.Media;
using System.Reflection;
using System.Threading.Tasks;
using Svrf.Unity.Models;
using UnityEngine;
using UnityGLTF;

namespace Svrf.Unity.Utilities
{
    internal static class SvrfModelUtility
    {
        internal static async Task AddSvrfModel(GameObject gameObject, MediaModel model, SvrfModelOptions options)
        {
            var gltfComponent = gameObject.AddComponent<GLTFComponent>();

            // Do not load the model automatically: let us call .Load() method manually and await it.
            SetGltfComponentField(gltfComponent, "loadOnStart", false);
            gltfComponent.GLTFUri = model.GetMainGltfFile();

            SetGltfComponentField(gltfComponent, "shaderOverride", options.ShaderOverride);

            await gltfComponent.Load();

            var gltfRoot = gameObject.transform.GetChild(0);
            var occluder = FindDescendant(gltfRoot, "Occluder");

            // GLTF models are right-handed, but the Unity coordinates are left-handed,
            // so rotating the model around Y axis.
            gltfRoot.transform.Rotate(Vector3.up, 180);

            if (occluder == null)
            {
                return;
            }

            if (options.WithOccluder)
            {
                var meshRenderer = occluder.transform.Find("Primitive").GetComponent<SkinnedMeshRenderer>();
                // If we need to handle occlusion, apply our custom shader to handle it.
                meshRenderer.sharedMaterials[0].shader = Shader.Find("Svrf/Occluder");
            }
            else
            {
                // If we don't need to handle occlusion, hide the occluder.
                occluder.gameObject.SetActive(false);
            }
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
