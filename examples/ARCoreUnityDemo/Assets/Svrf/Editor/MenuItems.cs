using UnityEditor;
using UnityEngine;

namespace Svrf.Unity.Editor
{
    /// <summary>
    /// Enables creation of our custom game objects in the right-click context menu.
    /// </summary>
    public class MenuItems : MonoBehaviour
    {
        [MenuItem("GameObject/Svrf/3D Model", false, 10)]
        public static void CreateSvrfModel(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Svrf Model");
            gameObject.AddComponent<SvrfModel>();

            HandleObjectCreating(gameObject, menuCommand);
        }

        [MenuItem("GameObject/Svrf/Api Key", false, 10)]
        public static void CreateSvrfApiKey(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Svrf Api Key");
            gameObject.AddComponent<SvrfApiKey>();

            HandleObjectCreating(gameObject, menuCommand);
        }

        private static void HandleObjectCreating(GameObject gameObject, MenuCommand menuCommand)
        {
            GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
            Selection.activeObject = gameObject;
        }
    }
}
