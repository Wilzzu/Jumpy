using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI highScoreTextMobile;
    [SerializeField] private AudioSource selectSound;
    [SerializeField] private int levelCount = 8;

    private void Awake()
    {
        // Calculate how many levels are completed and show them on highscore
        int levelsCompleted = 0;
        for (int i = 0; i < levelCount; i++)
        {
            if (PlayerPrefs.HasKey("Level_" + i + "_jumps"))
            {
                levelsCompleted++;
            }
        }
        highScoreText.text = "Levels completed: " + levelsCompleted + "/" + levelCount;
        highScoreTextMobile.text = "Levels completed: " + levelsCompleted + "/" + levelCount;
    }

    // Delete PlayerPrefs data
    public void DeleteScores()
    {
        PlayerPrefs.DeleteAll();
        selectSound.Play();
        highScoreText.text = "Levels completed: 0/" + levelCount;
        highScoreTextMobile.text = "Levels completed: 0/" + levelCount;
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
