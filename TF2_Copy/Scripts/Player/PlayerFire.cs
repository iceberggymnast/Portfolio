using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 

public class PlayerFire : MonoBehaviour
{
    [Header("��")]
    public GameObject bulletFXObject;
    public float playerHaveBullet; // �����ϰ� �ִ� �Ѿ� �������� ��
    public float maxBullet = 30; // źâ
    public float currentBullet; // ���� �Ѿ��� ����
    public float playerBulletDamage; // �Ѿ� ������
    public bool isHit;
    
    [Header("����")]
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

        // �Ѿ� ���� �ʱ�ȭ
        currentBullet = maxBullet;
        playerHaveBullet = 0;

        // origin �þ߰��� ���� ī�޶� �þ߰��̴�.
        originFov = Camera.main.fieldOfView;

    }

    void Update()
    {
        // ���콺 ������ ��ư�� Ŭ����, �����Ѵ�.
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = !isAiming;
        }

        // ���콺 ��ư ���� Ŭ����, ����Ѵ�.
        if (Input.GetMouseButtonDown(0))
        {
            Shooting();
            playerAnim.SetTrigger("isFire");
            
        }

        // ����, Ű���� R�� �Է½� �Ѿ��� �������Ѵ�.
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

    // ��
    void Shooting()
    {
        // ����, �Ѿ��� �ִٸ�...
        if (currentBullet > 0)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hitInfo;

            isHit = Physics.Raycast(ray, out hitInfo, 1000, ~((1<<8) | (1<<2)));

            if (isHit)
            {
                // ����, hit�� ������Ʈ�� ���̶��
                if (hitInfo.collider.name.Contains("Enemy"))
                {
                    // ������ �������� ������.
                    hitInfo.collider.gameObject.GetComponent<EnemyAi>().enemyHp -= playerBulletDamage;
                    print("������ �������� �������ϴ�!");
                }

                bulletFXObject.transform.position = hitInfo.point;
                bulletEffect.Play();

                //print("Hit" + hitInfo.collider.name);
            }

            currentBullet--;
        }
        // ���� �Ѿ��� ���ٸ�...
        else
        {
            // �ڵ� ����
            Reload();
        }
    }

    void Reload()
    {
        isAiming = false;

        if(playerHaveBullet > 0)
        {
            // ���� �ִϸ��̼� ó��
            if (currentBullet > 0)
            {
                playerAnim.SetTrigger("isUseLessReload");
            }
            else if (currentBullet <= 0)
            {
                playerAnim.SetTrigger("isReload");
            }

            // ���� �ڵ�� ó��
            if (currentBullet <= 30)
            {
                // �����ϰ� �ִ� �Ѿ��� 30�ߺ��� ���ٸ�...
                if (playerHaveBullet + currentBullet >= maxBullet)
                {
                    playerHaveBullet += currentBullet;
                    currentBullet = 0;
                    currentBullet = maxBullet;
                    playerHaveBullet -= currentBullet;
                }
                // �����ϰ� �ִ� �Ѿ��� 30�ߺ��� ���ٸ�...
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