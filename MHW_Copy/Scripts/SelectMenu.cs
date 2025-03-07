using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SelectMenu : MonoBehaviour
{
    // int의 값에 따라서 메뉴 리스트에 할당된 위치로 이동.
    // 마우스와 키보드로 가능하고 키보드는 int 의 값을 조절할 수 있다.

    public List<GameObject> menuList = new List<GameObject>(); // 메뉴 버튼들 넣기
    public List<GameObject> uiimg = new List<GameObject>(); // ui 이미지들
    public int menuNum = 0; // 현재 위치한 메뉴 번호
    public GameObject selectParticle; // 파티클
    public GameObject selectimg; // 버튼 선택 배경 이미지
    float parcent = 0;
    Vector3 particlestartpos;
    TMP_Text currentext;
    public Material hover;
    public Material notHover;
    float timer;

    public bool isnotmain;
    public bool intoquestborad;

    public Camera maincam;
    public Camera tagetcam;
    bool gotocam;
    Vector3 savecampos;
    Vector3 savecamrot;
    Vector3 savetagetcampos;
    Vector3 savetagetcamrot;
    bool issavecampos;
    public GameObject disableui;
    public GameObject enableui;

    public GameObject characterselectionui;

    public List<float> ypos;
    void Start()
    {
        for (int i = 0; i < menuList.Count; i++)
        {
            ypos.Add(menuList[i].GetComponent<RectTransform>().anchoredPosition.y);
        }
    }

    void Update()
    {
        // W키와 S키를 눌렀을때 할당할 리스트 넘버를 1을 더하거나 뺀다.
        // 만약 int의 값이 -1이면 리스트에 할당된 번호로 이동.
        // 만약 int의 값이 리스트 카운트랑 같다면 0으로 

        // W키를 눌렀을때
        if(Input.GetKeyDown(KeyCode.W))
        {
            if (menuNum <= 0)
            {
                menuNum = menuList.Count - 1;
            }
            else
            { 
                menuNum = menuNum - 1;
            }
            gameObject.GetComponent<AudioSource>().Play();
        }

        // S키를 눌렀을때 
        if(Input.GetKeyDown(KeyCode.S))
        {
            if (menuNum == menuList.Count - 1)
            {
                menuNum = 0;
            }
            else
            {
                menuNum = menuNum + 1;
            }
            gameObject.GetComponent<AudioSource>().Play();

        }



        // 키보드로 확인 버튼 눌렀을때 해당 번호의 버튼이 입력 됨 (F 키)

        Particlesystem();

        // 메인메뉴용임
        if (!isnotmain)
        {
            // start시 메뉴 팝업
            timer += Time.deltaTime;
            for (int i = 0; i < menuList.Count; i++)
            {
                Menupopup(menuList[i], timer * 5 - (i * 0.2f), new Vector3(189.6f - 34.6f, ypos[i], 0), new Vector3(189.6f, ypos[i], 0));
            }
            // 메뉴 이미지들도 OP 올라옴
            for (int i = 0; i < uiimg.Count; i++)
            {
                uiimg[i].GetComponent<Image>().color = new Color(1, 1, 1, Mathf.Lerp(0, 1, timer * 10));
            }
        }

        if (intoquestborad)
        {
            gotocam = true;
            timer += Time.deltaTime;
            for (int i = 0; i < menuList.Count; i++)
            {
                menuList[i].GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(new Vector3(100, menuList[i].GetComponent<RectTransform>().anchoredPosition3D.y, menuList[i].GetComponent<RectTransform>().anchoredPosition3D.z), new Vector3(-390, menuList[i].GetComponent<RectTransform>().anchoredPosition3D.y, menuList[i].GetComponent<RectTransform>().anchoredPosition3D.z), timer * 3);
            }

        }   

        // 캠을 타겟 위치로 이동시키고 다 이동하면 전환
        if (gotocam)
        {
            if (maincam == null)
            {
                maincam = GameObject.Find("Main Camera").GetComponent<Camera>();
            }
            if (tagetcam == null)
            {
                tagetcam = GameObject.Find("questboardCamera").GetComponent<Camera>();
            }

            if (!issavecampos)
            {
                savecampos = maincam.transform.position;
                savecamrot = maincam.transform.eulerAngles;
                savetagetcampos = tagetcam.transform.position;
                savetagetcamrot = tagetcam.transform.eulerAngles;
                issavecampos = true;
                maincam.enabled = false;
                tagetcam.enabled = true;
                timer = 0;
            }

            print("위치 전환");
            tagetcam.transform.position = Vector3.Lerp(savecampos, savetagetcampos, timer * 2);
            tagetcam.transform.eulerAngles = Vector3.Lerp(savecamrot, savetagetcamrot, timer * 2);

            if (timer >= 0.5f)
            {
                enableui.SetActive(true);
                gameObject.SetActive(false);
            }

        }

    }

    // 메뉴 팝업용 Lerp 함수
    public void Menupopup(GameObject movegameUI, float timer, Vector3 startpos, Vector3 endpos)
    {
        movegameUI.GetComponent<RectTransform>().anchoredPosition = Vector3.Slerp(startpos, endpos, timer);
        movegameUI.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), timer);
        if (timer > 0)
        {
            movegameUI.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    // 파티클이 현재 설정된 버튼에 이동되게 하는 함수
    public void Particlesystem()
    {
        // 도착 위치값을 할당해줌
        Vector3 particlePostion = new Vector3(menuList[menuNum].transform.position.x - 3, menuList[menuNum].transform.position.y, menuList[menuNum].transform.position.z);

        // 파티클이 endpos에 도착할때만 startpos에 넣어야 함.
        if (selectParticle.transform.position == particlePostion)
        {
            // 파티클이 도착 위치에 도착했으면 시작 위치와 퍼센트 초기화
            particlestartpos = selectParticle.transform.position;
            parcent = 0;
        }
        else // 도착 위치에 없으면 parcent에 값을 계속 넣어줌
        {
            parcent += Time.deltaTime * 10;
        }

        // Lerp 계산으로 파티클 이동 시키기, 배경도 이동시키기
        selectParticle.transform.position = Vector3.Lerp(particlestartpos, particlePostion, parcent);
        selectimg.transform.position = menuList[menuNum].transform.position;

        // 선택된 텍스트를 할당하고 텍스트 효과를 적용시킨다.
        currentext = menuList[menuNum].transform.GetChild(0).GetComponent<TMP_Text>();
        if (!isnotmain)
        {
            currentext.fontSize = 34;
            currentext.fontMaterial = hover;
        }   

        // glow가 반짝거리게 ~~~~~~~~~~~~~~~~~
        hover.SetFloat(ShaderUtilities.ID_GlowPower, 0.3f);

        // 선택되지 않은 텍스트들은 원래값으로 돌아오게 한다
        for (int i = 0; i < menuList.Count; i++)
        {
            if (currentext != menuList[i].transform.GetChild(0).GetComponent<TMP_Text>())
            {
                menuList[i].transform.GetChild(0).GetComponent<TMP_Text>().fontSize = 32;
                menuList[i].transform.GetChild(0).GetComponent<TMP_Text>().fontMaterial = notHover;
            }
        }
    }

    public void Characterselection()
    {
        characterselectionui.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Intoquestboard()
    {
        intoquestborad = true;
    }

}
