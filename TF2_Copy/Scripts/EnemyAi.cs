using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyAi;
using Random = UnityEngine.Random;

public class EnemyAi : MonoBehaviour
{
    // ���¸� �����Ѵ�
    public enum EnemyState
    {
        IdleStop, // ������ ���ִ� Ai -> �߰��ϸ� Covering
        IdlePatrol, // �����ϴ� Ai -> �߰��ϸ� GoingAttack
        IdleStopPatrol, // ����Ʈ ���� -> ���� ����Ʈ�� �����ϸ� ��� ����ϴ� �뵵 (���� 30�� �ѷ���) -> IdlePatrol
        Reload, // ������ -> ���� State��
        MissingTaget, // ���ʵ��� �÷��̾ �������� ������ -> ���� �� �ѹ��� �� -> ���� State��
        StandAttack, // ������ ���� ����
        GoingAttack, // �ɾ�鼭 ����
        CoveringFind, // ������ ���� ã�´�
        CoveringGo,
        CoveringFire, // ������ ��Ȳ�� ���� ����
        GrenadeShoot,
        GrenadeAvoid,
        BypassAttack, // ��ȸ ����
        Dead
    }
    public EnemyState enemyState = EnemyState.IdleStop;

    // �ڽ��� �ڳ� ������Ʈ��
    Animator enemyanim;
    GameObject enemyWeapon;
    GameObject enemyRagdoll;

    // Player ������Ʈ 
    public GameObject player;

    // �þ� ���� ���� ����
    EnemyEyesightCollisionStay EnemyEyesightCollisionStay; // �ڳ� ������Ʈ ù��°���� ��
    public Vector3 playerFootPrintPos; // �÷��̾ �����Ǹ� ��ġ���� ����
    public bool engage;
    public bool seePlayer;

    // Ai�� Player ���� ���� ����
    NavMeshAgent agent;

    // ���� Stat ���� ����
    public float enemyHp = 100.0f;
    public int magazineMax = 30;
    public int currentMagazine = 30;

    // Idle ���¿����� ����
    public List<GameObject> patrol = new List<GameObject>();
    int patrolcountmax;
    int currnetpatrolcount;

    // ���� ��� ���� ����
    public BulletFireScripts bulletFireScripts;
    public float firePer; // ��� ����
    int currnetburst;
    public int burstnumbermax; // ���� Ƚ��
    public float firewait; // ����� ��ٸ��� ����
    float firewaittime;
    float fireCrurrtTime;
    bool cantFire;
    public bool detected;
    int detectedAction; // 0. ���ڸ����� ��� 1. ������ 2. �޷����鼭 ��� 3. ����ź ��ȸ
    float relordTimer;
    public float relodTimeSet = 2.0f;
    int beforestate; // ���� ���°��� ����ϱ� ���� �뵵
    Transform beforeTransform;

    // �ִϸ��̼� ��ũ��Ʈ
    EnemyAnimationControll enemyAnimationControll;
    bool reload;

    void Start()
    {
        // ����ִ� ������Ʈ�� ä���
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

        // ���� ���� ã�� �ڵ� (1ȸ ����)
        if (!isFindPoint)
        {
            // Enemy�� �������� Ư�� ���� ���� Collider���� �����ͼ� ����Ʈ�� �����Ѵ�.
            Collider[] coverObj = Physics.OverlapSphere(transform.position, 100.0f);
            // ������ Collider�� 11�� ���̾ ���� �ִ� ������Ʈ���� �����´�.
            for (int i = 0; i < coverObj.Length; i++)
            {
                if (coverObj[i].gameObject.layer == 13)
                {
                    coverPoint.Add(coverObj[i].gameObject);
                }
            }
            isFindPoint = true;
        }

        // �ش� ������Ʈ�� positon�� �ش� ��ǥ���� ���� �������� Ȯ���Ѵ�. (��ֹ��� ������ hit ������ �Ÿ��� ������ �ǳ�?)
        if (coverPoint.Count > 0)
        {
            int ri = Random.Range(0, coverPoint.Count - 1);
            Vector3 dir = player.transform.position - coverPoint[ri].transform.position;
            RaycastHit hit;
            if (Physics.Raycast(coverPoint[ri].transform.position, dir, out hit))
            {
                Vector3 range = hit.collider.gameObject.transform.position - coverPoint[ri].gameObject.transform.position;
                // ���� ������ �����̸� Cover�Ѵ�.
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
        // ������ ���� ���ٸ� �÷��̾��� �ݴ� �������� ���� �Ѵ��� �ٽ� ã�´�.
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
        // �÷��̾� ������ õõ�� �����Ѵ�.
        //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, beforeTransform.eulerAngles + new Vector3(0, 180, 0), Time.deltaTime);
        Looktarget(player.transform.position);
        agent.SetDestination((transform.forward * 5.0f) + transform.position);
        // ã������ ���� ���·�
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
        { //��ȸ ��Ʈ�� ����
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

    // Idle������ stop�ϰ� �ִ� ����
    void Idlestate(int idleType)
    {
        // idleType�� 0�̸� ������ �ִ´�.
        if (idleType == 0)
        {
            agent.SetDestination(transform.position);
        }
        // idleType�� 1�̸� ������ �����Ѵ�.
        else if (idleType == 1)
        {
            // ����Ʈ�� ������ �� ���ӿ�����Ʈ�� ��� ������ Ȯ���Ѵ�.
            patrolcountmax = patrol.Count;
            // ����Ʈ�� patrolcountmax�� �̵��Ѵ�.
            agent.SetDestination(patrol[currnetpatrolcount].transform.position);
            Vector3 direction = patrol[currnetpatrolcount].transform.position - transform.position;
            // �������� ���� �ߴ��� Ȯ���ϰ� ���� ����Ʈ�� ���ӿ�����Ʈ���� �̵��Ѵ�.
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


    // OnTriggerStay�� �������� ����ȭ �����ش�.
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

    // ���� ����� �����϶� ���
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

        // �ùٸ� �ӵ� ���
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

    // �߻�� ȣ���ϴ� �Լ�
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

            // player ü�� ��ȭ Ž��
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
    // �� ü�� ���� check
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
