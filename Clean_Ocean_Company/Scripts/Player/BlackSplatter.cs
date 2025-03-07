using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackSplatter : MonoBehaviour
{
    public Image image;          // UI 이미지
    public float blinkSpeed = 1.0f; // 깜박이는 속도

    void Update()
    {
        //스테미나가 30미만일 때 검은색으로 비네트
        if(PlayerInfo.instance.currentPlayerHP < 30)
        {
            // PingPong을 이용해 0에서 1 사이로 알파 값을 변경
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1);

            // 이미지 색상의 알파값을 변경
            Color color = image.color;
            color = Color.black;
            color.a = alpha;
            image.color = color;
        }
        //산소가 30미만일 때 빨간색으로 비네트
        if(PlayerInfo.instance.oxygen < 30)
        {
            // PingPong을 이용해 0에서 1 사이로 알파 값을 변경
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1);

            // 이미지 색상의 알파값을 변경
            Color color = image.color;
            color = Color.red;
            color.a = alpha;
            image.color = color;
        }
        else
        {
            // 이미지 색상의 알파값을 변경
            Color color = image.color;
            color.a = 0;
            image.color = color;
        }
    }
}