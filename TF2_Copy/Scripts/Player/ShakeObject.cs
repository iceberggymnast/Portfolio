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

    ////// ������ ����
    ////// ������ �ð� ���� ������ ���� �ȿ��� ������ �������� ��ġ�� �����Ѵ�.
    ////// �ʿ� ��� : ��ü �ð�, ���� Ƚ��, ����
    IEnumerator ShakePosition(float duration, float frequency, Vector3 amplitude)
    {
        float interval = 1.0f / frequency;
        FollowCamera.CameraType currentType = FollowCamera.CameraType.BasicType;

        shaking = true;
        followCamera.cameraType = FollowCamera.CameraType.RunType;


        for (float i = 0; i < duration; i += interval)
        {
            // ���� �Լ��� �̿��� ���� ���
            Vector2 randomPos = new Vector2(1, 0);

            randomPos.x *= amplitude.x;
            randomPos.y *= amplitude.y;

            // originPos�� �������� ������ ��ġ ���� ����ؼ� �� ������ ��ġ�� �����Ѵ�.
            transform.position = cameraController.camList[0].position + new Vector3(randomPos.x, randomPos.y, 0);

            yield return new WaitForSeconds(interval);
        }

        shaking = false;
        followCamera.cameraType = currentType;
        
        
    }



    // �����̼� ����
    // ������ �ð� ���� ������ ���� �ȿ��� ������ �������� ȸ�� ���� �����Ѵ�.
    // �ʿ� ��� : ��ü �ð�, ���� Ƚ��, ����
    IEnumerator ShakeRotation(float duration, float frequency, Vector3 amplitude)
    {
        float interval = 1.0f / frequency;
   
        FollowCamera.CameraType currentType = FollowCamera.CameraType.BasicType;
        
        shaking = true;
        followCamera.cameraType = FollowCamera.CameraType.RunType;

        for (float i = 0; i < duration; i += interval)
        {
            // Perline Noise�� �̿��� ���� ���
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

            // eulerOrigin�� �������� ������ ȸ�� ���� ����ؼ� �� ������ ȸ�� ���� �����Ѵ�.
            transform.eulerAngles = cameraController.camList[0].eulerAngles + new Vector3(randomPos.x, randomPos.y, 0);

            yield return new WaitForSeconds(interval);
        }

        shaking = false;
        followCamera.cameraType = currentType;
        

    }

}
