using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SelectMenu : MonoBehaviour
{
    // int�� ���� ���� �޴� ����Ʈ�� �Ҵ�� ��ġ�� �̵�.
    // ���콺�� Ű����� �����ϰ� Ű����� int �� ���� ������ �� �ִ�.

    public List<GameObject> menuList = new List<GameObject>(); // �޴� ��ư�� �ֱ�
    public List<GameObject> uiimg = new List<GameObject>(); // ui �̹�����
    public int menuNum = 0; // ���� ��ġ�� �޴� ��ȣ
    public GameObject selectParticle; // ��ƼŬ
    public GameObject selectimg; // ��ư ���� ��� �̹���
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
        // WŰ�� SŰ�� �������� �Ҵ��� ����Ʈ �ѹ��� 1�� ���ϰų� ����.
        // ���� int�� ���� -1�̸� ����Ʈ�� �Ҵ�� ��ȣ�� �̵�.
        // ���� int�� ���� ����Ʈ ī��Ʈ�� ���ٸ� 0���� 

        // WŰ�� ��������
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

        // SŰ�� �������� 
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



        // Ű����� Ȯ�� ��ư �������� �ش� ��ȣ�� ��ư�� �Է� �� (F Ű)

        Particlesystem();

        // ���θ޴�����
        if (!isnotmain)
        {
            // start�� �޴� �˾�
            timer += Time.deltaTime;
            for (int i = 0; i < menuList.Count; i++)
            {
                Menupopup(menuList[i], timer * 5 - (i * 0.2f), new Vector3(189.6f - 34.6f, ypos[i], 0), new Vector3(189.6f, ypos[i], 0));
            }
            // �޴� �̹����鵵 OP �ö��
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

        // ķ�� Ÿ�� ��ġ�� �̵���Ű�� �� �̵��ϸ� ��ȯ
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

            print("��ġ ��ȯ");
            tagetcam.transform.position = Vector3.Lerp(savecampos, savetagetcampos, timer * 2);
            tagetcam.transform.eulerAngles = Vector3.Lerp(savecamrot, savetagetcamrot, timer * 2);

            if (timer >= 0.5f)
            {
                enableui.SetActive(true);
                gameObject.SetActive(false);
            }

        }

    }

    // �޴� �˾��� Lerp �Լ�
    public void Menupopup(GameObject movegameUI, float timer, Vector3 startpos, Vector3 endpos)
    {
        movegameUI.GetComponent<RectTransform>().anchoredPosition = Vector3.Slerp(startpos, endpos, timer);
        movegameUI.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), timer);
        if (timer > 0)
        {
            movegameUI.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    // ��ƼŬ�� ���� ������ ��ư�� �̵��ǰ� �ϴ� �Լ�
    public void Particlesystem()
    {
        // ���� ��ġ���� �Ҵ�����
        Vector3 particlePostion = new Vector3(menuList[menuNum].transform.position.x - 3, menuList[menuNum].transform.position.y, menuList[menuNum].transform.position.z);

        // ��ƼŬ�� endpos�� �����Ҷ��� startpos�� �־�� ��.
        if (selectParticle.transform.position == particlePostion)
        {
            // ��ƼŬ�� ���� ��ġ�� ���������� ���� ��ġ�� �ۼ�Ʈ �ʱ�ȭ
            particlestartpos = selectParticle.transform.position;
            parcent = 0;
        }
        else // ���� ��ġ�� ������ parcent�� ���� ��� �־���
        {
            parcent += Time.deltaTime * 10;
        }

        // Lerp ������� ��ƼŬ �̵� ��Ű��, ��浵 �̵���Ű��
        selectParticle.transform.position = Vector3.Lerp(particlestartpos, particlePostion, parcent);
        selectimg.transform.position = menuList[menuNum].transform.position;

        // ���õ� �ؽ�Ʈ�� �Ҵ��ϰ� �ؽ�Ʈ ȿ���� �����Ų��.
        currentext = menuList[menuNum].transform.GetChild(0).GetComponent<TMP_Text>();
        if (!isnotmain)
        {
            currentext.fontSize = 34;
            currentext.fontMaterial = hover;
        }   

        // glow�� ��¦�Ÿ��� ~~~~~~~~~~~~~~~~~
        hover.SetFloat(ShaderUtilities.ID_GlowPower, 0.3f);

        // ���õ��� ���� �ؽ�Ʈ���� ���������� ���ƿ��� �Ѵ�
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
