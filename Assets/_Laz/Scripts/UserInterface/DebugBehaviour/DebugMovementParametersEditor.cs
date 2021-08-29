using UnityEngine;

namespace Laz
{
    public class DebugMovementParametersEditor : MonoBehaviour
    {
        private const float HEADER_HEIGHT_MULTIPLIER = 0.2f;
        private const float FOOTER_HEIGHT_MULTIPLIER = 0.1f;
        private const float HEADER_PADDING_MULTIPLIER = 0.03f;
        private const float FOOTER_PADDING_MULTIPLIER = 0.03f;

        [SerializeField] private Rect _rect;

        private string _fileName = string.Empty;
        private LazMovementPropertyScriptableObject _movementProperty;

        private Vector2 _scrollPosition;

        //GUIStyles
        private GUIStyle _headerGUIStyle;
        private GUIStyle _footerGUIStyle;

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
            CreateGUIStyles();
            CreateGUIParameters();
        }

        private void Initialize()
        {
            ResetContent();
            gameObject.SetActive(true);
        }

        #region GUICalls

        private void OnGUI()
        {
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
            using (new GUILayout.HorizontalScope(GUILayout.Height(_headerHeight)))
            {
                GUILayout.Space(_headerPadding);
                GUILayout.Label("Create Movement Property", GUILayout.ExpandWidth(true));
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
            using (new GUILayout.HorizontalScope(GUILayout.Height(_footerHeight)))
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

        #endregion

        private void ResetContent()
        {
            _movementProperty = ScriptableObject.CreateInstance<LazMovementPropertyScriptableObject>();
            _fileName = string.Empty;
        }

        private bool DoesfileExist(string fileName)
        {
            return false;
        }

        private void SaveMovementProperty()
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                Debug.LogWarning("Please fill in file name before attempting to Save");
                return;
            }

            string fileName = _fileName;
            int fileDuplicateIndex = 1;

            while (DoesfileExist(fileName))
            {
                fileName = $"{fileName}_{fileDuplicateIndex}";
                fileDuplicateIndex++;
            }

            TextWriter.WriteToFile(_movementProperty, fileName);

            Debug.Log($"{fileName}.json Has been Created!");
        }

        private void UpdateMovementProperties()
        {
            
        }

        private void CreateGUIStyles()
        {
            GUIStyle helpBoxStyle = GUI.skin.FindStyle("HelpBox") ?? GUIStyle.none;

            _headerGUIStyle = new GUIStyle(helpBoxStyle)
            {
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState
                {
                    textColor = Color.black
                }
            };
        }

        private void CreateGUIParameters()
        {
            if (_rect == Rect.zero)
            {
                _rect = GetComponent<Rect>();
            }

            _headerHeight = _rect.height * HEADER_HEIGHT_MULTIPLIER;
            _headerPadding = _rect.width * HEADER_PADDING_MULTIPLIER;

            _footerHeight = _rect.height * FOOTER_HEIGHT_MULTIPLIER;
            _footerPadding = _rect.width * FOOTER_PADDING_MULTIPLIER;
        }
    }
}
