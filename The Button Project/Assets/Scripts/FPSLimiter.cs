using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    public static FPSLimiter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;
    }
}
