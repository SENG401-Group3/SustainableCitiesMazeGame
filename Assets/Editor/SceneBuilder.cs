using UnityEngine;
using UnityEditor;

public class SceneBuilder : MonoBehaviour
{
    [MenuItem("Tools/Add All Scenes to Build")]
    static void AddAllScenesToBuild()
    {
        // List of all scene paths
        string[] scenePaths = {
            "Assets/Scenes/CitySelection.unity",
            "Assets/Scenes/LoginMenu/UI.unity",
            "Assets/Scenes/Maze/MazeScene.unity",
            "Assets/Scenes/QuestionScene.unity",
            "Assets/Scenes/LeaderboardScene.unity"
        };

        // Create new scene list
        var scenes = new EditorBuildSettingsScene[scenePaths.Length];

        for (int i = 0; i < scenePaths.Length; i++)
        {
            scenes[i] = new EditorBuildSettingsScene(scenePaths[i], true);
            Debug.Log($"Added to build: {scenePaths[i]}");
        }

        // Apply to build settings
        EditorBuildSettings.scenes = scenes;

        Debug.Log("All scenes added to Build Settings!");
        EditorUtility.DisplayDialog("Success", "All scenes added to Build Settings!", "OK");
    }
}