/*
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DefaultPlayMode : EditorWindow
{
    private string scenePath = "Assets/Scenes/MainMenu.unity";

    void OnGUI()
    {
        if (GUILayout.Button("Set start Scene: " + scenePath))
            SetPlayModeStartScene(scenePath);
    }

    void SetPlayModeStartScene(string scenePath)
    {
        SceneAsset startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        if (startScene != null)
            EditorSceneManager.playModeStartScene = startScene;
        else
            Debug.Log("Could not find Scene " + scenePath);
    }

    [MenuItem("Set Default Scene/Open")]
    static void Open()
    {
        GetWindow<DefaultPlayMode>();
    }
}
*/