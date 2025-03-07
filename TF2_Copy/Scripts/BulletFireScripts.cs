using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class BulletFireScripts : MonoBehaviour
{
    [Header("적이 쏘는 총이라면 체크")]
    public bool isEnemyshoot;
    [Header("총알의 데미지")]
    public float bulletDamege;
    [Header("총알이 빗나갈 정도")]
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

        // 누가 발사하는건지 확인한뒤에
        // 레이어 값 적용
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

        // 명중률 계산
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
            // 플레이어의 체력을 매번 체크해서 명중률을 조절함 
            bulletPrecisionOffset = 1 - (playerState.playerHP / 100);
        }
        else if(isEnemyshoot && pelletNumber > 1)
        {
            bulletPrecisionOffset = 3 - (playerState.playerHP / 100);
        }
    }

    public PlayerSlide playerslide;

    // playerPos를 입력받으면 타겟을 기준으로 명중률 계산
    public void Fire(Vector3 playerPos)
    {
        enemyAi.currentMagazine--;
        startShoot++;

        for (int i = 0; i < pelletNumber; i++)
        {
            // 명중률을 계산하고 Raycast를 발사
            float bulletPrecisionX = Random.Range(-bulletPrecisionOffset, bulletPrecisionOffset);
            float bulletPrecisionY = Random.Range(-bulletPrecisionOffset, bulletPrecisionOffset);
            float bulletPrecisionZ = Random.Range(-bulletPrecisionOffset, bulletPrecisionOffset);
            Vector3 bulletdir = (playerPos + new Vector3(bulletPrecisionX, bulletPrecisionY, bulletPrecisionZ)) - transform.position;


            RaycastHit hit;
            if (Physics.Raycast(transform.position, bulletdir, out hit))
            {
                // hit 했다면 플레이어, 또는 적의 체력을 깎음
                if (isEnemyshoot)
                {
                    // hit한게 player가 맞는지 확인
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

    // 매개변수가 없는 Fire는 카메라를 front 방향으로 날아감 총구 기준으로 명중률 계산
    public void Fire()
    {
        // 명중률을 계산하고 Raycast를 발사
        float bulletPrecisionX = Random.Range(-bulletPrecisionOffset, bulletPrecisionOffset);
        float bulletPrecisionY = Random.Range(-bulletPrecisionOffset, bulletPrecisionOffset);
        float bulletPrecisionZ = Random.Range(-bulletPrecisionOffset, bulletPrecisionOffset);
        Vector3 bulletstart = (playerCamera.transform.position + new Vector3(bulletPrecisionX, bulletPrecisionY, bulletPrecisionZ)) - transform.position;

        RaycastHit hit;
        if (Physics.Raycast(bulletstart, playerCamera.transform.forward, out hit))
        {
            // hit 했다면 플레이어, 또는 적의 체력을 깎음
            if (isEnemyshoot)
            {
                // hit한게 player가 맞는지 확인
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
