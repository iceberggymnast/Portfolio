using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 

public class PlayerFire : MonoBehaviour
{
    [Header("총")]
    public GameObject bulletFXObject;
    public float playerHaveBullet; // 소지하고 있는 총알 아이템의 수
    public float maxBullet = 30; // 탄창
    public float currentBullet; // 현재 총알의 갯수
    public float playerBulletDamage; // 총알 데미지
    public bool isHit;
    
    [Header("에임")]
    public Image redCrossHair;
    public Image whiteCrossHair;
    public float originFov;
    public float maxFov;
    public float zoomSpeed = 200.0f;
    public bool isAiming;
    public bool isZoom;

    FollowCamera followCamera;
    EnemyAi enemyAi;
    Animator playerAnim;
    ParticleSystem bulletEffect;
    
    
    public TMP_Text[] bulletText = new TMP_Text[3];


    void Start()
    {
        followCamera = Camera.main.gameObject.GetComponent<FollowCamera>();
        playerAnim = Camera.main.gameObject.GetComponentInChildren<Animator>();

        bulletFXObject = GameObject.Find("Stone_BulletImpact");
        bulletEffect = bulletFXObject.GetComponent<ParticleSystem>();

        Cursor.lockState = CursorLockMode.Locked;

        // 총알 갯수 초기화
        currentBullet = maxBullet;
        playerHaveBullet = 0;

        // origin 시야각은 현재 카메라 시야각이다.
        originFov = Camera.main.fieldOfView;

    }

    void Update()
    {
        // 마우스 오른쪽 버튼을 클릭시, 조준한다.
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = !isAiming;
        }

        // 마우스 버튼 왼쪽 클릭시, 사격한다.
        if (Input.GetMouseButtonDown(0))
        {
            Shooting();
            playerAnim.SetTrigger("isFire");
            
        }

        // 만일, 키보드 R을 입력시 총알을 재장전한다.
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
            
        }

        bulletText[0].text = currentBullet.ToString();
        bulletText[2].text = playerHaveBullet.ToString();

        if(isAiming)
        {
            playerAnim.SetBool("isAiming", true);
            whiteCrossHair.enabled = false;
            redCrossHair.enabled = true;
            isZoom = true;

        }
        else
        {
            playerAnim.SetBool("isAiming", false);
            redCrossHair.enabled = false;
            whiteCrossHair.enabled = true;
            isZoom = false;
        }

        if(isZoom)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, maxFov, Time.deltaTime * zoomSpeed);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, originFov, Time.deltaTime * zoomSpeed);
        }
    }

    // 총
    void Shooting()
    {
        // 만일, 총알이 있다면...
        if (currentBullet > 0)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hitInfo;

            isHit = Physics.Raycast(ray, out hitInfo, 1000, ~((1<<8) | (1<<2)));

            if (isHit)
            {
                // 만약, hit한 오브젝트가 적이라면
                if (hitInfo.collider.name.Contains("Enemy"))
                {
                    // 적에게 데미지를 입힌다.
                    hitInfo.collider.gameObject.GetComponent<EnemyAi>().enemyHp -= playerBulletDamage;
                    print("적에게 데미지를 입혔습니다!");
                }

                bulletFXObject.transform.position = hitInfo.point;
                bulletEffect.Play();

                //print("Hit" + hitInfo.collider.name);
            }

            currentBullet--;
        }
        // 현재 총알이 없다면...
        else
        {
            // 자동 장전
            Reload();
        }
    }

    void Reload()
    {
        isAiming = false;

        if(playerHaveBullet > 0)
        {
            // 장전 애니메이션 처리
            if (currentBullet > 0)
            {
                playerAnim.SetTrigger("isUseLessReload");
            }
            else if (currentBullet <= 0)
            {
                playerAnim.SetTrigger("isReload");
            }

            // 장전 코드로 처리
            if (currentBullet <= 30)
            {
                // 소유하고 있는 총알이 30발보다 많다면...
                if (playerHaveBullet + currentBullet >= maxBullet)
                {
                    playerHaveBullet += currentBullet;
                    currentBullet = 0;
                    currentBullet = maxBullet;
                    playerHaveBullet -= currentBullet;
                }
                // 소유하고 있는 총알이 30발보다 적다면...
                else if (playerHaveBullet + currentBullet < maxBullet)
                {
                    playerHaveBullet += currentBullet;
                    currentBullet = 0;
                    currentBullet = playerHaveBullet;
                    playerHaveBullet -= currentBullet;
                }
            }
        }
        
        bulletText[2].text = playerHaveBullet.ToString();
        
    }

}