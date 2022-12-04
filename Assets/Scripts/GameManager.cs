using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isMobile = false;
    [SerializeField] private string[] noInGameUIScenes;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject endScreenUI;
    [SerializeField] private TextMeshProUGUI TimeValueText;
    [SerializeField] private TextMeshProUGUI JumpsValueText;
    [SerializeField] private TextMeshProUGUI JumpCountMobileText;
    [SerializeField] private TextMeshProUGUI JumpCountPcText;
    private string currentScene;
    private int jumpCount = 0;

    private void Awake()
    {
        // Assign GameManager instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Check if mobile
        if (Application.isMobilePlatform)
        {
            isMobile = true;
            // Disable PC UI gameObject
        }
        else
        {
            // Disable Mobile UI gameObject
        }
    }

    private void OnEnable()
    {
        // Subscribe to events
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayerScript.playerJumped += OnPlayerJumped;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerScript.playerJumped -= OnPlayerJumped;
    }

    // Check what scene loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Assign current scene name to a variable
        currentScene = scene.name;

        // Don't show ingame UI if user is in the menus
        if (Array.IndexOf(noInGameUIScenes, scene.name) > -1) inGameUI.SetActive(false);
        else inGameUI.SetActive(true);
    }

    public void ChangeScene(string sceneName)
    {
        // Disable end scene and change scene
        endScreenUI.SetActive(false);
        if (sceneName == "next")
        {
            int currentLevel = Int16.Parse(currentScene.Substring(currentScene.IndexOf("_") + 1));
            SceneManager.LoadScene("Level_" + (currentLevel + 1));
        };
        SceneManager.LoadScene(sceneName);
    }

    private void OnPlayerJumped()
    {
        jumpCount++;
        JumpCountPcText.text = "Jumps: " + jumpCount;
        JumpCountMobileText.text = "Jumps: " + jumpCount;
    }

    // When player has finished the level
    public void LevelFinished()
    {
        TimeValueText.text = "00:30:493";
        JumpsValueText.text = jumpCount.ToString();
        endScreenUI.SetActive(true);

        // Reset level stats
        jumpCount = 0;
        JumpCountPcText.text = "Jumps: " + jumpCount;
        JumpCountMobileText.text = "Jumps: " + jumpCount;
    }

}
