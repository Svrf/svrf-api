using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Svrf.Models.Media;
using UnityEngine;
using UnityEngine.UI;

namespace SvrfUnityExample
{
    public class ScrollViewController : MonoBehaviour
    {
        public GameObject PreviewPrefab;

        public float PreviewsGap = 50;
        public float PaddingTop = 50;

        public Action<MediaModel> OnPreviewClick { get; set; }
        public Func<Task> OnReachLoadingPoint { get; set; }

        private float _newPreviewX;
        private float _newPreviewY;

        private Scrollbar _scrollbar;
        private GameObject _content;

        private float _prefabWidth;
        private float _prefabHeight;

        private float _contentWidth;
        private float _contentHeight;

        private float _loadingPoint = 0.5f;
        private bool _isLoading = false;

        private Button _lastSelectedButton;

        private readonly List<GameObject> _items = new List<GameObject>();

        public void Start()
        {
            _scrollbar = gameObject.transform.Find("Scrollbar Horizontal").gameObject.GetComponent<Scrollbar>();
            _scrollbar.onValueChanged.AddListener(OnScroll);

            Transform viewport = gameObject.transform.Find("Viewport");
            _content = viewport.Find("Content").gameObject;

            _prefabWidth = PreviewPrefab.GetComponent<RectTransform>().rect.width;
            _prefabHeight = PreviewPrefab.GetComponent<RectTransform>().rect.height;

            _contentWidth = _prefabWidth * ApplicationController.RequestSize + PreviewsGap * (ApplicationController.RequestSize + 1);
            _contentHeight = _prefabHeight + PaddingTop * 2;

            ResizeContent();

            InitCoordinates();
        }

        public void InitCoordinates()
        {
            _newPreviewX = PreviewsGap + _prefabWidth / 2;
            _newPreviewY = -(PaddingTop + _prefabHeight / 2);
        }

        public void AddItem(MediaModel faceFilter)
        {
            GameObject button = CreateButton(faceFilter);

            _items.Add(button);

            _contentWidth = _prefabWidth * _items.Count + PreviewsGap * (_items.Count + 1);
            ResizeContent();

            _newPreviewX += PreviewsGap + _prefabWidth;
        }

        public async void OnScroll(float value)
        {
            bool shouldLoadNewItems = value >= _loadingPoint;
            if (_isLoading || !shouldLoadNewItems) return;

            _isLoading = true;

            await OnReachLoadingPoint();

            _loadingPoint = (float)_items.Count / (_items.Count + ApplicationController.RequestSize);
            _isLoading = false;
        }

        public void Clear()
        {
            foreach (GameObject obj in _items)
            {
                Destroy(obj);
            }

            _items.Clear();
            _contentWidth = 0;
            ResizeContent();
        }

        public void Reset()
        {
            Clear();
            InitCoordinates();
        }

        private void ResizeContent(float deltaX = 0, float deltaY = 0)
        {
            _contentWidth += deltaX;
            _contentHeight += deltaY;

            _content.GetComponent<RectTransform>().sizeDelta = new Vector2(_contentWidth, _contentHeight);
        }

        private GameObject CreateButton(MediaModel faceFilter)
        {
            GameObject button = Instantiate(PreviewPrefab);

            button.transform.SetParent(_content.transform);
            button.transform.localPosition = new Vector3(_newPreviewX, _newPreviewY);
            button.name = faceFilter.Title;

            PreviewController previewController = button.GetComponent<PreviewController>();
            previewController.FaceFilter = faceFilter;

            Button buttonComponent = button.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() =>
            {
                if (_lastSelectedButton != null)
                {
                    _lastSelectedButton.enabled = true;
                }

                buttonComponent.enabled = false;
                _lastSelectedButton = buttonComponent;

                OnPreviewClick(faceFilter);
            });

            return button;
        }
    }
}
