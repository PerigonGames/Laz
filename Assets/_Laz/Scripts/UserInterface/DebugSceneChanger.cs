#if DEVELOPMENT_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Laz
{
    public class DebugSceneChanger : MonoBehaviour
    {
        private const string SCENE_NAME_DELIMITER = "/";
        private const string SCENE_NAME_EXTENSION = ".unity";

        [SerializeField] private Key _sceneChangeModifier = Key.LeftCtrl;
        private List<string> _buildSceneNames = new List<string>();

        private void Awake()
        {
            GetBuildScenes();
            
#if UNITY_EDITOR
            UnityEditor.EditorBuildSettings.sceneListChanged += GetBuildScenes;
#endif

        }

        private void GetBuildScenes()
        {
            _buildSceneNames = new List<string>();

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                _buildSceneNames.Add(GetAppropriateSceneName(SceneUtility.GetScenePathByBuildIndex(i)));
            }
        }

        private string GetAppropriateSceneName(string buildPath)
        {
            if (string.IsNullOrEmpty(buildPath))
            {
                return string.Empty;
            }

            string appropriateName = buildPath.Substring(buildPath.LastIndexOf(SCENE_NAME_DELIMITER) + 1)
                .Replace(SCENE_NAME_EXTENSION, string.Empty);
            
            return appropriateName;
        }

        private void Update()
        {
            if (Keyboard.current[_sceneChangeModifier].wasPressedThisFrame)
            {
                //
            }
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            UnityEditor.EditorBuildSettings.sceneListChanged += GetBuildScenes;
#endif
        }
    }
}
#endif
