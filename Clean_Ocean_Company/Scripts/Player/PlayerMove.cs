using DG.Tweening;
using Photon.Pun;
using StylizedWater2;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerMove : MonoBehaviour
{
    //땅 버전 이동 및 점프 관련
    Vector3 dir;
    CharacterController cc;
    float yVelocity;
    //float gravity = -9.81f;

    Vector3 moveDir;

    //바다 버전 이동 관련
    public float decreaseSpeed;

    //플레이어 회전 속도
    public float rotationSpeed = 10.0f;

    //바다인지 확인하는 bool값
    public bool isOcean = false;

    // 움직임을 관리하는 bool값
    public bool isMove = true;

    //PlayerInfo
    PlayerInfo playerInfo;

    //MainCamera
    Camera mainCamera;

    UIController uIController;

    PopupUI popupUI;

    PhotonView photonView;
    Vector3 playerCameraForward;

    HelpUIController helpUIController;

    bool isFirstStart = true;

    private void Awake()
    {
        Debug.Log("플레이어 생성됨");
        uIController = GameObject.FindObjectOfType<UIController>();
        popupUI = GameObject.FindObjectOfType<PopupUI>();
    }

    private void OnDestroy()
    {
        Debug.Log("플레이어 제거 됨");
    }

    void Start()
    {
        cc = transform.parent.GetComponent<CharacterController>();

        mainCamera = transform.parent.transform.GetChild(1).GetChild(0).GetComponent<Camera>();

        photonView = transform.parent.GetComponent<PhotonView>();
        helpUIController = GameObject.FindObjectOfType<HelpUIController>();
    }

    void Update()
    {
        if (photonView != null)
        {
            if (!photonView.IsMine) return;
        }
        OceanMove();

        //산소 감소 또는 충전해주는 함수
            
        OxygenHandle();

    }

    int checkstage = -1;
    // 안전지대: Ocean = 0
    // 오염지대: Ocean_1 = 1
    // 위험지대: Ocean_2 = 2

    //바다 구역을 확인하는 함수
    private void OnTriggerEnter(Collider other)
    {
        if (photonView != null)
        {
            if (!photonView.IsMine) return;
        }
        //안전지대에서는 산소가 충전됨. 단 최대충전용량까지만 충전됨
        if (other.gameObject.tag == "Ocean" && checkstage != 0)
        {
            checkstage = 0;
            decreaseSpeed = PlayerInfo.instance.decreaseSpeed_Ocean;
            isOcean = true;
            StartCoroutine(uIController.FadeIn("PointUI", 0.4f));
            popupUI.PopupActive("안전지대", "이곳은 안전한 지대입니다.@여기서 산소를 공급할 수 있고, 쓰레기도 비울 수 있습니다.@혹시 도움이 필요한 분이 있을 지도 모르니 살펴보는 것도 좋겠습니다.", 1.5f);
            if (isFirstStart)
            {
                isFirstStart = false;
                return;
            }
            //BriefingUI.instance.SetText("이곳은 안전한 지대입니다.\n여기서 산소를 공급할 수 있고, 쓰레기도 비울 수 있습니다.@혹시 도움이 필요한 분이 있을 지도 모르니\n살펴보는 것도 좋겠습니다.", 3f);

            SoundManger soundManger = GameObject.FindObjectOfType<SoundManger>();
            soundManger.BGMSetting(0);
            /*
            Volume volume = GameObject.FindAnyObjectByType<Volume>();
            ColorAdjustments colorAdjustments;
            volume.profile.TryGet(out colorAdjustments);
            if (colorAdjustments != null)
            {
                // DOTween.To를 사용하여 startPadding에서 targetPadding으로 애니메이션
                DOTween.To(
                    () => colorAdjustments.contrast.value,
                x =>
                {
                    colorAdjustments.contrast.value = x;
                },
                    15,                 // 목표 값
                    3f                             // 애니메이션 시간(초)
                );
            }
            */ 
            UnderwaterRenderer underwaterRenderer = GameObject.FindObjectOfType<UnderwaterRenderer>();
            if (underwaterRenderer != null)
            {
                // DOTween.To를 사용하여 startPadding에서 targetPadding으로 애니메이션
                DOTween.To(
                    () => underwaterRenderer.fogDensity,
                x => {
                    underwaterRenderer.fogDensity = x;
                },
                    1,                 // 목표 값
                    3f                             // 애니메이션 시간(초)
                );

                DOTween.To(
                    () => underwaterRenderer.startDistance,
                x => {
                    underwaterRenderer.startDistance = x;
                },
                    15,                 // 목표 값
                    3f                             // 애니메이션 시간(초)
                );
            }


        }
        else if (other.gameObject.tag == "Ocean_1" && checkstage != 1)
        {
            helpUIController.isHelpUiClose = true;
            checkstage = 1;
            isOcean = true;
            decreaseSpeed = PlayerInfo.instance.decreaseSpeed_Ocean1;
            StartCoroutine(uIController.FadeOut("PointUI", 0.4f));
            popupUI.PopupActive("오염지대", "해당 지역은 심각하게 오염되었습니다.@기름 유출 시, 유출 지역 뿐만 아니라\n주변도 심각한 오염에 빠집니다.", 1.5f);
            //BriefingUI.instance.SetText("해당 지역은 심각하게 오염되었습니다.@기름 유출 시,\n유출 지역 뿐만 아니라,\n주변도 심각한 오염에 빠집니다.", 2f);


            SoundManger soundManger = GameObject.FindObjectOfType<SoundManger>();
            soundManger.BGMSetting(1);
            /*
            Volume volume = GameObject.FindAnyObjectByType<Volume>();
            ColorAdjustments colorAdjustments;
            volume.profile.TryGet(out colorAdjustments);
            if (colorAdjustments != null)
            {
                // DOTween.To를 사용하여 startPadding에서 targetPadding으로 애니메이션
                DOTween.To(
                    () => colorAdjustments.contrast.value,
                x => {
                    colorAdjustments.contrast.value = x;
                },
                    - 5,                 // 목표 값
                    3f                             // 애니메이션 시간(초)
                );
            }
            */
            UnderwaterRenderer underwaterRenderer = GameObject.FindObjectOfType<UnderwaterRenderer>();
            if (underwaterRenderer != null)
            {
                // DOTween.To를 사용하여 startPadding에서 targetPadding으로 애니메이션
                DOTween.To(
                    () => underwaterRenderer.fogDensity,
                x => {
                    underwaterRenderer.fogDensity = x;
                },
                    3,                 // 목표 값
                    3f                             // 애니메이션 시간(초)
                );

                DOTween.To(
                    () => underwaterRenderer.startDistance,
                x => {
                    underwaterRenderer.startDistance = x;
                },
                    1,                 // 목표 값
                    3f                             // 애니메이션 시간(초)
                );
            }
        }
        else if (other.gameObject.tag == "Ocean_2" && checkstage != 2)
        {
            helpUIController.isHelpUiClose = true;
            checkstage = 2;
            isOcean = true;
            decreaseSpeed = PlayerInfo.instance.decreaseSpeed_Ocean2;
            StartCoroutine(uIController.FadeOut("PointUI", 0.4f));
            popupUI.PopupActive("위험지대", "해당 지역은 앞이 전혀 보이지 않습니다.@아마 기름이 유출되는 것 같습니다.\n신속하게 막아야 합니다.", 1.5f);
            //BriefingUI.instance.SetText("해당 지역은 앞이 전혀 보이지 않습니다.@아마 기름이 유출되는 것 같습니다.@신속하게 막아야 합니다.", 2f);


            SoundManger soundManger = GameObject.FindObjectOfType<SoundManger>();
            soundManger.BGMSetting(2);
            /*
            Volume volume = GameObject.FindAnyObjectByType<Volume>();
            ColorAdjustments colorAdjustments;
            volume.profile.TryGet(out colorAdjustments);
            if (colorAdjustments != null)
            {
                // DOTween.To를 사용하여 startPadding에서 targetPadding으로 애니메이션
                DOTween.To(
                    () => colorAdjustments.contrast.value,
                x => {
                    colorAdjustments.contrast.value = x;
                },
                    -30,                 // 목표 값
                    3f                             // 애니메이션 시간(초)
                );
            }
            */
            UnderwaterRenderer underwaterRenderer = GameObject.FindObjectOfType<UnderwaterRenderer>();
            if (underwaterRenderer != null)
            {
                // DOTween.To를 사용하여 startPadding에서 targetPadding으로 애니메이션
                DOTween.To(
                    () => underwaterRenderer.fogDensity,
                x => {
                    underwaterRenderer.fogDensity = x;
                },
                    8,                 // 목표 값
                    3f                             // 애니메이션 시간(초)
                );

                DOTween.To(
                    () => underwaterRenderer.startDistance,
                x => {
                    underwaterRenderer.startDistance = x;
                },
                    1,                 // 목표 값
                    3f                             // 애니메이션 시간(초)
                );
            }

        }
        else if (other.gameObject.tag == "Ground")
        {
            isOcean = false;
        }
    }

    //산소 충전
    public void RefillOxygen()
    {
        //양수로 만들어주기
        //decreaseSpeed = -1;

        //최댓값이상으로 채워지지 않도록 범위 이상 충전되었을 때 최댓값으로 바꿔주기
        if (PlayerInfo.instance.oxygen > PlayerInfo.instance.maxOxygen)
        {
            PlayerInfo.instance.oxygen = PlayerInfo.instance.maxOxygen;
        }
    }

    //산소 감소 또는 충전해주는 함수
    public void OxygenHandle()
    {
        PlayerInfo.instance.oxygen += Time.deltaTime * decreaseSpeed; 
        RefillOxygen();
    }

    // 땅 버전 좌우상하 이동 함수
    //public void GroundMove()
    //{
    //    transform.localPosition = Vector3.zero;
    //    //좌우상하 이동
    //    float h = Input.GetAxis("Horizontal");
    //    float v = Input.GetAxis("Vertical");

    //    dir = new Vector3(h, 0, v);
    //    dir.Normalize();

    //    cc.Move(dir * playerInfo.groundSpeed * Time.deltaTime);
    //}

    ////땅 버전 점프
    /////점프 필요하면 단축키 스페이스바 쓸 수 있는지 확인할 것
    //public void GroundJump()
    //{
    //    //만약에 땅에 있으면 yVelocity를 0으로 초기화
    //    if (cc.isGrounded)
    //    {
    //        yVelocity = 0;

    //        //스페이스 바 눌렀을 때 점프
    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            // 점프 속도 계산
    //            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
    //        }
    //    }

    //    //yVelocity값을 중력에 의해서 변경시키자.
    //    yVelocity += gravity * Time.deltaTime;

    //    //dir.y 에 yVelocity값을 셋팅
    //    dir.y = yVelocity;

    //    cc.Move(dir * GroundSpeed * Time.deltaTime);
    //}


    // 바다 버전 좌우상하 이동 함수
    public void OceanMove()
    {
        if(!isMove) return;

        if (mainCamera == null)
        {
            mainCamera = transform.parent.transform.GetChild(1).GetChild(0).GetComponent<Camera>();
        }
        playerCameraForward = mainCamera.transform.forward;

        //y축 방향 제거(수평으로만 사용)
        playerCameraForward.y = 0;  //카메라의 앞 방향 벡터를 수평으로 제한
        playerCameraForward.Normalize();
        //print(playerCamera.transform.eulerAngles);

        //사용자 입력을 기반으로 이동 방향 설정
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));   //사용자 입력에서 수직 성분을 제거


        //방향 정규화
        if (inputDirection.magnitude > 1)
        {
            inputDirection.Normalize();
        }

        //카메라의 앞 방향과 입력 방향을 결합하여 플레이어의 이동 방향 설정
        //z는 수직방향(앞/뒤 이동)
        //x는 수평방향(좌/우 이동)
        //playerCameraForward * inputDirection.z는 카메라의 앞 방향에 따라 조정된 벡터
        //playerCamera.transform.right * inputDirection.x는 카메라의 오른쪽 방향에 따라 조정된 벡터
        moveDir = (playerCameraForward * inputDirection.z) + (mainCamera.transform.right * inputDirection.x);

        //상승 및 하강처리
        float yVelocity = 0f;  //yVelocity초기화

        // LeftAlt키 누르는 동안 상승
        if (Input.GetAxis("Fire2") > 0)
        {
            yVelocity = Input.GetAxis("Fire2");  // 상승 속도
            if (moveDir.magnitude > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                PlayerInfo.instance.oceanSpeed = 4 + (6 * Input.GetAxis("Fire2"));
                PlayerInfo.instance.targetFOV = 100; // 목표 fov 설정
            }
        }
        // LeftControl키 누르는 동안 하강
        else if (Input.GetAxis("Fire1") > 0)
        {
            yVelocity = -Input.GetAxis("Fire1");  // 하강 속도
            if (moveDir.magnitude > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                PlayerInfo.instance.oceanSpeed = 4 + (6 * Input.GetAxis("Fire1"));
                PlayerInfo.instance.targetFOV = 70; // 목표 fov 설정
            }
        }
        // LeftShift 누르는 동안 이동 속도 빠르게
        else if (PlayerInfo.instance.currentPlayerHP > 0 && moveDir.magnitude > 0 &&  Input.GetKey(KeyCode.LeftShift))
        {
            PlayerInfo.instance.oceanSpeed = 4 + (6 * Input.GetAxis("Fire3"));
            //산소 감소 평소보다 많이 되도록
            //decreaseSpeed = playerInfo.decreaseSpeed_LeftShift;
            PlayerInfo.instance.targetFOV = 70; // 목표 fov 설정
        }
        else
        {
            PlayerInfo.instance.oceanSpeed = 4;
            PlayerInfo.instance.targetFOV = 60; // 목표 fov를 기본값으로 설정

        }
        // fov를 부드럽게 변경
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, PlayerInfo.instance.targetFOV, PlayerInfo.instance.fovSpeed * Time.deltaTime);

        //플레이어의 로컬 방향으로 회전(상숭 또는 하강이 아닐 때만, 플레이어 방향 회전시키기)
        if (moveDir.magnitude > 0.1f)
        {
            //dir 벡터 기준으로 목표 회전(quaternion)을 계산
            //LookRotation 함수는 주어진 방향을 바라보는 회전을 생성
            Quaternion PlayerRotation = Quaternion.LookRotation(moveDir);
            //회전 속도를 보간
            if (!PlayerInfo.instance.isFlashLightOn)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, PlayerRotation.normalized, Time.deltaTime * rotationSpeed);
            }
            else
            {
                if (inputDirection.z != 0)
                {
                    transform.forward = Vector3.Slerp(transform.forward, Camera.main.transform.forward, Time.deltaTime * rotationSpeed);
                    transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
                }


            }

            ////손전등도 같이 회전
            //Quaternion flashLightRotation = Quaternion.LookRotation(moveDir);
            ////회전 속도를 보간
            //GameObject flashLight;
            //flashLight = GameObject.Find("Player").transform.GetChild(2).gameObject;
            //flashLight.transform.rotation = Quaternion.Slerp(flashLight.transform.rotation, flashLightRotation.normalized, Time.deltaTime * rotationSpeed);
        }
        //float Dot = Vector3.Dot(transform.forward, new Vector3(0, Camera.main.transform.parent.forward.y, Camera.main.transform.parent.forward.z));
        //float Angle = Mathf.Acos(Dot) * Mathf.Rad2Deg;
        if (PlayerInfo.instance.isFlashLightOn)
        {
            float angle = Mathf.Abs(transform.localEulerAngles.y - Camera.main.transform.parent.localEulerAngles.y);
            if (angle > 45)
            {
                transform.forward = Vector3.Slerp(transform.forward, Camera.main.transform.forward, Time.deltaTime * rotationSpeed);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
        }


            
        //yVelocity를 moveDir에 추가
        moveDir.y = yVelocity;

        //캐릭터 이동
        cc.Move(moveDir * PlayerInfo.instance.oceanSpeed * Time.deltaTime);

    }
}
