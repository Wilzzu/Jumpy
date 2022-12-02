using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isMobile = false;
    [SerializeField] private string[] noInGameUIScenes;
    [SerializeField] private GameObject inGameUI;

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
        // Subscribe to scene loading
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Check what scene loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Don't show ingame UI if user is in the menus
        if (Array.IndexOf(noInGameUIScenes, scene.name) > -1) inGameUI.SetActive(false);
        else inGameUI.SetActive(true);
    }

    // When player has finished the level
    public void LevelFinished()
    {
        Debug.Log("Level finished!");
    }

}
