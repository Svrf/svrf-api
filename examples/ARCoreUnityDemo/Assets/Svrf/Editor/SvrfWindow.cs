using Svrf.Exceptions;
using Svrf.Unity.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Svrf.Unity.Editor
{
    public class SvrfWindow : EditorWindow
    {
        private static TextureLoader _textureLoader;

        private const int ThumbnailWidth = 128;
        private const int ThumbnailHeight = 96;
        private const int CellWidth = ThumbnailWidth;
        private const int CellSpacing = 5;
        private const int Padding = 5;
        private const int BlockSpacing = 10;

        private Vector2 _scrollPosition;
        private string _searchString = string.Empty;

        public static bool IsFaceFilter = true;

        private Texture _refreshIcon;

        private bool _isApiKeyValid;
        private string _oldApiKeyValue;

        [MenuItem("Window/Svrf")]
        public static void ShowWindow()
        {
            GetWindow<SvrfWindow>("Svrf");
        }

        public async void Awake()
        {
            _refreshIcon = Resources.Load("refresh_icon") as Texture;

            CheckApiKey();
            if (!_isApiKeyValid) return;

            try
            {
                _textureLoader = new TextureLoader(Repaint);
                await _textureLoader.FetchMediaModels();
            }
            catch (ApiKeyNotFoundException)
            {
                _isApiKeyValid = false;
                _textureLoader = null;
            }

            // Textures are lost when exiting play mode, so need to refresh.
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.ExitingPlayMode) Refresh();
            };
        }

        public void OnDestroy()
        {
            SvrfModelEditor.SelectedSvrfModel = null;
        }

        public void OnGUI()
        {
            CheckApiKey();

            if (_textureLoader == null)
            {
                Awake();
            }

            GUILayout.BeginArea(new Rect(Padding, Padding, position.width - 2 * Padding, position.height - 2 * Padding));

            if (Application.isPlaying)
            {
                GUILayout.Space(BlockSpacing);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Warning! Play mode is active.", EditorStyles.wordWrappedLabel);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            DrawSearchArea();

            if (!_isApiKeyValid)
            {
                GUILayout.EndArea();
                return;
            }

            DrawCellGrid();

            GUILayout.EndArea();
        }

        private void CheckApiKey()
        {
            if (_oldApiKeyValue == SvrfApiKey.Value) return;

            _oldApiKeyValue = SvrfApiKey.Value;
            _isApiKeyValid = !string.IsNullOrEmpty(SvrfApiKey.Value);
        }

        private void Refresh()
        {
            _textureLoader.SearchModels(_searchString);
        }

        private void DrawSearchArea()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(Padding);

            DrawWindowLabel();

            if (!_isApiKeyValid)
            {
                GUILayout.Space(Padding);
                GUILayout.EndVertical();
                return;
            }

            IsFaceFilter = GUILayout.Toggle(IsFaceFilter, "Is Face Filter", GUILayout.Width(140));
            
            DrawSearchField();

            GUILayout.Space(Padding);
            GUILayout.EndVertical();
        }

        private void DrawCellGrid()
        {
            if (_textureLoader.ModelIds.Count == 0 && _textureLoader.IsNoResult)
            {
                DrawNoResultMessage();
                return;
            }

            if (_textureLoader.ModelIds.Count == 0)
            {
                GUILayout.Label("Loading...", EditorStyles.boldLabel);
                return;
            }

            GUILayout.BeginHorizontal();
            var assetsPerRow = GetAssetsCountPerRow();
            var assetsThisRow = 0;

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            GUILayout.Space(Padding);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            SvrfPreview clickedModel = null;

            foreach (var modelId in _textureLoader.ModelIds)
            {
                if (assetsThisRow >= assetsPerRow)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.Space(BlockSpacing * 2);
                    GUILayout.BeginHorizontal();
                    assetsThisRow = 0;
                }

                if (assetsThisRow > 0) GUILayout.Space(CellSpacing);

                GUILayout.BeginVertical();

                var preview = PreviewsCache.Get(modelId);
                if (preview == null)
                {
                    GUILayout.Button("Loading", GUILayout.Height(ThumbnailHeight), GUILayout.Width(ThumbnailWidth));
                    GUILayout.Space(CellSpacing);
                    GUILayout.Label(string.Empty, EditorStyles.boldLabel, GUILayout.MaxWidth(CellWidth));
                }
                else
                {
                    var isPreviewButtonClicked = GUILayout.Button(preview.Texture,
                        GUILayout.Width(ThumbnailWidth), GUILayout.Height(ThumbnailHeight));

                    if (isPreviewButtonClicked)
                    {
                        clickedModel = preview;
                    }

                    GUILayout.Space(CellSpacing);
                    GUILayout.Label(preview.Title, EditorStyles.boldLabel, GUILayout.MaxWidth(CellWidth));
                }

                GUILayout.EndVertical();
                assetsThisRow++;
            }

            if (assetsThisRow > 0)
            {
                while (assetsThisRow < assetsPerRow)
                {
                    GUILayout.BeginVertical();
                    GUILayout.Label(string.Empty, GUILayout.Width(CellWidth), GUILayout.Height(ThumbnailHeight));
                    GUILayout.EndVertical();
                    assetsThisRow++;
                }
            }

            GUILayout.EndHorizontal();

            DrawLoadMoreButton();

            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            if (clickedModel != null)
            {
                InsertSelectedModel(clickedModel);
            }
        }

        private void DrawSearchField()
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(Padding);
            _searchString = EditorGUILayout.TextField(_searchString);
            GUILayout.EndVertical();

            var isSearchClicked = GUILayout.Button("Search");

            GUILayout.EndHorizontal();

            if (isSearchClicked)
            {
                Refresh();
            }
        }

        private void DrawWindowLabel()
        {
            GUILayout.BeginHorizontal();

            if (!_isApiKeyValid)
            {
                GUILayout.Label("Please set valid api key in the Svrf Api Key game object", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                var isCreateApiKeyObjectClicked = GUILayout.Button("Create Api Key game object", GUILayout.Height(30));

                if (isCreateApiKeyObjectClicked)
                {
                    SvrfObjectsFactory.CreateSvrfApiKey();
                }
            }
            else
            {
                GUILayout.Label("Discover Svrf face filter and 3D models", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
            }

            var isRefreshClicked = GUILayout.Button(_refreshIcon, GUILayout.Width(30), GUILayout.Height(30));

            GUILayout.EndHorizontal();

            if (isRefreshClicked)
            {
                Refresh();
            }
        }

        private static void DrawNoResultMessage()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("No result", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private static void DrawLoadMoreButton()
        {
            GUILayout.Space(BlockSpacing);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            var isLoadMoreClicked = false;

            if (_textureLoader.IsLoading)
            {
                GUILayout.Label("Loading...");
            }
            else if (!_textureLoader.AreAllModelsLoaded)
            {
                isLoadMoreClicked = GUILayout.Button("Load more");
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(Padding);

            if (isLoadMoreClicked)
            {
                _textureLoader.LoadMoreModels();
            }
        }

        private int GetAssetsCountPerRow()
        {
            var flooredToIntWidth = Mathf.FloorToInt((position.width - 40));

            return Mathf.Clamp(flooredToIntWidth / (CellSpacing + CellWidth), 1, int.MaxValue);
        }

        private static void InsertSelectedModel(SvrfPreview preview)
        {
            if (SvrfModelEditor.SelectedSvrfModel != null)
            {
                var svrfModel = SvrfModelEditor.SelectedSvrfModel;
                svrfModel.SvrfModelId = preview.Id;

                SvrfModelEditor.Preview = preview;

                return;
            }

            var svrfGameObject = SvrfObjectsFactory.CreateSvrfModel(preview.Title);
            var svrfComponent = svrfGameObject.GetComponent<SvrfModel>();
            svrfComponent.SvrfModelId = preview.Id;
        }
    }
}
