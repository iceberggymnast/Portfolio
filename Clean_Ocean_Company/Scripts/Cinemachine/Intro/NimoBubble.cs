using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class NimoBubble : MonoBehaviour
{
    public GameObject bubble;
    void Start()
    {
        bubble.gameObject.SetActive(false);

        // 씬에 SignalReceiver가 없으면 시그널 작동을 멈추게 처리
        if (FindObjectOfType<SignalReceiver>() == null)
        {
            Debug.LogWarning("SignalReceiver가 없으므로 ScreenDarken 시그널은 작동하지 않습니다.");
            this.enabled = false;  // ScreenDarken 스크립트 비활성화
        }
    }

    void Update()
    {
        
    }

    public void setActiveBubble()
    {
        bubble.gameObject.SetActive(true);
    }
}
