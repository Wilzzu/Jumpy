using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private GameObject[] levelBtnsPC;
    [SerializeField] private GameObject[] levelBtnsMobile;
    [SerializeField] private GameObject UIPc;
    [SerializeField] private GameObject UIMobile;

    private void Awake()
    {
        // Enable UI and initialize buttons
        if (GameManager.instance.isMobile)
        {
            UIMobile.SetActive(true);
            ActivateButtons(levelBtnsMobile);
        }
        else
        {
            UIPc.SetActive(true);
            ActivateButtons(levelBtnsPC);
        }
    }

    public void StartLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void ActivateButtons(GameObject[] levelBtns)
    {
        // Access each button
        for (int i = 0; i < levelBtns.Length; i++)
        {
            // Add stats if level is completed
            if (PlayerPrefs.HasKey(levelBtns[i].name + "_jumps"))
            {
                levelBtns[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Completed!";
                TimeSpan parsedTime = TimeSpan.FromSeconds(PlayerPrefs.GetFloat(levelBtns[i].name + "_time"));
                levelBtns[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Jumps: " + PlayerPrefs.GetInt(levelBtns[i].name + "_jumps").ToString() + "\nTime: " + parsedTime.ToString(@"mm\:ss\:fff");
            }

            // Unlock next levels
            if (levelBtns[i].name != "Level_0")
            {
                if (PlayerPrefs.HasKey(levelBtns[i - 1].name + "_jumps"))
                {
                    levelBtns[i].GetComponent<Button>().interactable = true;
                    levelBtns[i].GetComponent<Image>().color = new Color(1, 0.8f, 0.19f);
                    levelBtns[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
                }
                else
                {
                    levelBtns[i].GetComponent<Button>().interactable = false;
                    levelBtns[i].GetComponent<Image>().color = new Color(0.38f, 0.38f, 0.38f);
                    levelBtns[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0.5f);
                }
            }
        }
    }
}
