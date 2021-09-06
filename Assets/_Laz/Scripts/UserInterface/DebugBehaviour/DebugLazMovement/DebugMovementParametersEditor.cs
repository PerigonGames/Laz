using UnityEngine;

namespace Laz
{
    [RequireComponent(typeof(RectTransform))]
    public class DebugMovementParametersEditor : MonoBehaviour
    {
        private const float HEADER_HEIGHT_MULTIPLIER = 0.1f;
        private const float BODY_HEIGHT_MULTIPLIER = 0.7f;
        private const float FOOTER_HEIGHT_MULTIPLIER = 0.15f;
        private const float HEADER_PADDING_MULTIPLIER = 0.03f;

        private const float MIN_CURVATURE_RATE = 0.005f;
        private const float MAX_CURVATURE_RATE = 0.1f;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private LazMovementPropertyScriptableObject _movementProperty;
        
        private Rect _rect;
        private string _fileName = string.Empty;

        private float _acceleration = 1f;
        private float _deceleration = 1f;
        private float _baseMaxSpeed = 5f;
        private float _curvatureRate = 0.1f;
        private float _lazMaxSpeed = 10f;
        private float _boostTimeLimit = 2f;
        private float _boostSpeed = 20f;

        private bool _isPanelOpen = false;
        
        private Vector2 _scrollPosition;

        //GUIStyles
        private GUIStyle _headerGUIStyle = GUIStyle.none;
        private GUIStyle _labelGUIStyle = GUIStyle.none;
        private GUIStyle _valueGUIStyle = GUIStyle.none;
        private GUIStyle _fileNameStyle = GUIStyle.none;
        private GUIStyle _footerGUIStyle = GUIStyle.none;

        //GUIParameters
        private float _headerHeight;
        private float _headerPadding;
        private float _bodyHeight;
        private float _footerHeight;

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
            GetMovementProperty();
            gameObject.SetActive(true);
        }

        private void GetMovementProperty()
        {
            LazCoordinatorBehaviour coordinator = FindObjectOfType<LazCoordinatorBehaviour>();

            if (coordinator == null)
            {
                Debug.LogError("No LazCoordinatorBehaviour found in scene, unable to process Movement Property");
                CloseWindow();
            }

#if DEVELOPMENT_BUILD || UNITY_EDITOR
            _movementProperty = coordinator.LazMovementPropertyScriptableObject;
#endif

            if (_movementProperty == null)
            {
                Debug.LogError("No LazMovementPropertyScriptableObject found, unable to process");
                CloseWindow();
            }

        }
        
        #region GUICalls

        private void OnGUI()
        {
            CreateGUIStyles();

            using (new GUILayout.AreaScope(_rect))
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    
                    using (new GUILayout.VerticalScope())
                    {
                        DisplayHeader();

                        GUILayout.FlexibleSpace();

                        DisplayElements();

                        GUILayout.FlexibleSpace();

                        DisplayFooter();
                    
                        GUILayout.FlexibleSpace();
                    }
                    
                    GUILayout.FlexibleSpace();
                }
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
            using (new GUILayout.VerticalScope(GUI.skin.box, GUILayout.Height(_bodyHeight)))
            {
                using (var scrollScope = new GUILayout.ScrollViewScope(_scrollPosition, false, false))
                {
                    _scrollPosition = scrollScope.scrollPosition;
                    
                    GUILayout.FlexibleSpace();
                    DisplayFloatElement(ref _movementProperty.acceleration, "Acceleration");
                    GUILayout.FlexibleSpace();
                    DisplayFloatElement(ref _movementProperty.deceleration, "Deceleration");
                    GUILayout.FlexibleSpace();
                    DisplayFloatElement(ref _movementProperty.baseMaxSpeed, "Base Max Speed");
                    GUILayout.FlexibleSpace();
                    DisplayFloatElement(ref _movementProperty.curvatureRate,  "Curvature Rate", MIN_CURVATURE_RATE, MAX_CURVATURE_RATE);
                    GUILayout.FlexibleSpace();
                    DisplayFloatElement(ref _movementProperty.lazMaxSpeed, "Laz Max Speed");
                    GUILayout.FlexibleSpace();
                    DisplayFloatElement(ref _movementProperty.boostTimeLimit, "Boost Time Limit");
                    GUILayout.FlexibleSpace();
                    DisplayFloatElement(ref _movementProperty.boostSpeed, "Boost Speed");
                    GUILayout.FlexibleSpace();
                }
            }
        }

        private void DisplayFloatElement(ref float element, string label, float minValue = 0f, float maxValue = 100f)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label(label, _labelGUIStyle, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
                element = GUILayout.HorizontalSlider(element, minValue, maxValue, GUI.skin.horizontalSlider, GUI.skin.horizontalScrollbarThumb);
                GUILayout.Label(element.ToString("n2"), _valueGUIStyle);
            }
        }

        private void DisplayFileName(float height)
        {
            using (new GUILayout.HorizontalScope(GUILayout.Height(0.3f * height)))
            {
                GUILayout.Label("File Name", _labelGUIStyle, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(false));
                _fileName = GUILayout.TextField(_fileName, _fileNameStyle, GUILayout.ExpandHeight(true));
            }
        }

        private void DisplayFooter()
        {
            using (new GUILayout.VerticalScope(GUILayout.Height(_footerHeight)))
            {
                GUILayout.FlexibleSpace();
                
                DisplayFileName(_footerHeight);
                
                GUILayout.FlexibleSpace();
                
                using (new GUILayout.HorizontalScope(GUI.skin.box, GUILayout.Height(0.6f * _footerHeight)))
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Reset Movement", GUILayout.ExpandHeight(true), GUILayout.Width(0.45f*_rect.width)))
                    {
                        ResetContent();
                    }

                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Save Movement", GUILayout.ExpandHeight(true), GUILayout.Width(0.45f*_rect.width)))
                    {
                        SaveMovementProperty();
                    }
                    
                    GUILayout.FlexibleSpace();
                }
                
                GUILayout.FlexibleSpace();
            }
            
        }
        
        //Can only create GUIStyles on GUICalls
        //So Create GUIStyles on first call
        private void CreateGUIStyles()
        {
            if (_headerGUIStyle != GUIStyle.none && _labelGUIStyle != GUIStyle.none &&
                _valueGUIStyle != GUIStyle.none && _fileNameStyle != GUIStyle.none)
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
                },
                stretchWidth = true,
                fontSize =  20
            };

            _labelGUIStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal =  new GUIStyleState
                {
                    textColor = Color.white
                }
            };

            _valueGUIStyle = new GUIStyle()
            {
                stretchWidth = false,
                stretchHeight = true,
                normal = new GUIStyleState
                {
                    textColor = Color.white
                }
            };

            RectOffset fileNamePadding = GUI.skin.textField.padding;
            fileNamePadding.top = 4;
            
            _fileNameStyle = new GUIStyle(GUI.skin.textField)
            {
                padding = fileNamePadding
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

            _bodyHeight = _rect.height * BODY_HEIGHT_MULTIPLIER;
            
            _footerHeight = _rect.height * FOOTER_HEIGHT_MULTIPLIER;
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

        private void OnRectTransformDimensionsChange()
        {
            CreateGUIParameters();
        }
    }
}
