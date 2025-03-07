using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class GrenadeScript : MonoBehaviour
{
    public float explosionTime;
    float currentTime = 0;
    public float damege;
    public float radius;
    public ParticleSystem particle;
    public AudioSource audioSource;
    public GameObject grenadModel;
    bool particleTick;
    public bool explosion;
    public bool playerFind;

    List<Collider> objList = new List<Collider>();

    // �����Ǹ� Ÿ�̸Ӱ� ���ư���.
    // Ÿ�̸Ӱ� ������ Ư�� �� �ȿ� �ִ� ��ü���� ���̳� player�� �ִ��� üũ
    // ������ �߽ɺηκ��� �ش� ������� ����ĳ��Ʈ�� ����� �߰��� ��ֹ��� �ִ��� �Ǵ�
    // ��ֹ��� ���ٸ� ������ ����

    void Update()
    {
        currentTime += Time.deltaTime;
        
        if (currentTime > explosionTime)
        {
            Collider[] collidersArray = Physics.OverlapSphere(transform.position, radius);
            objList.AddRange(collidersArray);

            if (!explosion)
            {
                for (int i = 0; i < objList.Count; i++)
                {
                    if (objList[i].gameObject.layer == 8 || objList[i].gameObject.layer == 9)
                    {
                        Vector3 dir = objList[i].gameObject.transform.position - transform.position;
                        RaycastHit hit;
                        Physics.Raycast(transform.position, dir, out hit);
                        if (hit.collider.gameObject == objList[i].gameObject)
                        {
                            float offset = dir.magnitude;
                            float damegeresult = damege * (1 - offset / radius);
                            if (objList[i].gameObject.GetComponent<PlayerState>() != null)
                            {
                                objList[i].gameObject.GetComponent<PlayerState>().playerHP -= damegeresult;
                                print("player�� �������� �Ծ���." + damegeresult);
                            }
                            else if (objList[i].gameObject.GetComponent<EnemyAi>() != null)
                            {
                                objList[i].gameObject.GetComponent<EnemyAi>().enemyHp -= damegeresult;
                                print("Enemy�� �������� �Ծ���." + damegeresult);
                            }
                        }
                    }
                }
                explosion = true;
            }

            if (!particleTick)
            {
                particle.transform.forward = Vector3.up;
                particle.Play();
                audioSource.Play();
                particleTick = true;
            }

            grenadModel.SetActive(false);

            if (currentTime > explosionTime * 2)
            {
                Destroy(gameObject);
            }

        }

        
    }
}
