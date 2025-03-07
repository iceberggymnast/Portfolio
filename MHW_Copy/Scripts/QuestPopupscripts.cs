using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestPopupscripts : MonoBehaviour
{
    public List<GameObject> questpopupui= new List<GameObject>(); //UI 리스트


    public GameObject monsterInfo; // hp를 받아올 몬스터
    int hpM;

    public GameObject playerInfo; // hp를 받아올 player
    int hpP;




    public bool incombat; // 전투지역 판단 여부


    float time;
    float monsterdeadtimer;
    float playerdeadtimer;

    void Start()
    {
        if (incombat)
        {
            Uiactive(questpopupui[0]);
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        // 퀘스트가 시작할때 팝업되는 UI
        if (time > 3 && incombat)
        {
            Uideactive(questpopupui[0]);
        }

        // 몬스터가 사망했을때 뜨는 UI
        if (monsterInfo != null)
        {
            hpM = monsterInfo.GetComponent<MonsterState>().hp;
            if (hpM <= 0)
            {
                monsterdeadtimer += Time.deltaTime;

                if (monsterdeadtimer > 15)
                { 
                Uiactive(questpopupui[1]);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                }
                
                if (monsterdeadtimer > 20)
                {
                    PlayerManager.pm.blackon = true;
                }

                if (monsterdeadtimer > 22)
                {
                    monsterdeadtimer = 0;
                    SceneManager.LoadScene(1);
                }
            }
        }

        // 캐릭터가 사망했을때 뜨는 UI
        if (playerInfo != null)
        {
            hpP = playerInfo.GetComponent<PlayerManager>().life;
            if (hpP <= 0)
            {
                playerdeadtimer += Time.deltaTime;
                if (playerdeadtimer > 1)
                {
                    Uiactive(questpopupui[2]);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }

                if (playerdeadtimer > 2)
                {
                    PlayerManager.pm.blackon = true;
                }

                if (playerdeadtimer > 5)
                {
                    playerdeadtimer = 0;
                    SceneManager.LoadScene(1);
                }

            }
        }

    }

    public void Uiactive(GameObject ui)
    {
        ui.SetActive(true);
    }

    public void Uideactive(GameObject ui)
    {
        ui.SetActive(false);
    }
}
