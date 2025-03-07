using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public float maxHP = 100; 
    public float playerHP; // 현재 플레이어 HP
    public float playerBeforeHP; // 플레이어 바로 전 HP 
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
            //print("피가 얼마 없습니다.");
            //img_BloodScreen.enabled = true;
            
        }
        else if(playerHP <= 0)
        {
            print("죽음");
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

        
        // 현재 체력이 maxHP 보다 낮다면,
        if(playerHP > 0 && playerHP < maxHP)
        {
            if (currentTime > generateTime)
            {
                // 현재 체력은 maxHP 까지 회복한다.
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
        // 회복 중
        if(playerBeforeHP < playerHP)
        {
            playerBeforeHP = playerHP;
        }
        // 적한테 맞은 경우
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
