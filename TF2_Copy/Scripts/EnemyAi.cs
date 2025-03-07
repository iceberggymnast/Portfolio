using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyAi;
using Random = UnityEngine.Random;

public class EnemyAi : MonoBehaviour
{
    // 상태를 열거한다
    public enum EnemyState
    {
        IdleStop, // 가만히 서있는 Ai -> 발견하면 Covering
        IdlePatrol, // 순찰하는 Ai -> 발견하면 GoingAttack
        IdleStopPatrol, // 포인트 도착 -> 순찰 포인트에 도착하면 잠시 대기하는 용도 (전방 30도 둘러봄) -> IdlePatrol
        Reload, // 재장전 -> 이전 State로
        MissingTaget, // 몇초동안 플레이어가 감지되지 않으면 -> 전투 중 한바퀴 돎 -> 이전 State로
        StandAttack, // 가만히 서서 공격
        GoingAttack, // 걸어가면서 공격
        CoveringFind, // 엄폐할 곳을 찾는다
        CoveringGo,
        CoveringFire, // 엄폐후 상황에 맞춰 공격
        GrenadeShoot,
        GrenadeAvoid,
        BypassAttack, // 우회 공격
        Dead
    }
    public EnemyState enemyState = EnemyState.IdleStop;

    // 자신의 자녀 오브젝트들
    Animator enemyanim;
    GameObject enemyWeapon;
    GameObject enemyRagdoll;

    // Player 오브젝트 
    public GameObject player;

    // 시야 감지 관련 변수
    EnemyEyesightCollisionStay EnemyEyesightCollisionStay; // 자녀 오브젝트 첫번째여야 함
    public Vector3 playerFootPrintPos; // 플레이어가 감지되면 위치값을 남김
    public bool engage;
    public bool seePlayer;

    // Ai의 Player 추적 관련 변수
    NavMeshAgent agent;

    // 적의 Stat 관련 변수
    public float enemyHp = 100.0f;
    public int magazineMax = 30;
    public int currentMagazine = 30;

    // Idle 상태에서의 관리
    public List<GameObject> patrol = new List<GameObject>();
    int patrolcountmax;
    int currnetpatrolcount;

    // 적의 사격 관련 변수
    public BulletFireScripts bulletFireScripts;
    public float firePer; // 사격 간격
    int currnetburst;
    public int burstnumbermax; // 연사 횟수
    public float firewait; // 점사시 기다리는 간격
    float firewaittime;
    float fireCrurrtTime;
    bool cantFire;
    public bool detected;
    int detectedAction; // 0. 제자리에서 사격 1. 엄폐사격 2. 달려오면서 사격 3. 수류탄 우회
    float relordTimer;
    public float relodTimeSet = 2.0f;
    int beforestate; // 이전 상태값을 기억하기 위한 용도
    Transform beforeTransform;

    // 애니메이션 스크립트
    EnemyAnimationControll enemyAnimationControll;
    bool reload;

    void Start()
    {
        // 비어있는 오브젝트들 채우기
        if (agent == null)
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
        }
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        if (EnemyEyesightCollisionStay == null)
        {
            EnemyEyesightCollisionStay = transform.GetChild(0).GetComponent<EnemyEyesightCollisionStay>();
        }
        if (bulletFireScripts == null)
        {
            bulletFireScripts = transform.GetChild(1).GetComponent<BulletFireScripts>();
        }
        if (enemyanim == null)
        {
            enemyanim = transform.GetChild(2).GetComponent<Animator>();
        }
        if (enemyWeapon == null)
        {
            enemyWeapon = transform.GetChild(3).gameObject;
        }
        if (enemyRagdoll == null)
        {
            enemyRagdoll = transform.GetChild(4).gameObject;
        }
        enemyAnimationControll = GetComponent<EnemyAnimationControll>();
    }

    void Update()
    {
        RaycastSynchronization();
        CantFireProcess();
        if (!(enemyState == EnemyState.BypassAttack))
        {
            GrenadeCheck();
        }
        EnemyHPCheck();

        if (enemyHp <= 0)
        {   
            enemyState = EnemyState.Dead;
        }
        
        switch (enemyState)
        {
            case EnemyState.IdleStop:
                Idlestate(0);
                break;
            case EnemyState.IdlePatrol:
                Idlestate(1);
                break;
            case EnemyState.IdleStopPatrol:
                IdleStopPatrol();
                break;
            case EnemyState.Reload:
                Reload();
                break;
            case EnemyState.MissingTaget:
                MissingTaget();
                break;
            case EnemyState.StandAttack:
                StandAttack();
                break;
            case EnemyState.GoingAttack:
                GoingAttack();
                break;
            case EnemyState.CoveringFind:
                CoveringFind();
                break;
            case EnemyState.CoveringGo:
                CoveringGo();
                break;
            case EnemyState.CoveringFire:
                CoveringFire();
                break;
            case EnemyState.GrenadeShoot:
                GrenadeShoot();
                break;
            case EnemyState.GrenadeAvoid:
                CoveringFind();
                break;
            case EnemyState.BypassAttack:
                BypassAttack();
                break;
            case EnemyState.Dead:
                Dead();
                break;
        }
    }

    List<Vector3> waypoint = new List<Vector3>();
    bool calculateComplete;
    float coverTimer = 0;
    public bool enemyseat;
    void CoveringFire()
    {
        Vector3 distance = player.transform.position - transform.position;
        if (distance.magnitude < 3)
        {
            coverPoint.Clear();
            coverPos = Vector3.zero;
            isFindPoint = false;
            coverTimer = 0;
            enemyState = EnemyState.CoveringFind;
        }

        if (!calculateComplete)
        {
            miss = 0;
            NavMeshPath navMeshPath = new NavMeshPath();
            agent.CalculatePath(player.transform.position, navMeshPath);
            waypoint.Clear();
            waypoint.AddRange(navMeshPath.corners);
            calculateComplete = true;
        }

        coverTimer += +Time.deltaTime;

        if (coverTimer < 4)
        {
            calculateComplete = false;
            agent.stoppingDistance = 0;
            agent.SetDestination(coverPos);
            Looktarget(player.transform.position);
            enemyseat = true;
        }
        else if(coverTimer < 16)
        {
            agent.stoppingDistance = 0;
            agent.SetDestination(waypoint[2]);
            Looktarget(player.transform.position);
            enemyseat = false;
            if (detected && coverTimer > 5)
            {
                aiFire();
            }
        }
        else if (coverTimer > 16)
        {
            coverTimer = 0;
            
            if (miss > 15)
            {
                miss = 0;
                coverPoint.Clear();
                coverPos = Vector3.zero;
                isFindPoint = false;
                enemyState = EnemyState.GrenadeShoot;
            }
        }

    }

    public List<GameObject> coverPoint = new List<GameObject>();
    public Vector3 coverPos;
    bool isFindPoint;
    void CoveringFind()
    {

        // 숨을 곳을 찾는 코드 (1회 실행)
        if (!isFindPoint)
        {
            // Enemy를 기준으로 특정 범위 안의 Collider들을 가져와서 리스트에 저장한다.
            Collider[] coverObj = Physics.OverlapSphere(transform.position, 100.0f);
            // 저장한 Collider중 11번 레이어를 갖고 있는 오브젝트들을 가져온다.
            for (int i = 0; i < coverObj.Length; i++)
            {
                if (coverObj[i].gameObject.layer == 13)
                {
                    coverPoint.Add(coverObj[i].gameObject);
                }
            }
            isFindPoint = true;
        }

        // 해당 오브젝트의 positon가 해당 좌표에서 엄폐가 가능한지 확인한다. (장애물의 지점과 hit 지점의 거리가 가까우면 되나?)
        if (coverPoint.Count > 0)
        {
            int ri = Random.Range(0, coverPoint.Count - 1);
            Vector3 dir = player.transform.position - coverPoint[ri].transform.position;
            RaycastHit hit;
            if (Physics.Raycast(coverPoint[ri].transform.position, dir, out hit))
            {
                Vector3 range = hit.collider.gameObject.transform.position - coverPoint[ri].gameObject.transform.position;
                // 엄폐가 가능한 지점이면 Cover한다.
                if (range.magnitude < 3.0f && dir.magnitude > 5.0f)
                {
                    coverPos = coverPoint[ri].gameObject.transform.position;
                    //break;
                }
                else
                {
                    coverPoint.RemoveAt(ri);
                }
            }
        }
        else
        {
            enemyState = EnemyState.BypassAttack;
        }
        // 가능한 곳이 없다면 플레이어의 반대 방향으로 가게 한다음 다시 찾는다.
        if (!(coverPos == Vector3.zero))
        {
            enemyState = EnemyState.CoveringGo;
        }
    }

    void CoveringGo()
    {
        agent.stoppingDistance = 0;
        agent.SetDestination(coverPos);
        Vector3 distance = player.transform.position - coverPos;
        Vector3 velocity = agent.velocity;
        if (distance.magnitude < 3 && agent.remainingDistance < 1)
        {
            coverPoint.Clear();
            coverPos = Vector3.zero;
            isFindPoint = false;
            enemyState = EnemyState.CoveringFind;
        }
        else if (!agent.pathPending && distance.magnitude > 3 && velocity.magnitude < 0.3f && agent.remainingDistance < 0.3f)
        {
            enemyState = EnemyState.CoveringFire;
        }

    }

    void MissingTaget() 
    {
        agent.stoppingDistance = 0;
        // 플레이어 쪽으로 천천히 접근한다.
        //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, beforeTransform.eulerAngles + new Vector3(0, 180, 0), Time.deltaTime);
        Looktarget(player.transform.position);
        agent.SetDestination((transform.forward * 5.0f) + transform.position);
        // 찾았으면 이전 상태로
        if (detected)
        {
            enemyState = (EnemyState)beforestate;
        }
    }


    float offset;
    bool posset;
    bool bypassRandom;
    Vector3 dir;
    Vector3 bypassEndPos;

    void BypassAttack()
    {
        agent.stoppingDistance = 0;
        if (!posset)
        { //우회 루트를 지정
            dir = transform.position - player.transform.position;
            bypassRandom = Convert.ToBoolean(Random.Range(0, 2));
            posset = true;
        }

        offset += Time.deltaTime;
        if (bypassRandom)
        {
            float drg = Mathf.Lerp(0, 90, offset / 3);
            bypassEndPos = Quaternion.Euler(0, drg, 0) * dir * 1.5f;
        }
        else
        {
            float drg = Mathf.Lerp(0, -90, offset / 3);
            bypassEndPos = Quaternion.Euler(0, drg, 0) * dir * 1.5f;
        }
        
        if (offset/3 < 1.7f)
        { 
            agent.SetDestination(bypassEndPos + playerFootPrintPos); 
        }
        else 
        {
            posset = false;
            enemyState = EnemyState.StandAttack;
        }
    }

    void GoingAttack()
    {
        Looktarget(playerFootPrintPos);
        agent.stoppingDistance = 10;

        
        agent.SetDestination(playerFootPrintPos);
        if (seePlayer)
        {
            aiFire();
            ReloadCheck();
        }
        else
        {
            if(agent.remainingDistance < 12) 
            {
                beforeTransform = transform;
                beforestate = (int)enemyState;
                enemyState = EnemyState.MissingTaget;
            }
        }
        
    }

    bool seatRandom;
    void StandAttack()
    {
        if (!seatRandom)
        { 
            enemyseat = Convert.ToBoolean(Random.Range(0, 2));
            seatRandom = true;
        }
        Looktarget(playerFootPrintPos);
        agent.SetDestination(transform.position);

        if (seePlayer)
        {
            if (miss < 12)
            {
                aiFire();
            }
            else
            {
                miss = 0;
                enemyState = EnemyState.GoingAttack;
            }
        }
        else if (miss > 12)
        {
            beforestate = (int)enemyState;
            beforeTransform = transform;
            miss = 0;
            enemyseat = false;
            seatRandom = false;
            enemyState = EnemyState.GoingAttack;
        }
        else
        {
            aiFire();
        }

        ReloadCheck();
    }

    public GameObject mag;
    public Transform magPos;
    void Reload()
    {
        if (!reload)
        {
            enemyAnimationControll.ReloadAnimation();
            reload = true;
            GameObject go = Instantiate(mag);
            go.transform.position = magPos.transform.position;
        }
        relordTimer += Time.deltaTime;
        if (relordTimer > relodTimeSet)
        {
            currentMagazine = magazineMax;
            relordTimer = 0;
            reload = false;
            enemyState = (EnemyState)beforestate;
        }
    }

    void ReloadCheck()
    {
        if (currentMagazine == 0)
        {
            beforestate = (int)enemyState;
            enemyState = EnemyState.Reload;
        }
    }

    float patrioltimer;
    void IdleStopPatrol()
    {
        patrioltimer += Time.deltaTime;
        if (patrioltimer > 1)
        {
            enemyState = EnemyState.IdlePatrol;
        }
    }

    // Idle상태중 stop하고 있는 상태
    void Idlestate(int idleType)
    {
        // idleType이 0이면 가만히 있는다.
        if (idleType == 0)
        {
            agent.SetDestination(transform.position);
        }
        // idleType이 1이면 주위를 순찰한다.
        else if (idleType == 1)
        {
            // 리스트에 순찰을 할 게임오브젝트가 담긴 갯수를 확인한다.
            patrolcountmax = patrol.Count;
            // 리스트의 patrolcountmax로 이동한다.
            agent.SetDestination(patrol[currnetpatrolcount].transform.position);
            Vector3 direction = patrol[currnetpatrolcount].transform.position - transform.position;
            // 목적지에 도달 했는지 확인하고 다음 리스트의 게임오브젝트으로 이동한다.
            if (!agent.pathPending && agent.remainingDistance < 0.1f)
            {
                currnetpatrolcount ++;
                if (currnetpatrolcount >= patrolcountmax)
                {
                    currnetpatrolcount = 0;
                }
                patrioltimer = 0;
                enemyState = EnemyState.IdleStopPatrol;

            }
        }

        if(engage)
        {
            float loto = Random.Range(0.0f, 1.0f);
            if (loto < 0.3f)
            {
                enemyState = EnemyState.StandAttack;
            }
            else if (loto < 0.6f)
            {
                enemyState = EnemyState.GoingAttack;
            }
            else if (loto < 0.9f)
            {
                enemyState = EnemyState.CoveringFind;
            }
            else if (loto < 1.0f)
            {
                enemyState = EnemyState.GrenadeShoot;
            }

        }
    }


    // OnTriggerStay의 변수들을 동기화 시켜준다.
    void RaycastSynchronization()
    {
        if (!engage)
        {
            engage = EnemyEyesightCollisionStay.engage;
        }

        playerFootPrintPos = EnemyEyesightCollisionStay.playerFootPrintPos;
        detected = EnemyEyesightCollisionStay.detected;
        seePlayer = EnemyEyesightCollisionStay.seePlayer;
    }

    // 총을 못쏘는 상태일때 계산
    void CantFireProcess()
    {
        if (cantFire && currnetburst < burstnumbermax)
        {
            fireCrurrtTime += Time.deltaTime;
            if (fireCrurrtTime >= firePer)
            {
                fireCrurrtTime = 0;
                cantFire = false;
                currnetburst++;
            }
        }
        else if(currnetburst == burstnumbermax)
        {
            fireCrurrtTime += Time.deltaTime;
            if(fireCrurrtTime >= firewait)
            {
                fireCrurrtTime = 0;
                currnetburst = 0;
            }
        }
    }

    public Transform firePos;
    public GameObject grenadePrefab;
    void GrenadeShoot()
    {
        Vector3 velocity = GrenadeBallisticCalculation(10);
        GameObject grenade = Instantiate(grenadePrefab);
        grenade.transform.position = firePos.transform.position;
        grenade.GetComponent<Rigidbody>().velocity = velocity;

        enemyState = EnemyState.BypassAttack;
    }

    Vector3 GrenadeBallisticCalculation(float initialVelocity)
    {
        Vector3 startPos = firePos.position;
        Vector3 targetPos = playerFootPrintPos;
        float angle = Mathf.Deg2Rad * 45;
        float gravity = Mathf.Abs(Physics.gravity.y);

        Vector3 distance = targetPos - startPos;
        float horizontalDistance = new Vector3(distance.x, 0, distance.z).magnitude;
        float verticalDistance = distance.y;

        float vx = initialVelocity * Mathf.Cos(angle);
        float vy = initialVelocity * Mathf.Sin(angle);
        Vector3 direction = new Vector3(distance.x, 0, distance.z).normalized;

        // 올바른 속도 계산
        float time = horizontalDistance / vx;
        vy = (verticalDistance + 0.5f * gravity * time * time) / time;

        Vector3 velocity = (direction * vx) + (Vector3.up * vy);

        return velocity;
    }

    void Looktarget(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        Quaternion lookDir = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, Time.deltaTime);
    }


    public ParticleSystem fireParticle;
    public AudioSource gunFireSound;
    public GameObject bulletshell;
    public Transform bulletShellPos;
    float beforePlayerHP = 100;
    int miss;

    // 발사시 호출하는 함수
    void aiFire()
    {
        if (!cantFire)
        {
            GameObject bulletShell = Instantiate(bulletshell);
            bulletShell.transform.position = bulletShellPos.position;
            bulletShell.transform.rotation = Quaternion.LookRotation(bulletShellPos.forward);
            fireParticle.Play();
            gunFireSound.Play();
            enemyAnimationControll.FireAnimation();
            bulletFireScripts.Fire(playerFootPrintPos);
            cantFire = true;

            // player 체력 변화 탐지
            if (beforePlayerHP == player.GetComponent<PlayerState>().playerHP)
            {
                miss++;
            }
            else
            {
                beforePlayerHP = player.GetComponent<PlayerState>().playerHP;
            }
        }
    }


    Vector3 grenadePos;
    void GrenadeCheck()
    {
        Collider[] check = Physics.OverlapSphere(transform.position, 10);
        for (int i = 0; i < check.Length; i++)
        {
            if (check[i].gameObject.layer == 12 && enemyState != EnemyState.GrenadeAvoid)
            {
                grenadePos = check[i].gameObject.transform.position;
                beforestate = (int)enemyState;
                enemyState = EnemyState.GrenadeAvoid;
                break;
            }
        }
    }

    int grenadisnot;
    void GrenadeAvoid()
    {
        Vector3 avoiddir = grenadePos - transform.position;
        agent.SetDestination(avoiddir.normalized * -10);

        Collider[] check = Physics.OverlapSphere(transform.position, 10);
        for (int i = 0; i < check.Length; i++)
        {
            if (check[i].gameObject.layer != 12)
            {
                grenadisnot++;
            }
        }

        if (check.Length == grenadisnot)
        {
            grenadisnot = 0;
            enemyState = (EnemyState)beforestate;
        }
        else
        {
            grenadisnot = 0;
        }

    }

    float beforeEnemyHP = 100;
    // 적 체력 감소 check
    void EnemyHPCheck()
    {
        if (beforeEnemyHP != enemyHp)
        {
            EnemyEyesightCollisionStay.playerFootPrintPos = player.transform.position;
            beforeEnemyHP = enemyHp;

            if(enemyState == EnemyState.StandAttack)
            {
                enemyState = EnemyState.CoveringFind;
            }

            if(!engage)
            {
                engage = true;
                Collider[] check = Physics.OverlapSphere(transform.position, 10);
                for (int i = 0; i < check.Length; i++)
                {
                    if (check[i].gameObject.layer == 9)
                    {
                        check[i].gameObject.GetComponent<EnemyAi>().engage = true;
                        check[i].gameObject.GetComponent<EnemyAi>().EnemyEyesightCollisionStay.playerFootPrintPos = player.transform.position;
                    }
                }
            }

        }
    }


    void Dead()
    {
        enemyWeapon.SetActive(true);
        enemyWeapon.transform.SetParent(null);
        enemyWeapon.name = "Ammo";
        enemyRagdoll.SetActive(true);
        enemyRagdoll.transform.SetParent(null);
        Destroy(gameObject);
    }

}
