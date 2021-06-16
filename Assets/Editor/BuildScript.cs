using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

// Output the build size or a failure depending on BuildPlayer.

public class BuildScript : MonoBehaviour
{
    [MenuItem("Build/Build Windows")]
    public static void MyBuild()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        // TODO: Update which scenes need to be built
        buildPlayerOptions.scenes = new[] { "Assets/_Laz/Scenes/Gym.unity" };
        buildPlayerOptions.locationPathName = "C:/Users/Developer/OneDrive/_PerigonGames/new_build/windowsBuild.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}
