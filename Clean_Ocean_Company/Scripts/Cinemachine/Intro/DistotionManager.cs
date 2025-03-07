using StylizedWater2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class DistotionManager : MonoBehaviour
{
    public GameObject UnderwaterRenderer;
    float DistortionStrenth;

    void Start()
    {
        // 씬에 SignalReceiver가 없으면 시그널 작동을 멈추게 처리
        if (FindObjectOfType<SignalReceiver>() == null)
        {
            Debug.LogWarning("SignalReceiver가 없으므로 ScreenDarken 시그널은 작동하지 않습니다.");
            this.enabled = false;  // ScreenDarken 스크립트 비활성화
        }
        DistortionStrenth = UnderwaterRenderer.GetComponent<UnderwaterRenderer>().distortionStrength;
    }

    void Update()
    {

    }

    //물고기 말풍선, 잠수정 내부 비출 때 일렁이는 효과 꺼주기
    public void DistortionOff()
    {
        DistortionStrenth = 0f;
        UnderwaterRenderer.GetComponent<UnderwaterRenderer>().distortionStrength = DistortionStrenth;

    }
}
