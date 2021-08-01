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
        [SerializeField] private Key _sceneChangingModifier = Key.LeftCtrl;
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
            _sceneChanger = new DebugSceneChanger();
            _sceneChanger.BuildSceneNamesChanged += HandleBuildSceneChanges;
            _sceneChanger.Initialize();
        }

        public void SetDebugText(string text)
        {
            _genericDebugTextOnWall.text = text;
        }

        private void Awake()
        {
            _instance = this;
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
                _sceneChangeDebugText.gameObject.SetActive(true);
            }

            if (Keyboard.current[_sceneChangingModifier].wasReleasedThisFrame)
            {
                _isModifierBeingPressed = false;
                _sceneChangeDebugText.gameObject.SetActive(false);
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
            
            foreach (string buildSceneName in buildSceneNames)
            {
                sceneNameStringBuilder.AppendLine(buildSceneName);
            }

            _sceneChangeDebugText.text = sceneNameStringBuilder.ToString();
        }

        private void HandleVelocityChange(float velocity)
        {
            _lazoVelocity.text = $"Velocity: {velocity}";
        }

    }
}
