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

    // ���� �þ߸� ����ϴ� �Լ�
    private void OnTriggerStay(Collider other)
    {
        // �þ߰� Collider�� ������ ��� ������Ʈ���� ����.
        // Collider �ȿ� player�� �ִ��� Ȯ���Ѵ�.
        if (other.gameObject.layer == 8)
        {
            detected = true;
            RaycastHit hit;
            Vector3 direction = player.transform.position - enemy.transform.position;
            // Raycast�� ����Ͽ� �÷��̾ �����Ǵ��� Ȯ���Ѵ�.
            if (Physics.Raycast(enemy.transform.position, direction, out hit))
            {
                // �÷��̾ �����Ǹ� Idle ���¿��� battle�� �Ѿ��, PlayerFootprint�� �����.
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

    // �÷��̾ ������ detected�� ����
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            detected = false;
            seePlayer = false;
        }
    }
}
