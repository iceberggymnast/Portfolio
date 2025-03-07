using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class Playeranimation : MonoBehaviour
{
    PlayerMove playermove;
    PlayerManager playerManager;
    Animator controll;
    GameObject playerModel;
    GameObject player;
    GameObject playerhitboxSword;
    GameObject playerhurtboxShield;
    GameObject playerhurtbox;
    public GameObject healingparticle;
    public ParticleSystem aura;


    public float forwardSpeed = 0;
    public float rightSpeed = 0;
    public float upSpeed = 0;
    public bool inputLock = false;
    public bool cantClick = false;
    public bool hitbox = false;
    public bool hitboxShield = false;
    public bool hurtbox = true;
    public bool guard = false;
    public float speedOffset = 1;

    float mouse0time;
    float mouse1time;
    bool mouse0timebool;
    bool mouse1timebool;

    bool rollstamina;
    float rolltimer;

    float hpuptimer;
    bool hppostionuse;

    public List<AudioSource> audioSources = new List<AudioSource>();

    public void ATK1 ()
    {
        audioSources[1].Play();
    }

    public void HURT1()
    {
        audioSources[2].Play();
    }

    void Start()
    {
        playerModel = GameObject.Find("test2");
        player = GameObject.Find("PlayerHunter");
        playerhitboxSword = GameObject.Find("PlayerHitBoxSword");
        playerhurtboxShield = GameObject.Find("PlayerHitBoxShield");
        playerhurtbox = GameObject.Find("PlayerHurtBox");

        controll = playerModel.GetComponent<Animator>();
        playermove = player.GetComponent<PlayerMove>();
        playerManager = player.GetComponent<PlayerManager>();

    }

    void Update()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool click0 = Input.GetKeyDown(KeyCode.Mouse0);
        bool click1 = Input.GetKeyDown(KeyCode.Mouse1);
        bool vbutton = Input.GetKey(KeyCode.LeftControl);
        Groundraycast(); // 바닥 확인용 
        wpgagevfx();

        // 애니메이터 키프레임에 따라 조절되는 수치들
        playermove.outputLock = inputLock;
        playermove.Playerforward(forwardSpeed);
        playermove.Playerright(rightSpeed);
        playermove.Playerup(upSpeed);
        playermove.speedOffset = speedOffset;

        // 마우스 클릭할때 조건이 맞으면 트리거 ON
        if (cantClick == false) 
        {   
            // 왼쪽 클릭, 오른쪽 클릭, 왼오 동시클릭 check
            if (click0)
            {
                mouse0time = Time.time;
                mouse0timebool = true;
                if (MathF.Abs(mouse0time - mouse1time) < 0.1f)
                {
                    controll.SetTrigger("mouseboth");
                    mouse0timebool = false;
                    mouse1timebool = false;
                }
                
            }
            if (MathF.Abs(mouse0time - Time.time) > 0.1f && mouse0timebool)
            { controll.SetTrigger("mouse0"); mouse0timebool = false; }

            if (click1)
            {
                mouse1time = Time.time;
                mouse1timebool = true;
                if (MathF.Abs(mouse0time - mouse1time) < 0.1f)
                {
                    controll.SetTrigger("mouseboth");
                    mouse0timebool = false;
                    mouse1timebool = false;
                }
            }
            if (MathF.Abs(mouse1time - Time.time) > 0.1f && mouse1timebool)
            {
                // 백스텝을 위해 뒤로 키 누르고 있는지 확인 + 해당 입력이 들어갈 수 있는 모션인지 확인을 해야함.
                
                if (Input.GetKey(KeyCode.S))
                {
                    if (controll.GetCurrentAnimatorStateInfo(0).IsTag("mouse1andS"))
                    { controll.SetTrigger("mouse1andS"); }
                }
                // 방패공격을 위해 w 키 누르고 있는지 확인
                else if (Input.GetKey(KeyCode.W))
                {
                    if (controll.GetCurrentAnimatorStateInfo(0).IsTag("mouse1andW"))
                    {
                        controll.SetTrigger("mouse1andW");
                    }
                }
                else // 둘다 안누르고 있으면 오른쪽 키로 반환.
                {
                    controll.SetTrigger("mouse1");
                }
                mouse1timebool = false; 
            }

        }
        
        // 걷기 여부 판단
        if (h != 0 || v != 0) // 버튼을 누르고 있는가?
        {   // inputLock이 걸려 있지 않나?
            if (inputLock == false)
            {
                controll.SetBool("isWalk", true);
            }
            else { controll.SetBool("isWalk", false); }
        }
        else
        {
            controll.SetBool("isWalk", false);
        }

        // 오른쪽 버튼을 누르고 있는가?
        if (Input.GetKey(KeyCode.Mouse1))
        {
            controll.SetBool("ismouse1", true);
        }
        else
        {
            controll.SetBool("ismouse1", false);
        }

        // 뛰기 여부 판단
        if (Input.GetKey(KeyCode.LeftShift))
        {
            controll.SetBool("isPutshift", true);
            controll.SetBool("isSit", false); // 뛰면 무조건 일어섬

        }
        else
        {
            controll.SetBool("isPutshift", false);
        }

        // 앉기 및 회피를 위한 스페이스바 조작 여부 확인
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //controll.SetBool("isEscape", true);

            if (h == 0 && v == 0) // 방향키를 조작하고 있지 않다면
            {
                // 무기 꺼냈는지 확인
                if (!controll.GetBool("isWpUse"))
                {
                    if (controll.GetBool("isSit")) // isSit의 온오프 확인후 처리
                    {
                        controll.SetBool("isSit", false);
                    }
                    else
                    {
                        controll.SetBool("isSit", true);
                    }
                }
                else // 회피 작동
                {
                    if (playerManager.StaminaManager(30))
                    {
                        controll.SetTrigger("isEscape");
                        controll.SetBool("isSit", false);
                    }
                }
            }
            else // 방향키를 조작하고 있을때 회피 ON
            {
                if (playerManager.StaminaManager(30))
                {
                    controll.SetTrigger("isEscape");
                    controll.SetBool("isSit", false);
                }
            }
        }

        // 회피가 발동이 되면 스테미나가 소모됨
        if (controll.GetCurrentAnimatorStateInfo(0).IsTag("Roll") && controll.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.3f)
        {
            
            // bool이 false면 스테미나를 소모하고 ture로
            if (!rollstamina)
            {
                playerManager.mp -= 30;
                rollstamina = true;
            }
            else
            {            
                // bool 이 true면 타이머를 작동시킴
                // 구르기 애니메이션이 끝났을때 bool 스위치를 킴
                if (rolltimer < 0.2f)
                {
                    rolltimer += Time.deltaTime;
                }
                else
                {
                    rollstamina = false;
                    rolltimer = 0;
                }
            }

        }

        // 히트박스 판정 트리거
        if (hitbox) // 무기 히트박스 관련
        {
            playerhitboxSword.SetActive(true);
        }
        else
        {
            playerhitboxSword.SetActive(false);
            controll.SetBool("HitCheck", false);
        }

        if (hitboxShield) // 방패 히트박스 관련
        {
            playerhurtboxShield.SetActive(true);
        }
        else
        {
            playerhurtboxShield.SetActive(false);
        }

        if (hurtbox) // 무적일때
        {
            playerhurtbox.SetActive(true);
        }
        else
        {
            playerhurtbox.SetActive(false);
        }

        // V 버튼 입력
        if (vbutton)
        {
            controll.SetBool("Vbutton", true);
        }
        else
        {
            controll.SetBool("Vbutton", false);
        }


        // 포션 사용 애니메이션 
        // E 키 입력을 받는다.
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 피가 깎여 있는지 확인
            if (playerManager.hp < 200)
            {
                // 애니메이션이 아이템 사용 가능한 모션에 있는지 확인한다.
                // 포션 갯수 함수를 사용하여 포션이 있는지 확인한다.
                if (playerManager.healingpostion())
                {
                    // 무기가 수납 중인지 확인한다
                    if (controll.GetBool("isWpUse"))
                    {
                        // 수납 중이지 않으면 무기를 집어 넣는다.
                        controll.SetBool("isWpUse", false);
                    }
                    if (!controll.GetCurrentAnimatorStateInfo(0).IsTag("potionusing"))
                    {
                        // 포션을 먹는 애니메이션으로 진입하기 위한 트리거 작동
                        controll.SetTrigger("Potionuse");
                    }
                }
            }
        }
        // 모션이 출력 될 동안 체력이 서서히 참
        if (controll.GetCurrentAnimatorStateInfo(0).IsTag("potionusing")) // 애니메이션이 출력 중인지
        {
            if (controll.IsInTransition(0)) // 애니메이션이 끝나고 다른 애니메이션으로 갈때
            {
                hppostionuse = false; // 소모 bool false
            }
            else
            {
                if (!hppostionuse)
                {
                    // 포션을 1개 소모함
                    playerManager.healingpostionuse();
                    hppostionuse = true;
                }
                    
                
            }

            hpuptimer += Time.deltaTime;
            if (hpuptimer >= 0.1f)
            {
                playerManager.hp++;
                hpuptimer = 0;
                // 힐링 파티클
                healingparticle.GetComponent<ParticleSystem>().Play();
            }
        }

    }
    
    // 바닥을 판단해서 파라미터를 반환
    public void Groundraycast()
    {
        if(Physics.Raycast(playerModel.transform.position, - Vector3.up, 1))
        {
            controll.SetBool("CheckGround", true);
        }
        else
        {
            controll.SetBool("CheckGround", false);
        }
    }
    
    // 백스텝, 기모으기 할때 이펙트
    public void wpgagevfx()
    {
        if (controll.GetCurrentAnimatorStateInfo(0).IsTag("aura"))
        {


            if (controll.IsInTransition(0))
            {
                aura.Stop();
            }
            else
            {
                if (controll.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.3f)
                {
                    aura.Play();

                    if (controll.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f)
                    {
                        controll.SetBool("Charge", true);
                    }
                }
                else
                {
                        controll.SetBool("Charge", false);
                }

            }
        }
    }

    public void Resetmove()
    {
        PlayerManager.pm.Restartmove();
    }


}
