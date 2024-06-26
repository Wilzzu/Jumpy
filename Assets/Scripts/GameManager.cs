using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject mainMenuUIPc;
    [SerializeField] private GameObject mainMenuUIMobile;
    [SerializeField] private string[] noInGameUIScenes;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject endScreenUI;
    [SerializeField] private GameObject exitConfirmationUI;
    [SerializeField] private TextMeshProUGUI FinalTimeValuePcText;
    [SerializeField] private TextMeshProUGUI FinalTimeValueMobileText;
    [SerializeField] private TextMeshProUGUI FinalJumpsValuePcText;
    [SerializeField] private TextMeshProUGUI FinalJumpsValueMobileText;
    [SerializeField] private TextMeshProUGUI JumpCountPcText;
    [SerializeField] private TextMeshProUGUI JumpCountMobileText;
    [SerializeField] private TextMeshProUGUI TimePcText;
    [SerializeField] private TextMeshProUGUI TimeMobileText;
    [SerializeField] private AudioSource changeSound;
    [SerializeField] private AudioSource finishSound;
    public bool isMobile = false;
    private bool firstTimeLoading = true;

    // Variables for scene handling
    private string currentScene;
    private int currentLevel;

    // Variables used for score
    private int jumpCount = 0;
    private bool timerActive = false;
    private float currentTime;
    private TimeSpan parsedTime;

    private void Awake()
    {
        // Assign GameManager instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        // Enable mobile elements
        if (Application.isMobilePlatform)
        {
            isMobile = true;
            mainMenuUIMobile.SetActive(true);
            inGameUI.transform.GetChild(0).gameObject.SetActive(true);
            endScreenUI.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            mainMenuUIPc.SetActive(true);
            inGameUI.transform.GetChild(1).gameObject.SetActive(true);
            endScreenUI.transform.GetChild(1).gameObject.SetActive(true);
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

    private void Update()
    {
        // Update timer
        if (timerActive)
        {
            currentTime = currentTime + Time.deltaTime;
            parsedTime = TimeSpan.FromSeconds(currentTime);
            TimeMobileText.text = "Time: " + parsedTime.ToString(@"mm\:ss\:fff");
            TimePcText.text = "Time: " + parsedTime.ToString(@"mm\:ss\:fff");
        }
    }

    // Check what scene loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Assign current scene name to a variable
        currentScene = scene.name;

        // Don't show ingame UI if player is in the menus
        if (Array.IndexOf(noInGameUIScenes, scene.name) > -1)
        {
            inGameUI.SetActive(false);
            if (!firstTimeLoading) changeSound.Play();
            else firstTimeLoading = false;
        }
        else inGameUI.SetActive(true);
    }

    public void ChangeScene(string sceneName)
    {
        // Disable end UI and change scene
        endScreenUI.SetActive(false);
        if (sceneName == "next")
        {
            currentLevel = Int16.Parse(currentScene.Substring(currentScene.IndexOf("_") + 1));
            SceneManager.LoadScene("Level_" + (currentLevel + 1));
        }
        else SceneManager.LoadScene(sceneName);
    }

    private void OnPlayerJumped()
    {
        // If player jumps for the first time start the timer
        if (jumpCount == 0)
        {
            timerActive = true;
        }

        // Count player jumps
        jumpCount++;
        JumpCountPcText.text = "Jumps: " + jumpCount;
        JumpCountMobileText.text = "Jumps: " + jumpCount;
    }

    // When player has finished the level
    public void LevelFinished()
    {
        // Stop timer and show final stats
        timerActive = false;
        FinalTimeValuePcText.text = parsedTime.ToString(@"mm\:ss\:fff");
        FinalTimeValueMobileText.text = parsedTime.ToString(@"mm\:ss\:fff");
        FinalJumpsValuePcText.text = jumpCount.ToString();
        FinalJumpsValueMobileText.text = jumpCount.ToString();
        inGameUI.SetActive(false);
        endScreenUI.SetActive(true);
        finishSound.Play();

        // Add stats to PlayerPrefs
        if (PlayerPrefs.HasKey(currentScene + "_jumps") == false || currentTime < PlayerPrefs.GetFloat(currentScene + "_time"))
        {
            // If player gets faster time update highscore 
            // Only better time will update it, not fewer jumps
            PlayerPrefs.SetInt(currentScene + "_jumps", jumpCount);
            PlayerPrefs.SetFloat(currentScene + "_time", currentTime);
        }

        ResetLevelStats();
    }

    // Reset level stats to normal values
    private void ResetLevelStats()
    {
        jumpCount = 0;
        currentTime = 0;
        timerActive = false;
        JumpCountPcText.text = "Jumps: " + jumpCount;
        JumpCountMobileText.text = "Jumps: " + jumpCount;
        TimeMobileText.text = "Time: 00:00:000";
        TimePcText.text = "Time: 00:00:000";
    }

    // Pause level and confirm exit
    public void ExitConfirmation()
    {
        if (exitConfirmationUI.activeSelf)
        {
            exitConfirmationUI.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            exitConfirmationUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // Exit level and unpause
    public void ExitLevel()
    {
        exitConfirmationUI.SetActive(false);
        Time.timeScale = 1;
        ResetLevelStats();
        SceneManager.LoadScene("LevelSelect");
    }
}
