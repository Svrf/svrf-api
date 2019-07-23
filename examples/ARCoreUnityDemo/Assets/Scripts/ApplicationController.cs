using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleARCore;
using Svrf.Models.Http;
using Svrf.Models.Media;
using Svrf.Unity;
using Svrf.Unity.Examples;
using Svrf.Unity.Models;
using UnityEngine;
using UnityEngine.UI;

namespace SvrfUnityExample
{
    public class ApplicationController : MonoBehaviour
    {
        public InputField Input;
        public ScrollViewController ScrollViewController;
        public FaceFilterController FaceFilterController;

        public GameObject SpinnerPrefab;
        public GameObject SpinnerContainer;

        public static int RequestSize { get; } = 10;

        private GameObject _spinner;

        private static SvrfApi _svrfApi;
        private int _pageNum = 0;

        public void Start()
        {
            _svrfApi = new SvrfApi();

            ScrollViewController.OnPreviewClick = OnLoadFaceFilter;
            ScrollViewController.OnReachLoadingPoint = OnAddNextItems;

            if (Application.isEditor)
            {
                DisableARCore();
            }

            SearchFaceFilters();
        }

        public async void SearchFaceFilters()
        {
            _pageNum = 0;

            IEnumerable<MediaModel> faceFilters = await FetchFaceFilters();
            ScrollViewController.Reset();

            AddToScrollView(faceFilters);
        }

        private async void OnLoadFaceFilter(MediaModel faceFilter)
        {
            Destroy(FaceFilterController.FaceFilter);

            _spinner = Instantiate(SpinnerPrefab);
            _spinner.transform.SetParent(SpinnerContainer.transform, false);

            SvrfModelOptions options = new SvrfModelOptions { WithOccluder = !Application.isEditor };
            
            GameObject svrfModel = await SvrfModel.GetSvrfModelAsync(faceFilter, options);

            svrfModel.transform.SetParent(FaceFilterController.transform);
            FaceFilterController.FaceFilter = svrfModel;

            Destroy(_spinner);
        }

        private void DisableARCore()
        {
            GameObject ARCoreDevice = GameObject.Find("ARCore Device");

            GameObject firstPersonCamera = ARCoreDevice.transform.Find("First Person Camera").gameObject;
            firstPersonCamera.GetComponent<ARCoreBackgroundRenderer>().enabled = false;
            firstPersonCamera.GetComponent<Camera>().depth = -1;
            firstPersonCamera.transform.position = new Vector3(0, 1, -10);

            CameraController cameraController = firstPersonCamera.AddComponent<CameraController>();
            cameraController.TargetObject = FaceFilterController.transform;

            FaceFilterController.GetComponent<FaceFilterController>().enabled = false;
        }

        private void AddToScrollView(IEnumerable<MediaModel> faceFilters)
        {
            foreach (MediaModel mask in faceFilters)
            {
                ScrollViewController.AddItem(mask);
            }
        }

        private async Task OnAddNextItems()
        {
            List<MediaModel> faceFilters = (await FetchFaceFilters()).ToList();

            AddToScrollView(faceFilters);
        }

        private async Task<IEnumerable<MediaModel>> FetchFaceFilters()
        {
            MediaRequestParams options = new MediaRequestParams()
            {
                PageNum = _pageNum,
                Size = RequestSize,
                IsFaceFilter = true,
            };

            MultipleMediaResponse mediaResponse = string.IsNullOrEmpty(Input.text)
                ? await _svrfApi.Media.GetTrendingAsync(options)
                : await _svrfApi.Media.SearchAsync(Input.text, options);

            _pageNum = mediaResponse.NextPageNum;

            return mediaResponse.Media;
        }
    }
}
