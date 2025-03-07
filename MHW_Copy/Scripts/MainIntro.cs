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

        // ù��° UI
        UifadeinoutMange(1, 3, 6, 0, 1);

        // �ι�° UI
        UifadeinoutMange(7, 9, 12, 1, 1);

        // ����° UI
        UifadeinoutMange(12, 13, 17, 2, 4);
        if (currenttime > 12 && !triscreen)
        {
            gameObject.GetComponent<AudioSource>().Play();
            triscreen = true;
        }

        // �׹�° UI
        UifadeinoutMange(17, 18, 19, 3, 4);

        // �ټ���° UI
        UifadeinoutMange(19, 20, 22, 4, 8);
        if (currenttime > 19 && !fivescreen)
        {
            gameObject.GetComponent<AudioSource>().Play();
            fivescreen = true;
        }

        // ������° UI
        UifadeinoutMange(20.5f, 21, 22, 5, 4);
        if (currenttime > 20.5f && !sixscreen)
        {
            gameObject.GetComponent<AudioSource>().Play();
            sixscreen = true;
        }

        // �ϰ����� UI
        UifadeinoutMange(22, 27, 33, 6, 3);

        // ������° UI (�� ��ũ��)
        UifadeinoutMange(33, 33, 37, 7, 1);

        // 22�� ������ ���콺 �߸� �ȴ�
        if (currenttime < 22)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true; 
        }

        // F�� Ŭ���ϸ� ������ 27�ʷ� ����
        if (currenttime < 27 && currenttime > 22)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                currenttime = 27;
            }
        }

        // ���İ� �������� ŰŰ
        if (intro[7].gameObject.GetComponent<Image>().color.a < 1)
        {
            mainmenu.SetActive(true);
        }

        // ��ũ�� ������ ������ ĵ���� ��Ʈ����
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


    // ���̵� �� �ƿ� �Լ�
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

    // �Ű������� ���� ���� ���� ���� ������ �Լ�
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
