using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandImage : MonoBehaviour
{
    public MediapipeThirdTimeManager mediapipeThirdTimeManager;
    private void OnEnable()
    {
        if (mediapipeThirdTimeManager == null)
        {
            MediapipeMiniGameTimeManager.Instance.isStart = true;
        }

        mediapipeThirdTimeManager.isStart = true;
    }
}
