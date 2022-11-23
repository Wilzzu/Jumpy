using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFPSForTesting : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
}
