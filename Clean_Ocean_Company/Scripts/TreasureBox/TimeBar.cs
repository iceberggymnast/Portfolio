using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviourPun
{
    public float timeLimit = 15.0f;  //15초 동안 존재
    float currentTime; // 현재 남은 시간
    public Image timebarImage_green;  //시간 바 이미지
    public Image timebarImage_red;  //시간 바 이미지
    public GameObject removingGameObject;  //timeLimit초 이후에 없앨 오브젝트

    public bool isTimeisruning = true;

    void Start()
    {
        currentTime = timeLimit;
    }

    public enum RemovingObject
    {
        TreeasureBox,
        o2Charger
    }

    [SerializeField]
    public RemovingObject removingObject;



    void Update()
    {
        //보급상자용
        if (isTimeisruning && removingObject == RemovingObject.TreeasureBox)
        {
            currentTime -= Time.deltaTime; // 매 프레임마다 시간 감소

        }

        //산소충전기용
        else if (isTimeisruning && removingObject == RemovingObject.o2Charger)
        {
            currentTime -= Time.deltaTime; // 매 프레임마다 시간 감소
        }

        if (currentTime > 0)
        {
            float fillAmount = currentTime / timeLimit; // 남은 시간 비율 계산
            timebarImage_green.fillAmount = fillAmount; // 이미지의 fillAmount 설정
            timebarImage_red.fillAmount = fillAmount; // 이미지의 fillAmount 설정

            //시간 40퍼 미만일 때 bar이미지를 빨강색으로 표시
            if (timebarImage_green.fillAmount < 0.4)
            {
                Color color = timebarImage_red.color; // 현재 색상 가져오기
                color.a = 1f;  // 알파 값 설정
                timebarImage_red.color = color; // 색상 다시 할당
            }
        }

        // 시간이 다 되었을 때 오브젝트 제거
        if (currentTime <= 0)
        {
            currentTime = 0;

            // 시간 종료 시 호출
            Destroy(removingGameObject);
        }



    }
}
