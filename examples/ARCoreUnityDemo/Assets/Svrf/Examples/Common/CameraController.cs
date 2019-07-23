using UnityEngine;

namespace Svrf.Unity.Examples
{
    public class CameraController : MonoBehaviour
    {
        public Transform TargetObject;
        public Vector3 Offset;
        public float Sensitivity = 10f;
        public float OrdinateLimit = 80f;
        public float ZoomDelta = 0.25f;
        public float ZoomMax = 10f;
        public float ZoomMin = 0.5f;

        private float _x;
        private float _y;

        void Start()
        {
            OrdinateLimit = Mathf.Min(Mathf.Abs(OrdinateLimit), 90);
            Offset = new Vector3(Offset.x, Offset.y, -Mathf.Abs(ZoomMin) / 2);
            transform.position = TargetObject.position + Offset;
        }

        void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Offset.z += ZoomDelta;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                Offset.z -= ZoomDelta;
            }

            Offset.z = Mathf.Clamp(Offset.z, -Mathf.Abs(ZoomMax), -Mathf.Abs(ZoomMin));

            if (Input.GetMouseButton(0))
            {
                _x = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Sensitivity;
                _y += Input.GetAxis("Mouse Y") * Sensitivity;
                _y = Mathf.Clamp(_y, -OrdinateLimit, OrdinateLimit);

                transform.localEulerAngles = new Vector3(-_y, _x, 0);
            }

            transform.position = transform.localRotation * Offset + TargetObject.position;
        }
    }
}
