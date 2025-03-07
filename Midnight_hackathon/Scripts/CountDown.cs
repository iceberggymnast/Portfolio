using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    private Text counterText;

    private float timeCounter = 0f;
    public bool isTimeUp = false;
    
    //종료 신호를 위하나 델리게이트와 이벤트
    public delegate void TimeUpEventHandler();

    public static event TimeUpEventHandler OnTimeUp;

    private void Start()
    {
       //제한시간 설정 값
        counterText = GetComponent<Text>();
        timeCounter = 60f;
    }

    private void Update() 
    {
        //제한시간 -1초 씩 줄어드는 설정
        if (timeCounter > 0)
        {
        timeCounter -= 1 * Time.deltaTime;
        timeCounter = Mathf.Max(timeCounter, 0);
        }

        if (timeCounter == 0 && !isTimeUp)
        {
            TimeUp();
        }

        counterText.text = timeCounter.ToString("0");
    }

    private void TimeUp()
    {
        isTimeUp = true;
        Debug.Log("시간이 종료되었습니다!");
        
        //종료이벤트를 호출
        if (OnTimeUp != null)
        {
            OnTimeUp.Invoke();
        }
    }
}
