using UnityEngine;

namespace SvrfUnityExample
{
    public class Spinning : MonoBehaviour
    {
        public RectTransform MainIcon;
        public float TimeStep;
        public float AngleStep;

        private float _startTime;

        void Start()
        {
            _startTime = Time.time;
        }

        void Update()
        {
            if (Time.time - _startTime < TimeStep)
            {
                return;
            }

            Vector3 iconAngle = MainIcon.localEulerAngles;
            iconAngle.z += AngleStep;

            MainIcon.localEulerAngles = iconAngle;

            _startTime = Time.time;
        }
    }
}
