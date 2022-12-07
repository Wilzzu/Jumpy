using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    // Variables for tutorial images
    [SerializeField] private GameObject jumpImg;
    [SerializeField] private GameObject jumpImgMobile;
    [SerializeField] private GameObject zoomImg;
    [SerializeField] private GameObject zoomImgMobile;

    // Enable tutorial images depending on device being used
    private void Awake()
    {
        if (GameManager.instance.isMobile)
        {
            jumpImgMobile.SetActive(true);
            zoomImgMobile.SetActive(true);
        }
        else
        {
            jumpImg.SetActive(true);
            zoomImg.SetActive(true);
        }
    }
}
