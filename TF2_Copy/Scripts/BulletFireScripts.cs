using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class BulletFireScripts : MonoBehaviour
{
    [Header("���� ��� ���̶�� üũ")]
    public bool isEnemyshoot;
    [Header("�Ѿ��� ������")]
    public float bulletDamege;
    [Header("�Ѿ��� ������ ����")]
    public float bulletPrecisionOffset;

    EnemyAi enemyAi;
    PlayerState playerState;
    GameObject playerCamera;

    public int pelletNumber;
    public Transform firePos;
    public GameObject bulletTrail;
    public int startShoot;
    public GrenadeDetectionUI grenadeDetection;

    void Start()
    {
        grenadeDetection = GameObject.Find("Canvas_Hit_Grenade").GetComponent<GrenadeDetectionUI>();
        playerslide = GameObject.Find("offset").GetComponent<PlayerSlide>();

        // ���� �߻��ϴ°��� Ȯ���ѵڿ�
        // ���̾� �� ����
        if (isEnemyshoot)
        {
            gameObject.layer = 11;
            enemyAi = transform.parent.GetComponent<EnemyAi>();
            playerState = GameObject.Find("Player").GetComponent<PlayerState>();
        }
        else
        {
            gameObject.layer = 10;
            playerState = GameObject.Find("Player").GetComponent<PlayerState>();
            playerCamera = Camera.main.gameObject;
        }
    }

    void Update()
    {
        transform.position = firePos.position;

        // ���߷� ���
        if (startShoot < 6)
        {
            if (pelletNumber == 1)
            {
                bulletPrecisionOffset = 1;
            }
            else
            {
                bulletPrecisionOffset = 5;
            }
        }
        else if (isEnemyshoot && pelletNumber == 1)
        {
            // �÷��̾��� ü���� �Ź� üũ�ؼ� ���߷��� ������ 
            bulletPrecisionOffset = 1 - (playerState.playerHP / 100);
        }
        else if(isEnemyshoot && pelletNumber > 1)
        {
            bulletPrecisionOffset = 3 - (playerState.playerHP / 100);
        }
    }

    public PlayerSlide playerslide;

    // playerPos�� �Է¹����� Ÿ���� �������� ���߷� ���
    public void Fire(Vector3 playerPos)
    {
        enemyAi.currentMagazine--;
        startShoot++;

        for (int i = 0; i < pelletNumber; i++)
        {
            // ���߷��� ����ϰ� Raycast�� �߻�
            float bulletPrecisionX = Random.Range(-bulletPrecisionOffset, bulletPrecisionOffset);
            float bulletPrecisionY = Random.Range(-bulletPrecisionOffset, bulletPrecisionOffset);
            float bulletPrecisionZ = Random.Range(-bulletPrecisionOffset, bulletPrecisionOffset);
            Vector3 bulletdir = (playerPos + new Vector3(bulletPrecisionX, bulletPrecisionY, bulletPrecisionZ)) - transform.position;


            RaycastHit hit;
            if (Physics.Raycast(transform.position, bulletdir, out hit))
            {
                // hit �ߴٸ� �÷��̾�, �Ǵ� ���� ü���� ����
                if (isEnemyshoot)
                {
                    // hit�Ѱ� player�� �´��� Ȯ��
                    if (hit.collider.gameObject.layer == 8)
                    {
                        playerState.playerHP -= bulletDamege;
                        grenadeDetection.StartCoroutine(grenadeDetection.Hit(enemyAi.gameObject.transform.position, 0));
                        playerslide.hurt = true;
                    }
                }
                else
                {
                    GameObject entity;
                    entity = hit.collider.gameObject;
                    if (entity.gameObject.layer == 9)
                    {
                        entity.GetComponent<EnemyAi>().enemyHp -= bulletDamege;
                        
                    }
                }

                GameObject go = Instantiate(bulletTrail);
                go.SetActive(false);
                go.transform.position = firePos.transform.position;
                go.transform.forward = bulletdir;
                go.SetActive(true);

                //gameObject.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                //if (hit.collider != null)
                //{
                //    gameObject.GetComponent<LineRenderer>().SetPosition(1, hit.point);
                //}
                //else
                //{
                //    gameObject.GetComponent<LineRenderer>().SetPosition(1, transform.position + transform.forward * 10.0f);
                //}
            }
        }
    }

    // �Ű������� ���� Fire�� ī�޶� front �������� ���ư� �ѱ� �������� ���߷� ���
    public void Fire()
    {
        // ���߷��� ����ϰ� Raycast�� �߻�
        float bulletPrecisionX = Random.Range(-bulletPrecisionOffset, bulletPrecisionOffset);
        float bulletPrecisionY = Random.Range(-bulletPrecisionOffset, bulletPrecisionOffset);
        float bulletPrecisionZ = Random.Range(-bulletPrecisionOffset, bulletPrecisionOffset);
        Vector3 bulletstart = (playerCamera.transform.position + new Vector3(bulletPrecisionX, bulletPrecisionY, bulletPrecisionZ)) - transform.position;

        RaycastHit hit;
        if (Physics.Raycast(bulletstart, playerCamera.transform.forward, out hit))
        {
            // hit �ߴٸ� �÷��̾�, �Ǵ� ���� ü���� ����
            if (isEnemyshoot)
            {
                // hit�Ѱ� player�� �´��� Ȯ��
                if (hit.collider.gameObject.layer == 8)
                {
                    playerState.playerHP -= bulletDamege;
                    grenadeDetection.StartCoroutine(grenadeDetection.Hit(enemyAi.gameObject.transform.position, 0));
                    playerslide.hurt = true;
                }
            }
            else
            {
                GameObject entity;
                entity = hit.collider.gameObject;
                if (entity.gameObject.layer == 9)
                {
                    entity.GetComponent<EnemyAi>().enemyHp -= bulletDamege;
                }
            }
        }

        


        gameObject.GetComponent<LineRenderer>().SetPosition(0, transform.position);
        if (hit.collider != null)
        {
            gameObject.GetComponent<LineRenderer>().SetPosition(1, hit.point);
        }
        else
        {
            gameObject.GetComponent<LineRenderer>().SetPosition(1, playerCamera.transform.position + playerCamera.transform.forward * 10.0f);
        }
    }

}
