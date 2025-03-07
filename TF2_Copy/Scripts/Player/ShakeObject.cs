using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    public float positionDuration;
    public float positionFrequency;
    public Vector3 positionAmplitude;

    public float rotationDuration;
    public float rotationFrequency;
    public Vector3 rotationAmplitude;

    public bool shaking = false;
    Vector3 originPos;
    Quaternion originRot;

    float px = 0;
    float py = 0;

    public Coroutine cameraShake;

    PlayerMove playerMove;
    PlayerFire playerFire;
    FollowCamera followCamera;

    CameraController cameraController;

   
    public Vector3 recoilKickBack;
    public float recoilAmount;


    void Start()
    {
        
        GameObject player = GameObject.Find("Player");
        if(player != null)
        {
            playerMove = player.GetComponent<PlayerMove>();
            playerFire = player.GetComponent<PlayerFire>();
        }
        followCamera = Camera.main.gameObject.GetComponentInParent<FollowCamera>();
        cameraController = player.GetComponent<CameraController>();


    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !shaking)
        {
            cameraShake = StartCoroutine(ShakePosition(positionDuration, positionFrequency, positionAmplitude));
        }

        if (playerMove.isRun && !shaking)
        {
            cameraShake = StartCoroutine(ShakeRotation(rotationDuration, rotationFrequency, rotationAmplitude));

        }

    }

    ////// 포지션 흔들기
    ////// 지정된 시간 동안 일정한 범위 안에서 일정한 간격으로 위치를 변경한다.
    ////// 필요 요소 : 전체 시간, 진동 횟수, 진폭
    IEnumerator ShakePosition(float duration, float frequency, Vector3 amplitude)
    {
        float interval = 1.0f / frequency;
        FollowCamera.CameraType currentType = FollowCamera.CameraType.BasicType;

        shaking = true;
        followCamera.cameraType = FollowCamera.CameraType.RunType;


        for (float i = 0; i < duration; i += interval)
        {
            // 랜덤 함수를 이용한 랜덤 방식
            Vector2 randomPos = new Vector2(1, 0);

            randomPos.x *= amplitude.x;
            randomPos.y *= amplitude.y;

            // originPos를 기준으로 랜덤한 위치 값을 계산해서 그 쪽으로 위치를 변경한다.
            transform.position = cameraController.camList[0].position + new Vector3(randomPos.x, randomPos.y, 0);

            yield return new WaitForSeconds(interval);
        }

        shaking = false;
        followCamera.cameraType = currentType;
        
        
    }



    // 로테이션 흔들기
    // 지정된 시간 동안 일정한 범위 안에서 일정한 간격으로 회전 값을 변경한다.
    // 필요 요소 : 전체 시간, 진동 횟수, 진폭
    IEnumerator ShakeRotation(float duration, float frequency, Vector3 amplitude)
    {
        float interval = 1.0f / frequency;
   
        FollowCamera.CameraType currentType = FollowCamera.CameraType.BasicType;
        
        shaking = true;
        followCamera.cameraType = FollowCamera.CameraType.RunType;

        for (float i = 0; i < duration; i += interval)
        {
            // Perline Noise를 이용한 랜덤 방식
            px += 0.1f;
            py += 0.1f;
            if (px >= 1.0f)
            {
                px = 0;
                
                if (py >= 1.0f)
                {
                    py = 0;
                }
            }

            Vector2 randomPos = new Vector2(Mathf.PerlinNoise(px, 0) - 0.5f, Mathf.PerlinNoise(0, py) - 0.5f);
            randomPos.x *= amplitude.x;
            randomPos.y *= amplitude.y;

            // eulerOrigin를 기준으로 랜덤한 회전 값을 계산해서 그 쪽으로 회전 값을 변경한다.
            transform.eulerAngles = cameraController.camList[0].eulerAngles + new Vector3(randomPos.x, randomPos.y, 0);

            yield return new WaitForSeconds(interval);
        }

        shaking = false;
        followCamera.cameraType = currentType;
        

    }

}
