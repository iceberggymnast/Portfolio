using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainIntro : MonoBehaviour
{

    public List<GameObject> intro;
    
    float currenttime = 0;

    public GameObject mainmenu;

    bool triscreen;
    bool fivescreen;
    bool sixscreen;

    void Start()
    {

    }

    void Update()
    {
        currenttime += Time.deltaTime;

        // 첫번째 UI
        UifadeinoutMange(1, 3, 6, 0, 1);

        // 두번째 UI
        UifadeinoutMange(7, 9, 12, 1, 1);

        // 세번째 UI
        UifadeinoutMange(12, 13, 17, 2, 4);
        if (currenttime > 12 && !triscreen)
        {
            gameObject.GetComponent<AudioSource>().Play();
            triscreen = true;
        }

        // 네번째 UI
        UifadeinoutMange(17, 18, 19, 3, 4);

        // 다섯번째 UI
        UifadeinoutMange(19, 20, 22, 4, 8);
        if (currenttime > 19 && !fivescreen)
        {
            gameObject.GetComponent<AudioSource>().Play();
            fivescreen = true;
        }

        // 여섯번째 UI
        UifadeinoutMange(20.5f, 21, 22, 5, 4);
        if (currenttime > 20.5f && !sixscreen)
        {
            gameObject.GetComponent<AudioSource>().Play();
            sixscreen = true;
        }

        // 일곱번재 UI
        UifadeinoutMange(22, 27, 33, 6, 3);

        // 여덟번째 UI (블랙 스크린)
        UifadeinoutMange(33, 33, 37, 7, 1);

        // 22초 전까진 마우스 뜨면 안댐
        if (currenttime < 22)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true; 
        }

        // F나 클릭하면 누르면 27초로 점프
        if (currenttime < 27 && currenttime > 22)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                currenttime = 27;
            }
        }

        // 알파값 낮아지면 키키
        if (intro[7].gameObject.GetComponent<Image>().color.a < 1)
        {
            mainmenu.SetActive(true);
        }

        // 스크린 완전히 꺼지면 캔버스 디스트로이
        if (intro[7].gameObject.GetComponent<Image>().color.a < 0.1)
        {
            Destroy(gameObject);
        }

    }

    public void skip()
    {
        mainmenu.SetActive(true);
        Destroy(gameObject);
        Cursor.visible = true;
    }


    // 페이드 인 아웃 함수
    public void FadeOut(GameObject ui, float speed)
    {
        Image img = ui.GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - Time.deltaTime * speed);
    }

    public void FadeIn(GameObject ui, float speed)
    {
        Image img = ui.GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + Time.deltaTime * speed);
    }

    // 매개변수에 따라 등장 시점 설정 가능한 함수
    public void UifadeinoutMange(float starttime, float fadeintime, float fadeouttime, int listnum, float speed)
    {
        if (currenttime > starttime)
        {
            if (currenttime < fadeintime)
            {
            FadeIn(intro[listnum], speed);
            }
            else if (currenttime < fadeouttime)
            {
            FadeOut(intro[listnum], speed);
            }
        }
    }

}
