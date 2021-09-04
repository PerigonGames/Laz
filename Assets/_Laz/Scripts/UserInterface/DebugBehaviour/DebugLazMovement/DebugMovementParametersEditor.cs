using System;
using UnityEngine;
using System.Reflection;

namespace Laz
{
    [RequireComponent(typeof(RectTransform))]
    public class DebugMovementParametersEditor : MonoBehaviour
    {
        private const float HEADER_HEIGHT_MULTIPLIER = 0.2f;
        private const float FOOTER_HEIGHT_MULTIPLIER = 0.1f;
        private const float HEADER_PADDING_MULTIPLIER = 0.03f;
        private const float FOOTER_PADDING_MULTIPLIER = 0.03f;

        private const float MIN_CURVATURE_RATE = 0.005f;
        private const float MAX_CURVATURE_RATE = 0.1f;

        //private const Key DEBUG_KEY = Key.Backslash;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private LazMovementPropertyScriptableObject _movementProperty;

        private Rect _rect;
        private string _fileName = string.Empty;
        private Type _movementPropertyType = typeof(LazMovementPropertyScriptableObject);

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
                
                GUILayout.Space(5f);
                // Display Properties - Discuss about using Reflection
                DisplayFloatElement(ref _acceleration, "Acceleration");
                GUILayout.Space(5f);
                DisplayFloatElement(ref _deceleration, "Deceleration");
                GUILayout.Space(5f);
                DisplayFloatElement(ref _baseMaxSpeed, "Base Max Speed");
                GUILayout.Space(5f);
                DisplayFloatElement(ref _curvatureRate, "Curvature Rate", MIN_CURVATURE_RATE, MAX_CURVATURE_RATE);
                GUILayout.Space(5f);
                DisplayFloatElement(ref _lazMaxSpeed, "Laz Max Speed");
                GUILayout.Space(5f);
                DisplayFloatElement(ref _boostTimeLimit, "Boost Time Limit");
                GUILayout.Space(5f);
                DisplayFloatElement(ref _boostSpeed, "Boost Speed");
                
                GUILayout.Space(10f);
                DisplayFileName();
                
                if (GUI.changed)
                {
                    UpdateMovementProperties();
                }
            }
        }

        private void DisplayFloatElement(ref float element, string label, float minValue = 0f, float maxValue = 100f)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label(label, _labelGUIStyle, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
                GUILayout.Space(2f);
                element = GUILayout.HorizontalSlider(element, minValue, maxValue);
                GUILayout.Space(2f);
                GUILayout.Label(element.ToString("n2"), _valueGUIStyle);
            }
        }

        private void DisplayFileName()
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("File Name", _labelGUIStyle, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
                GUILayout.Space(2f);
                _fileName = GUILayout.TextField(_fileName);
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
            };

            RectOffset labelPadding = GUI.skin.label.padding;
            labelPadding.top = -3;
            
            _labelGUIStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                padding = labelPadding,
                normal =  new GUIStyleState
                {
                    textColor = Color.white
                }
            };

            RectOffset valuePadding = GUI.skin.button.padding;
            valuePadding.top = -4;
            valuePadding.bottom = -2;

            _valueGUIStyle = new GUIStyle(GUI.skin.button)
            {
                stretchWidth = false,
                padding = valuePadding
            };

            RectOffset fileNamePadding = GUI.skin.textField.padding;
            fileNamePadding.top = -2;
            
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
            Debug.Log("Wishy Washy");
        }

        private void OnRectTransformDimensionsChange()
        {
            CreateGUIParameters();
        }
    }
}
