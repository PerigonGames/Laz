using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Laz
{
    public class DebugSceneChanger
    {
        private const string SCENE_NAME_DELIMITER = "/";
        private const string SCENE_NAME_EXTENSION = ".unity";
        
        private List<string> _buildSceneNames = new List<string>();

        public Action BuildSceneNamesChanged;

        public List<string> BuildSceneNames => _buildSceneNames;

        public void Initialize()
        {
            GetBuildSceneNames();
            
#if UNITY_EDITOR
            UnityEditor.EditorBuildSettings.sceneListChanged += GetBuildSceneNames;
#endif
        }

        public void CleanUp()
        {
#if UNITY_EDITOR
            UnityEditor.EditorBuildSettings.sceneListChanged -= GetBuildSceneNames;
#endif
        }

        public void ChangeScene(int buildIndex, Action onCallBack)
        {
            if (buildIndex < 0 || buildIndex >= _buildSceneNames.Count)
            {
                Debug.LogWarning("Inappropriate Scene Index, unable to load scene");
                return;
            }

            onCallBack?.Invoke();
            SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
        }

        private void GetBuildSceneNames()
        {
            _buildSceneNames = new List<string>();

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                _buildSceneNames.Add(GetAppropriateSceneName(SceneUtility.GetScenePathByBuildIndex(i)));
            }
            
            BuildSceneNamesChanged?.Invoke();
        }

        private string GetAppropriateSceneName(string buildPath)
        {
            if (string.IsNullOrEmpty(buildPath))
            {
                Debug.LogError("Scene Build Name Not Available");
                return string.Empty;
            }

            string sceneNameWithExtension = buildPath.Substring(buildPath.LastIndexOf(SCENE_NAME_DELIMITER) + 1);
            string sceneName = sceneNameWithExtension.Replace(SCENE_NAME_EXTENSION, string.Empty);
            
            return sceneName;
        }
    }
}
