using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Laz
{
    public class MovementPropertyCreator : ScriptableWizard
    {
        private static MovementPropertyCreator _window;
        
        public List<TextAsset> jsonFiles = new List<TextAsset>();
        
        private string _movementPropertyDirectory;
        private LazMovementPropertyScriptableObject _movementProperty;
        
        
        [MenuItem("LazTools/Utility/Create Movement Properties")]
        public static void CreateWizard()
        {
            _window = DisplayWizard<MovementPropertyCreator>("Create Movement Property", "Create");
            _window.minSize = new Vector2(500,250);
            _window.maxSize = _window.minSize;
            _window.ShowPopup();
        }

        private void OnEnable()
        {
            // can't set as a ReadOnly file for ScriptableWizard (ScriptableObject)
            _movementPropertyDirectory = "Assets/_Laz/Scripts/ScriptableObject/Laz/MovementProperties";

            if (!Directory.Exists(_movementPropertyDirectory))
            {
                Debug.LogError($"Directory \"{_movementPropertyDirectory}\" does not exist. Closing Window");
                Close();
            }
        }
        
        protected override bool DrawWizardGUI()
        {
            isValid = jsonFiles.Count > 0;
            return base.DrawWizardGUI();
        }

        private void OnWizardUpdate()
        {
            helpString = "Place Json files that you would like to convert into LazMovementScriptableObjects";
        }

        private void OnWizardCreate()
        {
            foreach (TextAsset textAsset in jsonFiles)
            {
                LazMovementPropertyScriptableObject movementProperty = ScriptableObject.CreateInstance<LazMovementPropertyScriptableObject>();
                JsonUtility.FromJsonOverwrite(textAsset.text, movementProperty);
                
                int fileDuplicateIndex = 1;
                string originalFileName = textAsset.name;
                string fileName = textAsset.name;
                
                while (DoesPathExist(fileName))
                {
                    fileName = $"{originalFileName}_{fileDuplicateIndex}";
                    fileDuplicateIndex++;
                }
                
                AssetDatabase.CreateAsset(movementProperty, $"{_movementPropertyDirectory}/{fileName}.asset");
                AssetDatabase.SaveAssets();
                _movementProperty = movementProperty;
            }
            
            EditorGUIUtility.PingObject(_movementProperty);
        }

        private bool DoesPathExist(string fileName)
        {
            return File.Exists($"{_movementPropertyDirectory}/{fileName}.asset");
        }
    }
}
