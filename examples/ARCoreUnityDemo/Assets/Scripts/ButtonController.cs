using System.Collections;
using Svrf.Models.Media;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace SvrfUnityExample
{
    public class ButtonController : MonoBehaviour
    {
        public MediaModel FaceFilter { get; set; }

        public IEnumerator Start()
        {
            string previewUrl = FaceFilter.Files.Images.Size720x720;
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(previewUrl);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError(request.error);
                yield break;
            }

            Button button = gameObject.GetComponent<Button>();
            EnableButton(button);

            Image image = button.GetComponent<Image>();
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            InsertTexture(image, texture);
        }

        private void EnableButton(Button button)
        {
            ResetAngle(button);
            Destroy(button.GetComponent<Spinning>());
            button.enabled = true;
        }

        private static void ResetAngle(Button button)
        {
            Vector3 iconAngle = button.transform.localEulerAngles;
            iconAngle.z = 0;
            button.transform.localEulerAngles = iconAngle;
        }

        private void InsertTexture(Image image, Texture2D texture)
        {
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteImage = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            ResetOpacity(image);
            image.sprite = spriteImage;
        }

        private static void ResetOpacity(Graphic image)
        {
            Color color = image.color;
            color.a = 1;
            image.color = color;
        }
    }
}
