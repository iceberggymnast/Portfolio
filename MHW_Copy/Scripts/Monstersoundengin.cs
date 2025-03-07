using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monstersoundengin : MonoBehaviour
{
    public List<Camera> cameras = new List<Camera>();
    public List<AudioSource> monstersound = new List<AudioSource>();
    public MonsterState monsterState;
    public Engager engager;

    bool startplay1;
    bool startplay2;
    bool startplay3;

    float time;

    void Start()
    {
        monsterState = GameObject.Find("Monster").GetComponent<MonsterState>();
        engager = GameObject.Find("Monster").GetComponent<Engager>();
        if (GameObject.Find("BoneFunction_004") != null)
        {
            gameObject.transform.parent = GameObject.Find("BoneFunction_004").transform;
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    void Update()
    {
        DeathCam();

        // 포효
        if (engager.isEngage)
        {
            if (!startplay1)
            {
                monstersound[0].Play();
                startplay1 = true;
            }
        }

        // 전투 배경음악
        if (engager.isEncount)
        {
            if (!startplay2)
            {
                monstersound[1].Play();
                startplay2 = true;
            }
        }

        // 클리어 배경음악 출력
        if (monsterState.hp <= 0)
        {
            if (!startplay3)
            {
                monstersound[0].Stop();
                monstersound[2].Play();
                startplay3 = true;
            }
        }
    }

    public void DeathCam()
    {
       if(monsterState.hp <= 0)
        {
            time += Time.deltaTime;
            if (time <= 3.3f)
            {
                cameras[0].enabled = false;
                cameras[1].enabled = true;
            }
            else if(time <= 6.6f)
            {
                cameras[1].enabled = false;
                cameras[2].enabled = true;
            }
            else if(time <= 9.9f)
            {
                cameras[2].enabled = false;
                cameras[3].enabled = true;
            }
            else
            {
                cameras[3].enabled = false;
                cameras[0].enabled = true;
            }
        }
    }

}
