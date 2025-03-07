using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public float maxHP = 100; 
    public float playerHP; // ���� �÷��̾� HP
    public float playerBeforeHP; // �÷��̾� �ٷ� �� HP 
    public float currentTime;
    public float generateTime = 2.0f;
    public bool hit;

    public GameObject player;
    public GameObject regdollPlayer;
    public Image img_BloodScreen;

    FollowCamera followCam;
    Volume volume;

    void Start()
    {
        playerHP = maxHP;

        followCam = Camera.main.gameObject.GetComponent<FollowCamera>();
        volume = GameObject.Find("Global Volume").GetComponent<Volume>();

    }

    
    void Update()
    {
        //print(playerHP);
        currentTime += Time.deltaTime;

        if (playerHP > 0 && playerHP < 30)
        {
            //print("�ǰ� �� �����ϴ�.");
            //img_BloodScreen.enabled = true;
            
        }
        else if(playerHP <= 0)
        {
            print("����");
            ColorAdjustments colorAdjustments;
            volume.profile.TryGet(out colorAdjustments);
            colorAdjustments.active = true;
            player.SetActive(false);
            regdollPlayer.SetActive(true);
            followCam.checkDeathCam(true);

        }
        else
        {
            player.SetActive(true);
            regdollPlayer.SetActive(false);
            img_BloodScreen.enabled = false;
            
        }

        
        // ���� ü���� maxHP ���� ���ٸ�,
        if(playerHP > 0 && playerHP < maxHP)
        {
            if (currentTime > generateTime)
            {
                // ���� ü���� maxHP ���� ȸ���Ѵ�.
                playerHP += 100 * Time.deltaTime * 0.2f;
            }

        }
        else if(playerHP > maxHP)
        {
            playerHP = maxHP;
        }

        HPCheck();
        HealingTimer();

    }

    void HPCheck()
    {
        // ȸ�� ��
        if(playerBeforeHP < playerHP)
        {
            playerBeforeHP = playerHP;
        }
        // ������ ���� ���
        else if(playerBeforeHP > playerHP)
        {
            hit = true;
            playerBeforeHP = playerHP;
        }
    }

    void HealingTimer()
    {
        if(hit)
        {
            currentTime = 0;
            hit = false;
        }
    }
}
