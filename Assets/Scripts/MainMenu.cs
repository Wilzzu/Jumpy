using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private int levelCount = 8;

    private void Awake()
    {
        int levelsCompleted = 0;
        for (int i = 0; i < levelCount; i++)
        {
            if (PlayerPrefs.HasKey("Level_" + i + "_jumps"))
            {
                levelsCompleted++;
            }
        }
        highScoreText.text = "Levels completed: " + levelsCompleted + "/" + levelCount;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
