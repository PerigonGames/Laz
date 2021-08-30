using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    [RequireComponent(typeof(RectTransform))]
    public class DebugMovementParametersEditor : MonoBehaviour
    {
        private const float HEADER_HEIGHT_MULTIPLIER = 0.2f;
        private const float FOOTER_HEIGHT_MULTIPLIER = 0.1f;
        private const float HEADER_PADDING_MULTIPLIER = 0.03f;
        private const float FOOTER_PADDING_MULTIPLIER = 0.03f;

        //private const Key DEBUG_KEY = Key.Backslash;

        [SerializeField] private RectTransform _rectTransform;

        private Rect _rect;
        private string _fileName = string.Empty;
        
        private LazMovementPropertyScriptableObject _movementProperty;

        private bool _isPanelOpen = false;
        
        private Vector2 _scrollPosition;

        //GUIStyles
        private GUIStyle _headerGUIStyle = GUIStyle.none;
        private GUIStyle _footerGUIStyle = GUIStyle.none;

        //GUIParameters
        private float _headerHeight;
        private float _headerPadding;
        private float _footerHeight;
        private float _footerPadding;
        
        public void OpenWindow()
        {
            Initialize();
        }

        public void CloseWindow()
        {
            gameObject.SetActive(false);
        }
        
        private void Awake()
        {
            CreateGUIParameters();
            gameObject.SetActive(false);
        }

        private void Initialize()
        {
            ResetContent();
            gameObject.SetActive(true);
        }
        
        #region GUICalls

        private void OnGUI()
        {
            CreateGUIStyles();
            
            using (new GUILayout.VerticalScope())
            {
                DisplayHeader();

                GUILayout.FlexibleSpace();

                DisplayElements();

                GUILayout.FlexibleSpace();

                DisplayFooter();
            }
        }

        private void DisplayHeader()
        {
            using (new GUILayout.HorizontalScope(GUI.skin.window, GUILayout.Height(_headerHeight)))
            {
                GUILayout.Space(_headerPadding);
                GUILayout.Label("Create Movement Property", _headerGUIStyle, GUILayout.ExpandWidth(true));
                GUILayout.Space(_headerPadding);
            }
        }

        private void DisplayElements()
        {
            using (var scrollScope = new GUILayout.ScrollViewScope(_scrollPosition, false, false))
            {
                _scrollPosition = scrollScope.scrollPosition;
                
                GUI.changed = false;
                
                // Display Properties - Discuss With Marin about changing private to Public

                if (GUI.changed)
                {
                    UpdateMovementProperties();
                }
            }
        }

        private void DisplayFooter()
        {
            using (new GUILayout.HorizontalScope(GUI.skin.box,GUILayout.Height(_footerHeight)))
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Create New Movement Property"))
                {
                    ResetContent();
                }

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Save Movement Property"))
                {
                    SaveMovementProperty();
                }
            }
        }
        
        //Can only create GUIStyles on GUICalls
        //So Create GUIStyles on first call
        private void CreateGUIStyles()
        {
            if (_headerGUIStyle != GUIStyle.none)
            {
                return;
            }
            
            _headerGUIStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = new GUIStyleState
                {
                    textColor = Color.black
                }
            };
        }

        private void CreateGUIParameters()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            _rect = new Rect(_rectTransform.anchorMin.x * Screen.width, _rectTransform.anchorMin.y * Screen.height,
                _rectTransform.rect.width, _rectTransform.rect.height);

            _headerHeight = _rect.height * HEADER_HEIGHT_MULTIPLIER;
            _headerPadding = _rect.width * HEADER_PADDING_MULTIPLIER;

            _footerHeight = _rect.height * FOOTER_HEIGHT_MULTIPLIER;
            _footerPadding = _rect.width * FOOTER_PADDING_MULTIPLIER;
        }

        #endregion

        private void ResetContent()
        {
            _movementProperty = ScriptableObject.CreateInstance<LazMovementPropertyScriptableObject>();
            _fileName = string.Empty;
        }

        private void SaveMovementProperty()
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                Debug.LogWarning("Please fill in file name before attempting to Save");
                return;
            }

            TextWriter.WriteToFile(_movementProperty, _fileName);
        }

        private void UpdateMovementProperties()
        {
            
        }

        private void OnRectTransformDimensionsChange()
        {
            CreateGUIParameters();
        }
    }
}
