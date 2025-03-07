using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyEyesightCollisionStay : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public EnemyAi enemyAi;
    public Vector3 playerFootPrintPos;

    public bool engage;
    public bool detected;
    public bool seePlayer;

    private void Start()
    {
        if (enemy == null)
        {
            enemy = transform.parent.gameObject;
        }

        if (enemyAi == null)
        {
            enemyAi = transform.parent.GetComponent<EnemyAi>();
        }

        if (player == null)
        {
            player = GameObject.Find("Player");
        }


    }

    // 적의 시야를 담당하는 함수
    private void OnTriggerStay(Collider other)
    {
        // 시야각 Collider에 들어오는 모든 오브젝트들을 감지.
        // Collider 안에 player가 있는지 확인한다.
        if (other.gameObject.layer == 8)
        {
            detected = true;
            RaycastHit hit;
            Vector3 direction = player.transform.position - enemy.transform.position;
            // Raycast를 사용하여 플레이어가 감지되는지 확인한다.
            if (Physics.Raycast(enemy.transform.position, direction, out hit))
            {
                // 플레이어가 감지되면 Idle 상태에서 battle로 넘어가고, PlayerFootprint를 남긴다.
                if (hit.collider.gameObject.layer == 8)
                {
                    engage = true;
                    seePlayer = true;
                    playerFootPrintPos = player.transform.position;
                }
                else
                {
                    seePlayer = false;
                }
            }

        }
    }

    // 플레이어가 나가면 detected가 꺼짐
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            detected = false;
            seePlayer = false;
        }
    }
}
