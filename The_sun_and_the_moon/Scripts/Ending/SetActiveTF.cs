using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveTF : MonoBehaviour
{
    //해랑 달 뜨기 시작하는 위치랑 시작 다했을 때의 위치
    public GameObject SUN;
    public Transform SUN_start;
    public Transform SUN_end;
    public GameObject MOON;
    public Transform MOON_start;
    public Transform MOON_end;

    //처음에 떠오를 때
    private float duration = 1.0f; // 상승 시간
    private float elapsedTime = 0f; // 경과 시간

    //위치 바꿀 때
    private float duration_change = 3.0f; // 위치 변경 시간
    private float elapsedTime_change = 0f; // 위치 변경 시간

    //텍스쳐 바꿔진 해와 달
    public GameObject changeSun;
    public GameObject changemoon;

    //띄워줄 이미지
    public GameObject c1;
    public GameObject c1_seal;
    public GameObject c2;

    //밧줄 끊어질 때 이펙트
    public GameObject efx;

    //마지막에 하늘에 띄울 별 이펙트
    public GameObject starefx;


    void Start()
    {
        //테스트용
        //StartCoroutine(SunAndMoonCo());
        //starEx();
    }

    //처음에 뜰 때(산 뒤에서 앞으로 뜸)
    //Slerp를 사용하여 포물선으로 해와 달이 뜨는 함수
    public void SunAndMoonUp()
    {
        print("SunAndMoonUp함수 실행됨");
        StartCoroutine(SunAndMoonCo());
    }
    IEnumerator SunAndMoonCo()
    {
        float t = 0;

        while (t <= 1)
        {
            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            // 비율 계산
            t = Mathf.Clamp01(elapsedTime / duration);

            // 해와 달의 위치 보간
            SUN.transform.position = Vector3.Slerp(SUN_start.position, SUN_end.position, t);
            MOON.transform.position = Vector3.Slerp(MOON_start.position, MOON_end.position, t);

            yield return null;
        }
            print("이동완료");
        // 모든 위치에 도달한 경우 (t가 1에 도달하면)
        if (t >= 1.0f)
        {
            // 필요한 경우, 종료 후 추가 작업 수행
            elapsedTime = 0f; // 경과 시간 초기화
        }
    }

    //해와 달 위치 바꿀 때
    public void changeSunAndMoonPos()
    {
        print("changeSunAndMoonPos함수 실행됨");
        StartCoroutine(SunAndMoonChangePosCo());
    }

    IEnumerator SunAndMoonChangePosCo()
    {
        float t = 0;

        while (t <= 0.9999f)
        {
            // 경과 시간 업데이트
            elapsedTime_change += Time.deltaTime;

            // 비율 계산
            t = Mathf.Clamp01(elapsedTime_change / duration_change);

            // 해와 달의 위치 보간
            SUN.transform.position = Vector3.Slerp(SUN_end.position, MOON_end.position, t);
            SUN.transform.rotation = Quaternion.Slerp(SUN_end.rotation, MOON_end.rotation, t);
            MOON.transform.position = Vector3.Slerp(MOON_end.position, SUN_end.position, t);
            MOON.transform.rotation = Quaternion.Slerp(MOON_end.rotation, SUN_end.rotation, t);
            print(t);
            yield return null;
        }
        print("위치 바꾸기 완료");

        SUN.gameObject.SetActive(false);
        MOON.gameObject.SetActive(false);

        // 모든 위치에 도달한 경우 (t가 1에 도달하면)
        if (t >= 1.0f)
        {
            // 필요한 경우, 종료 후 추가 작업 수행
            elapsedTime_change = 0f; // 경과 시간 초기화
        }
    }

    //위치 바꾼 후 띄울 해와 달
    public void endPos()
    {
        print("endPos함수 실행됨");
        changeSun.gameObject.SetActive(true);
        changemoon.gameObject.SetActive(true);
    }

    //캔버스 켜주기
    public void setActiveImage()
    {
        StartCoroutine(time());
    }

    IEnumerator time()
    {
        c1.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        c1_seal.gameObject.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        c2.gameObject.SetActive(true);
    }

    //밧줄 끊기는 이펙트 활성화시키는 함수
    public void EFX()
    {
        efx.gameObject.SetActive(true);

    }

    public void starEx()
    {
        starefx.gameObject.SetActive(true);
        
    }

}
