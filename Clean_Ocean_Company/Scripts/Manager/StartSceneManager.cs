using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public string url;

    public List<TMP_InputField> inputFields;

    public TMP_Text typeText;

    public TMP_Text playerNameInputText;
    public TMP_Text playerPasswordInputText;
    public TMP_Text playerNameText;
    

    public GameObject inputPanel;

    public GameObject BeforeBottomPanel;
    public GameObject AfterBottomPanel;

    public GameObject StampText;

    public TMP_Text createbutton;

    public StartScenePhotonManager loginPun;

    public SoundManger soundManger;


    float currentTime = 0;

    bool isCheckTrue = false;

    public enum SceneType
    {
        ProtoTypeLobby,
        AlphaLobby_Lia,
        AlphaLobby_Yoon2
    }

    public SceneType sceneType = SceneType.ProtoTypeLobby;

    void Start()
    {
        SetResolution();

        currentTime = 0;
        playerNameText.text = string.Empty;
        playerPasswordInputText.text = string.Empty;

        soundManger = GameObject.FindObjectOfType<SoundManger>();
        soundManger.BGMSetting(3);
    }

    public void SetResolution()
    {
        int setWidth = 1920; // 화면 너비
        int setHeight = 1080; // 화면 높이

        //해상도를 설정값에 따라 변경
        //3번째 파라미터는 풀스크린 모드를 설정 > true : 풀스크린, false : 창모드
        Screen.SetResolution(setWidth, setHeight, true);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isCheckTrue)
        {
            if (inputFields[0].text.Length != 0 && inputFields[1].text.Length != 0)
            {
                soundManger.UISFXPlayRandom(8, 10);
                typeText.text = "로그인 시도 중...";
                isCheckTrue = true;
                inputPanel.SetActive(false);
                PhotonNetwork.NickName = inputFields[0].text;
                print($"check : {PhotonNetwork.NickName}");
                //StartCoroutine(MoveNextScene());
                loginPun.JoinId(url);
            }
            else
            {
                soundManger.UISFXPlay(11);
                typeText.text = "잘못된 정보를 입력하셨습니다.";
                print("로그인 실패");
            }
        }
    }


    public void LoginResult(string result)
    {
        print(result);


        // 이미 존재하는 id
        if (result == "false")
        {
            soundManger.UISFXPlay(11);
            createbutton.text = "이미 아이디가 존재합니다";
            isCheckTrue = false;
        }
        // 아이디가 오류
        else if (result == "아이디 오류")
        {
            soundManger.UISFXPlay(11);
            typeText.text = "...아이디..오류...";
            isCheckTrue = false;
            inputPanel.SetActive(true);
        }
        //비밀번호 오류
        else if (result == "비밀번호 오류")
        {
            soundManger.UISFXPlay(11);
            typeText.text = "...비밀번호..오류...";
            isCheckTrue = false;
            inputPanel.SetActive(true);
        }
        // 회원가입이 성공했거나 로그인이 성공함
        else if(!result.Contains("Error"))
        {
            print("로그인 성공");
            if (typeText.gameObject.activeInHierarchy)
            {
                print("로그인 창");
                typeText.text = "....환.영.합.니.다....";
                soundManger.UISFXPlayRandom(8, 10);
                StartCoroutine(MoveNextScene());

            }
            else
            {
                BeforeBottomPanel.SetActive(false);
                AfterBottomPanel.SetActive(true);
                print("회원가입 창");
                StartCoroutine(IStampEvent());
            }
        }
        else
        {
            soundManger.UISFXPlay(11);
            typeText.text = "서버 연결 실패, 오프라인 모드 실행";
            Debug.LogError("로그인 실패. 오프라인 모드 실행...");
            // fail 오프라인으로 실행
            StartCoroutine(MoveNextScene());
        }
    }
    // 로비 씬으로 이동
    IEnumerator MoveNextScene()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
        Camera.main.transform.DOMoveX(19.9f, 1.0f, false).SetEase(Ease.InCirc);

        yield return new WaitForSeconds(1.5f);

        if (SaveSys.saveSys != null)
        {
            SaveSys.saveSys.setPath();
            SaveSys.saveSys.DataLoad();
        }
        SceneType scene = sceneType;
        PhotonNetwork.LoadLevel(scene.ToString());
    }

    public void JobEvent()
    {
        CheckText(playerNameInputText.text, playerPasswordInputText.text);
    }

    void CheckText(string name, string password)
    {
        if (name != "" && password != "")
        {
            soundManger.UISFXPlayRandom(8, 10);
            StopAllCoroutines();
            currentTime = 0;
            playerNameText.text = playerNameInputText.text;
            PhotonNetwork.NickName = playerNameText.text;
            loginPun.CreateId(url);
            //StartCoroutine(IStampEvent());
        }
        else
        {
            soundManger.UISFXPlay(11);
            createbutton.text = "아이디 혹은 비밀번호를 작성하지 않으셨습니다.";
            print("아이디 혹은 비밀번호를 작성하지 않으셨습니다.");
            return;
        }
    }

    public IEnumerator IStampEvent()
    {
        while (currentTime < 2.5f)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        currentTime = 0;

        StampText.SetActive(true);

        SoundManger soundManger = GameObject.FindObjectOfType<SoundManger>();
        soundManger.UISFXPlay(12);

        StartCoroutine(MoveNextScene());
    }

    public void Btn_sound()
    {
        soundManger.UISFXPlayRandom(8, 10);
    }

}
