using System.Collections.Generic;
using System.Text;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    public partial class DebugUIBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text _lazoVelocity = null;
        [SerializeField] private TMP_Text _genericDebugTextOnWall = null;

        [Header("Scene Changer")] 
        [SerializeField] private Key _sceneChangingModifier = Key.LeftShift;
        [SerializeField] private TMP_Text _sceneChangeDebugText = null;
        
        private LazMovement _movement = null;
        private DebugSceneChanger _sceneChanger = null;
        private bool _isModifierBeingPressed = false;
        private static DebugUIBehaviour _instance = null;

        public static DebugUIBehaviour Instance => _instance;
        
        public void Initialize(LazMovement movement)
        {
            _movement = movement;
            _movement.OnSpeedChanges += HandleVelocityChange;
        }

        public void SetDebugText(string text)
        {
            _genericDebugTextOnWall.text = text;
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
            
            DontDestroyOnLoad(this);

            _sceneChanger = new DebugSceneChanger();
            _sceneChanger.BuildSceneNamesChanged += HandleBuildSceneChanges;
            _sceneChanger.Initialize();
        }

        private void Update()
        {
            HandleModifierInput();
        }
        
        //Create Callback method to be called before SceneSwitches
        //Because at the point where OnDestroy would be called,
        //The references for _movement and _sceneChanger would be null
        private void OnSceneSwitch()
        {
            _movement.OnSpeedChanges -= HandleVelocityChange;
            _sceneChanger.CleanUp();
            _sceneChanger.BuildSceneNamesChanged -= HandleBuildSceneChanges;
        }

        private void HandleModifierInput()
        {
            if (Keyboard.current[_sceneChangingModifier].wasPressedThisFrame)
            {
                _isModifierBeingPressed = true;
                _sceneChangeDebugText.transform.parent.gameObject.SetActive(true);
            }

            HandleSceneNumberInput();
            
            if (Keyboard.current[_sceneChangingModifier].wasReleasedThisFrame)
            {
                _isModifierBeingPressed = false;
                _sceneChangeDebugText.transform.parent.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// User can switch to scene by pressing the appropriate number
        /// Whilst the modifier is active
        /// At the current moment, assuming that only up to 10 scenes will be in build at a time
        /// </summary>
        private void HandleSceneNumberInput()
        {
            if (!_isModifierBeingPressed)
            {
                return;
            }

            int buildIndex = GetDebugBuildIndex();
            
            if (buildIndex > -1)
            {
                _sceneChanger.ChangeScene(buildIndex, OnSceneSwitch);
            }
        }

        private void HandleBuildSceneChanges()
        {
            List<string> buildSceneNames = _sceneChanger.BuildSceneNames; 
            if (buildSceneNames.IsNullOrEmpty())
            {
                return;
            }
            
            StringBuilder sceneNameStringBuilder = new StringBuilder();
            int buildIndexUI = 1;
            
            foreach (string buildSceneName in buildSceneNames)
            {
                if (string.IsNullOrEmpty(buildSceneName))
                {
                    continue;
                }

                sceneNameStringBuilder.Append(buildSceneName);
                sceneNameStringBuilder.Append(" - ");
                sceneNameStringBuilder.AppendLine($"LShift + {buildIndexUI}");
                buildIndexUI++;
            }

            _sceneChangeDebugText.text = sceneNameStringBuilder.ToString();
        }

        private void HandleVelocityChange(float velocity)
        {
            _lazoVelocity.text = $"Velocity: {velocity}";
        }

    }
}
