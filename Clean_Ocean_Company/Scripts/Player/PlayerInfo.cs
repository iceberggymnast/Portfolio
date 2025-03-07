using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
    public List<GameObject> playerList = new List<GameObject>();

    public static PlayerInfo instance;

    private GameObject _playerObj;

    public Vector3 instantiatePos = new Vector3(-30, 30, 127);

    // 내 플레이어 게임 오브젝트
    public GameObject playerObj
    {
        get => _playerObj;
        set
        {
            _playerObj = value;
            OnPlayerChanged?.Invoke(_playerObj);
        }
    }

    Vector3 playerPos;

    // player 값이 변경될 때 발생하는 이벤트
    public event Action<GameObject> OnPlayerChanged;

    public  GameObject player;

    // 인터렉션중인 게임오브젝트
    public GameObject currnetInteraction;

    // 포인트 내역
    public int point = 0;

    // 산소통
    GameObject O2_item;

    //플레이어 체력
    public float maxPlayerHP = 100;  //최대 체력
    public float currentPlayerHP =100;     //현재 체력
    float shiftingTime = 4.0f;    //빨리 달릴 때 체력 줄어들게 하는 속도
    float vacummingTime = 3.0f;    //흡입시 사용 시 체력 줄어들게 하는 속도

    // 커서 여부 설정
    public bool isCusor = true;

    [Header("땅 이동")]
    public float groundSpeed;
    public float groundJumpPower;
    public float jumpHeight = 2.0f;

    [Header("바다 이동")]
    public float oceanSpeed;
    [Range(0.1f, 3.0f)]
    public float waterResistance = 0.9f; // 물 저항(속도가 빠르게 줄어들도록)
    //손전등이 켜져있는 지 여부
    public bool isFlashLightOn = false;
    public bool isFlashStop = false;

    [Header("산소 시스템")]
    //산소 상태
    public float oxygen = 1000.0f;
    //최대 산소량
    [Header("장비 upgrade: 산소 저장 용량")]
    public float maxOxygen = 1000.0f;
    //안전지역일 때, 산소 충전 속도
    public float decreaseSpeed_Ocean = 10.0f;
    //오염지역일 때, 산소 감소 속도
    public float decreaseSpeed_Ocean1 = -2.0f;
    //위험지역일 때, 산소 감소 속도
    public float decreaseSpeed_Ocean2 = -4.0f;
    //LeftShift키 눌러서 빨리 이동할 때, 산소 감소 속도
    public float decreaseSpeed_LeftShift = -10.0f;

    //현재 보유한 산소충전기 개수
    public int current_OxygenchargerAmount;

    public PointUpFactory pointUpFactory;

    //shift키 눌러서 빨리이동할 때 fov값을 천천히 설정해주는 변수(lerp에 사용)
    public float targetFOV = 60f;
    public float fovSpeed = 2.0f; // fov 변화 속도

    public Vector3 cameraPos;

    //장비 강화 관련 4가지
    [Header("장비 upgrade: 쓰레기 흡입 속도")]
    //쓰레기 흡입 속도
    public float cleaningSpeed;

    // 감지 범위의 길이와 각도
    [Header("장비 upgrade: 쓰레기 흡입 범위")]
    public float detectionDistance = 10.0f;

    [Header("장비 upgrade: 쓰레기 저장용량")]
    public float maxTrashcanCapacity = 10;  //최대 용량

    public Trashcan_UI trashcan_UI;

    public enum PlayerModel
    {
        Player,
        PlayerYoon
    }
    public PlayerModel playerModel = PlayerModel.Player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
        cameraPos = new Vector3(1.5f, 1.2f, -5);
    }

    void UpdatePlayerObject(GameObject player)
    {
        _playerObj = player;
        playerList.Add(player);
    }

    // 씬이 로드될때 하는 작업
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        GameObject obj = GameObject.Find("Canvas_Player");
        if (obj != null)
        {
            pointUpFactory = obj.GetComponent<PointUpFactory>();
        }

        //print($"PhotonNetwork.InRoom : {PhotonNetwork.InRoom}");

        //if (player == null && PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        //{
        //    player = PhotonNetwork.Instantiate("Player/Player", instantiatePos, Quaternion.identity);
        //}

        // GI 업데이트
        DynamicGI.UpdateEnvironment();
    }

    public override void OnJoinedRoom()
    {
        //if (player == null && !PhotonNetwork.IsMasterClient)
        //{
        //    player = PhotonNetwork.Instantiate("Player/Player", instantiatePos, Quaternion.identity);
        //}
        StartCoroutine(iC());
    }

    public IEnumerator iC()
    {
        if (player == null) yield return new WaitForSeconds(3f);
        //player = PhotonNetwork.Instantiate("Player/Player", Vector3.zero, Quaternion.identity);
        player = PhotonNetwork.Instantiate($"Player/{playerModel.ToString()}", instantiatePos, Quaternion.identity);
        UpdatePlayerObject(player);
    }

    private void Update()
    {
        CursorCheck();
        recovery_HP();
        Use_HP();

        if(oxygen < 0)
        {
            oxygen = 0;
        }

    }

    void CursorCheck()
    {

        if (isCusor)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = isCusor;


    }

    //포인트 값 추가 또는 감소하는 함수
    public void PointPlusOrMinus(int pointvalue)
    {
        point += pointvalue;

        //포인트가 0이하로 내려가지 않게 예외처리
        if(point < 0)
        {
            point = 0;
        }

        //UI연동
        pointUpFactory.AddPoint(Vector3.zero, pointvalue);
    }

    //체력 자동으로 회복되는  함수
    public void recovery_HP()
    {
        //체력 회복은 10씩
        currentPlayerHP += Time.deltaTime *5;

        //최대체력 이상으로는 회복되지 않도록
        if (currentPlayerHP > maxPlayerHP)
        {
            currentPlayerHP = maxPlayerHP;
        }
    }

    //빨리 달리거나(shift), 흡입기 사용시(왼클릭) 시 스테미나가 닳도록
    public void Use_HP()
    {
        PlayerMove playerMove = FindObjectOfType<PlayerMove>();
        if (playerMove == null) return;


        if (playerMove == null) return;

        //빨리 달릴 때_(leftShitft 눌렀을 때)
        //if (playerMove.isOcean && Input.GetKey(KeyCode.LeftShift))
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentPlayerHP -= Time.deltaTime * shiftingTime;
        }

        //흡입시 사용할 때_(바다속에서 왼클릭 눌렀을 때)
        //f (playerMove.isOcean && Input.GetMouseButton(0))
        if (Input.GetMouseButton(0))
        {
            currentPlayerHP -= Time.deltaTime * vacummingTime;
        }

        //스테미나가 0보다 작아지지 않도록
        if (currentPlayerHP < 0)
        {
            currentPlayerHP = 0;
        }

        //Debug.Log($"현재 스테미나: {currentPlayerHP}/{maxPlayerHP}");
    }


}
