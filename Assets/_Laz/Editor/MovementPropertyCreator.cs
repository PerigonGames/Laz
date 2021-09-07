using System.IO;
using UnityEditor;
using UnityEngine;

namespace Laz
{
    public class MovementPropertyCreator : ScriptableWizard
    {
        private static MovementPropertyCreator _window;
        
        private string _movementPropertyDirectory;
        private TextAsset _movementPropertyJson = null;
        
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
                Debug.LogError($"Directory \" {_movementPropertyDirectory} \" does not exist. Closing Window");
                Close();
            }
            
        }
        
        protected override bool DrawWizardGUI()
        {
            _movementPropertyJson =
                (TextAsset) EditorGUILayout.ObjectField("Json File", _movementPropertyJson, typeof(TextAsset), false);
            
            isValid = _movementPropertyJson != null;
            
            return base.DrawWizardGUI();
        }


        private void OnWizardUpdate()
        {
            helpString = "Place Json file that you would like to convert into LazMovementScriptableObjects";
        }

        private void OnWizardCreate()
        {
            LazMovementPropertyScriptableObject movementProperty = ScriptableObject.CreateInstance<LazMovementPropertyScriptableObject>();
            JsonUtility.FromJsonOverwrite(_movementPropertyJson.text, movementProperty);

            int fileDuplicateIndex = 1;
            string originalFileName = _movementPropertyJson.name;
            string fileName = _movementPropertyJson.name;

            while (DoesPathExist(fileName))
            {
                fileName = $"{originalFileName}_{fileDuplicateIndex}";
                fileDuplicateIndex++;
            }
            
            AssetDatabase.CreateAsset(movementProperty, $"{_movementPropertyDirectory}/{fileName}.asset");
            AssetDatabase.SaveAssets();
            EditorGUIUtility.PingObject(movementProperty);
        }

        private bool DoesPathExist(string fileName)
        {
            return File.Exists($"{_movementPropertyDirectory}/{fileName}.asset");
        }
    }
}
