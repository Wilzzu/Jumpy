using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isMobile = false;
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
        if (Application.isMobilePlatform) isMobile = true;
    }

    public void LevelFinished()
    {
        Debug.Log("Level finished!");
    }

}
