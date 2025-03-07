using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Tooptipfade : MonoBehaviour
{
    CanvasGroup cg;
    Vector3 range;
    float offset = 8f;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (PlayerInfo.instance != null && PlayerInfo.instance.player != null)
        {
            range = transform.position - PlayerInfo.instance.player.transform.position;

            if (range.magnitude <= offset)
            {
                cg.alpha = offset - range.magnitude;
            }
            else
            {
                cg.alpha = 0;
            }
        }
    }
}
