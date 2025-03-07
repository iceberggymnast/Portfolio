using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 플레이어를 버튼에 따라 움직이게 하고 싶다.
    public float moveSpeed = 10.0f; // 움직이는 속도 조절
    public float rollSpeed = 0.001f; // 구르는 속도 조절
    public float sitMoveSpeedRatio = 0.8f; // 거
    public float runMoveSpeedRatio = 1.2f; // 뛸때 속도가 빨라질 정도
    public float rotationSpeed = 0.02f; // 회전하는 속도 조절
    public GameObject playerCamera; // 플레이어 카메라 할당용 변수
    float wasdDrgree; // 입력받은 방향키의 각도
    public float speedOffset = 1;

    public bool goup;

    //캐릭터 컨트롤러
    CharacterController cc;
    float yVelocity = 0;

    //캐릭터 인풋
    float h;
    float v;
    public bool outputLock = false;

    // 애니메이션 체크
    Animator controll;
    GameObject playerModel;

    void Start()
    {
        // playerCamera 변수에 태그가 MainCamera인 게임 오브젝트를 참조한다.
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cc = GetComponent<CharacterController>();

        playerModel = GameObject.Find("test2");
        controll = playerModel.GetComponent<Animator>();
    }

    void Update()
    {
        // 사용자의 입력 받기
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (outputLock == false)
        {
            controll.SetBool("ismove", true);
            PlayerRotaion();
            PlayerMoving();
        }
        else
        {
            controll.SetBool("ismove", false);
        }
    }

        void PlayerRotaion()
    {
        // 키 입력이 들어올 때만 회전 해야 됨
        if (v != 0 || h != 0)
        {
            // wasd 입력에 따른 각도를 구해보기
            Vector2 playerkeyinput = new Vector2(h, v);
            wasdDrgree = Vector2.SignedAngle(new Vector2(0, 1), playerkeyinput); // 백터의 각도를 구하는 함수 (0 ~ 360), 백터의 내각

            // 카메라를 기준으로 각도에 따라 방향을 바라보게 만들기
            // Quaternion.Slerp(움직여야 할 값, 타겟 값, 속도)
            transform.rotation = Quaternion.Slerp(transform.rotation, playerCamera.transform.rotation * Quaternion.Euler(0, wasdDrgree * -1, 0), rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
    }

    void PlayerMoving()
    {
        // 키 입력을 받았을 때 입력한 값에 따라 카메라를 기준으로 해당 방향으로 걸어감
        Vector3 playerMoveKeyInput = new Vector3(h, 0, v); // 플레이어 입력 값 백터에 할당
        float cameraDrgree = playerCamera.transform.eulerAngles.y; // 카메라 로테이션의 앵글러 Y 값 받아오기
        Vector3 dir = Quaternion.Euler(0, cameraDrgree, 0) * playerMoveKeyInput; // 입력값의 백터에 카메라 회전 각 만큼 회전 ( A * B 하면 A에서 B의 각만큼 돌아감)

        //dir값의 길이가 1이 넘으면 노멀라이즈

        if (Vector3.Magnitude(dir) > 1)
        {
            dir = dir.normalized;
        }

        // 중력 값 추가
        if (goup)
        {
            yVelocity = 0;
        }
        else
        {
            yVelocity = 0;
            yVelocity += -100f * Time.deltaTime;
        }
        dir.y = yVelocity;

        if (Input.GetKey(KeyCode.LeftShift)) // 뛰기 여부를 판단하고 움직이게 만든다.
        { cc.Move(dir * moveSpeed * Time.deltaTime * runMoveSpeedRatio); }
        else { cc.Move(dir * moveSpeed * Time.deltaTime * speedOffset); }
    }

    public void Playerforward(float forwardspeed)
    {
        //print("앞으로조금가기!");
        // 애니메이션 플레이 중
        float lookR = transform.eulerAngles.y; // 현재 플레이어의 Y 로테이션 값

        Vector3 rolldir = Quaternion.Euler(0, lookR, 0) * Vector3.forward; // 앞 방향을 기준으로 * lookR도를 돌려야 됨
        cc.Move(rolldir * forwardspeed * Time.deltaTime); // 그 뱡향으로 구름

        //(rolldir);
    }

    public void Playerright(float rightspeed)
    {
        //print("앞으로조금가기!");
        // 애니메이션 플레이 중
        float lookR = transform.eulerAngles.y; // 현재 플레이어의 Y 로테이션 값

        Vector3 rolldir = Quaternion.Euler(0, lookR + 90, 0) * Vector3.forward; // 앞 방향을 기준으로 * lookR도를 돌려야 됨
        cc.Move(rolldir * rightspeed * Time.deltaTime); // 그 뱡향으로 구름

        //print(rolldir);
    }

    public void Playerup(float upSpeed)
    {
        cc.Move(new Vector3 (0, 1, 0) * upSpeed * Time.deltaTime);
        if (upSpeed == 0)
        {
            goup = false;
        }
        else
        {
            goup = true;
        }
    }


}

