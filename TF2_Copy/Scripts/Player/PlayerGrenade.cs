using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGrenade : MonoBehaviour
{
    [Header("수류탄")]
    public GameObject grenadePrefab;
    public float throwPower = 5.0f;
    public float simulationTime = 5.0f;
    public float interval = 0.1f;
    public float mass = 5.0f;
    public bool isLine = false;

    public GameObject gun;
    public GameObject bullet;

    List<Vector3> trajectory = new List<Vector3>();

    LineRenderer line;
    Animator playerAnim;

    

    void Start()
    {
        line = GetComponent<LineRenderer>();
        playerAnim = Camera.main.gameObject.GetComponentInChildren<Animator>();

    }

    void Update()
    {
        GrenadeType();

        Line();

        AnimationCheck();
    }

    // 수류탄
    void GrenadeType()
    {
        // 만일, 키보드 Q 입력을 누르고 있다면...
        if(Input.GetKey(KeyCode.Q))
        {
            // 수류탄이 날아가는 궤적을 그린다.
            // p = p0 + vt - 0.5 * g * t * t;

            // 시작점
            Vector3 startPos = transform.position;

            // 방향
            Vector3 dir = (Camera.main.transform.forward + Camera.main.transform.up) + Camera.main.transform.forward;
            dir.Normalize();

            // gravity 변수
            Vector3 gravity = Physics.gravity;

            int simulCount = (int)(simulationTime / interval);

            trajectory.Clear();
            for(int i = 0; i < simulCount; i++)
            {
                float currentTime = interval * i;

                Vector3 result = startPos + dir * throwPower * currentTime + 0.5f * gravity * currentTime * currentTime * Mathf.Pow(mass, 2);
                
                if(trajectory.Count > 0)
                {
                    // 충돌 확인
                    Vector3 rayDir = result - trajectory[trajectory.Count - 1];
                    Ray ray = new Ray(trajectory[trajectory.Count - 1], rayDir.normalized);
                    RaycastHit hitInfo;

                    // 만일, 부딪힌 대상이 있다면...
                    if (Physics.Raycast(ray, out hitInfo, rayDir.magnitude))
                    {
                        trajectory.Add(hitInfo.point);
                        break;
                    }
                    else
                    {
                        trajectory.Add(result);
                    }
                }
                else
                {
                    trajectory.Add(result);
                }

                isLine = true;

            }

            
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            playerAnim.SetTrigger("isGrenadeOn");
        }

        // 만일, 키보드 Q 입력을 눌렀다가 떼면...
        if (Input.GetKeyUp(KeyCode.Q))
        {
            // 수류탄 프리팹을 생성하고, 물리적으로 발사한다.
            GameObject bomb = Instantiate(grenadePrefab, transform.position, transform.rotation);

            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.mass = mass;

                Vector3 dir = (Camera.main.transform.forward + Camera.main.transform.up) + Camera.main.transform.forward;
                dir.Normalize();

                rb.AddForce(dir * throwPower, ForceMode.Impulse);
            }

            isLine = false;
            line.enabled = false;

            playerAnim.SetTrigger("isGrenadeOff");

        }
        
    }

    private void OnDrawGizmos()
    {
        if(trajectory.Count < 1)
        {
            return;
        }

        Gizmos.color = Color.green;

        for(int i = 0; i < trajectory.Count - 1; i++)
        {
            Gizmos.DrawLine(trajectory[i], trajectory[i + 1]);
        }
    }

    void Line()
    {
        if(isLine)
        {
            line.enabled = true;
            line.positionCount = trajectory.Count;
            line.SetPositions(trajectory.ToArray());
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            line.startColor = Color.white;
            line.endColor = Color.white;

        }
    }

    void AnimationCheck()
    {
        if(playerAnim.GetCurrentAnimatorStateInfo(0).IsName("GrenadePin") || playerAnim.GetCurrentAnimatorStateInfo(0).IsName("GrenadeThrow"))
        {
            gun.SetActive(false);
            bullet.SetActive(false);

        }
        else
        {
            gun.SetActive(true);
            bullet.SetActive(true);
        }
    }
}
