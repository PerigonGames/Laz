using System.Collections.Generic;
using System.Text;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Laz
{
    public class DebugUIBehaviour : MonoBehaviour
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
            _instance = this;
            _sceneChanger = new DebugSceneChanger();
            _sceneChanger.BuildSceneNamesChanged += HandleBuildSceneChanges;
            _sceneChanger.Initialize();
        }

        private void Update()
        {
            HandleModifierInput();
        }

        private void OnDestroy()
        {
            _movement.OnSpeedChanges -= HandleVelocityChange;
            _sceneChanger.BuildSceneNamesChanged -= HandleBuildSceneChanges;
            _sceneChanger.CleanUp();
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

            Debug.Log($"SceneChanger is {(_sceneChanger == null ? "Null" : "Not Null")}");

            int buildIndex = -1;
            
            if (Keyboard.current[Key.Digit1].wasPressedThisFrame)
            {
                buildIndex = 0;
            }
            else if (Keyboard.current[Key.Digit2].wasPressedThisFrame)
            {
                buildIndex = 1;
            }
            else if (Keyboard.current[Key.Digit3].wasPressedThisFrame)
            {
                buildIndex = 2;
            }
            else if (Keyboard.current[Key.Digit4].wasPressedThisFrame)
            {
                buildIndex = 3;
            }
            else if (Keyboard.current[Key.Digit5].wasPressedThisFrame)
            {
                buildIndex = 4;
            }
            else if (Keyboard.current[Key.Digit6].wasPressedThisFrame)
            {
                buildIndex = 5;
            }
            else if (Keyboard.current[Key.Digit7].wasPressedThisFrame)
            {
                buildIndex = 6;
            }
            else if (Keyboard.current[Key.Digit8].wasPressedThisFrame)
            {
                buildIndex = 7;
            }
            else if (Keyboard.current[Key.Digit9].wasPressedThisFrame)
            {
                buildIndex = 8;
            }
            else if (Keyboard.current[Key.Digit0].wasPressedThisFrame)
            {
                buildIndex = 9;
            }

            if (buildIndex > -1)
            {
                _sceneChanger.ChangeScene(buildIndex);
            }
        }

        private void HandleBuildSceneChanges()
        {
            List<string> buildSceneNames = _sceneChanger.BuildSceneNames; 
            if (buildSceneNames.IsNullOrEmpty() || _sceneChangeDebugText == null)
            {
                return;
            }
            
            StringBuilder sceneNameStringBuilder = new StringBuilder();
            int buildIndexUI = 1;
            
            foreach (string buildSceneName in buildSceneNames)
            {
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
