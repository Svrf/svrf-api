using UnityEditor;
using UnityEngine;

namespace Svrf.Unity.Editor.Utilities
{
    internal static class SvrfObjectsFactory
    {
        internal static GameObject CreateSvrfModel(string name = null)
        {
            name = name ?? "Svrf Model";

            var gameObject = new GameObject(name);
            gameObject.AddComponent<SvrfModel>();

            HandleObjectCreating(gameObject);

            return gameObject;
        }

        internal static GameObject CreateSvrfApiKey()
        {
            var gameObject = new GameObject("Svrf Api Key");
            gameObject.AddComponent<SvrfApiKey>();

            HandleObjectCreating(gameObject);

            return gameObject;
        }

        internal static void HandleObjectCreating(GameObject gameObject)
        {
            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
            Selection.activeObject = gameObject;
        }
    }
}
