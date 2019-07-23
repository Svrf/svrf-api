using System.Collections.Generic;
using System.Linq;
using GoogleARCore;
using UnityEngine;

namespace SvrfUnityExample
{
    public class FaceFilterController : MonoBehaviour
    {
        public GameObject FaceFilter { get; set; }

        private AugmentedFace _augmentedFace;

        public void Update()
        {
            if (Application.isEditor) return;

            if (_augmentedFace == null)
            {
                List<AugmentedFace> tempList = new List<AugmentedFace>();
                Session.GetTrackables(tempList);
                _augmentedFace = tempList.FirstOrDefault();
            }
        
            UpdateFace();
        }

        private void UpdateFace()
        {
            if (_augmentedFace == null) return;
        
            bool isTracking = _augmentedFace.TrackingState == TrackingState.Tracking;

            if (isTracking)
            {
                FaceFilter.SetActive(true);
                FaceFilter.transform.position = _augmentedFace.CenterPose.position;
                FaceFilter.transform.rotation = _augmentedFace.CenterPose.rotation;
            }
            else
            {
                FaceFilter.SetActive(false);
            }
        }
    }
}
