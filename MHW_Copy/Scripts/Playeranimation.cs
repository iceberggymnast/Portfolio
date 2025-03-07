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
        Groundraycast(); // �ٴ� Ȯ�ο� 
        wpgagevfx();

        // �ִϸ����� Ű�����ӿ� ���� �����Ǵ� ��ġ��
        playermove.outputLock = inputLock;
        playermove.Playerforward(forwardSpeed);
        playermove.Playerright(rightSpeed);
        playermove.Playerup(upSpeed);
        playermove.speedOffset = speedOffset;

        // ���콺 Ŭ���Ҷ� ������ ������ Ʈ���� ON
        if (cantClick == false) 
        {   
            // ���� Ŭ��, ������ Ŭ��, �޿� ����Ŭ�� check
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
                // �齺���� ���� �ڷ� Ű ������ �ִ��� Ȯ�� + �ش� �Է��� �� �� �ִ� ������� Ȯ���� �ؾ���.
                
                if (Input.GetKey(KeyCode.S))
                {
                    if (controll.GetCurrentAnimatorStateInfo(0).IsTag("mouse1andS"))
                    { controll.SetTrigger("mouse1andS"); }
                }
                // ���а����� ���� w Ű ������ �ִ��� Ȯ��
                else if (Input.GetKey(KeyCode.W))
                {
                    if (controll.GetCurrentAnimatorStateInfo(0).IsTag("mouse1andW"))
                    {
                        controll.SetTrigger("mouse1andW");
                    }
                }
                else // �Ѵ� �ȴ����� ������ ������ Ű�� ��ȯ.
                {
                    controll.SetTrigger("mouse1");
                }
                mouse1timebool = false; 
            }

        }
        
        // �ȱ� ���� �Ǵ�
        if (h != 0 || v != 0) // ��ư�� ������ �ִ°�?
        {   // inputLock�� �ɷ� ���� �ʳ�?
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

        // ������ ��ư�� ������ �ִ°�?
        if (Input.GetKey(KeyCode.Mouse1))
        {
            controll.SetBool("ismouse1", true);
        }
        else
        {
            controll.SetBool("ismouse1", false);
        }

        // �ٱ� ���� �Ǵ�
        if (Input.GetKey(KeyCode.LeftShift))
        {
            controll.SetBool("isPutshift", true);
            controll.SetBool("isSit", false); // �ٸ� ������ �Ͼ

        }
        else
        {
            controll.SetBool("isPutshift", false);
        }

        // �ɱ� �� ȸ�Ǹ� ���� �����̽��� ���� ���� Ȯ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //controll.SetBool("isEscape", true);

            if (h == 0 && v == 0) // ����Ű�� �����ϰ� ���� �ʴٸ�
            {
                // ���� ���´��� Ȯ��
                if (!controll.GetBool("isWpUse"))
                {
                    if (controll.GetBool("isSit")) // isSit�� �¿��� Ȯ���� ó��
                    {
                        controll.SetBool("isSit", false);
                    }
                    else
                    {
                        controll.SetBool("isSit", true);
                    }
                }
                else // ȸ�� �۵�
                {
                    if (playerManager.StaminaManager(30))
                    {
                        controll.SetTrigger("isEscape");
                        controll.SetBool("isSit", false);
                    }
                }
            }
            else // ����Ű�� �����ϰ� ������ ȸ�� ON
            {
                if (playerManager.StaminaManager(30))
                {
                    controll.SetTrigger("isEscape");
                    controll.SetBool("isSit", false);
                }
            }
        }

        // ȸ�ǰ� �ߵ��� �Ǹ� ���׹̳��� �Ҹ��
        if (controll.GetCurrentAnimatorStateInfo(0).IsTag("Roll") && controll.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.3f)
        {
            
            // bool�� false�� ���׹̳��� �Ҹ��ϰ� ture��
            if (!rollstamina)
            {
                playerManager.mp -= 30;
                rollstamina = true;
            }
            else
            {            
                // bool �� true�� Ÿ�̸Ӹ� �۵���Ŵ
                // ������ �ִϸ��̼��� �������� bool ����ġ�� Ŵ
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

        // ��Ʈ�ڽ� ���� Ʈ����
        if (hitbox) // ���� ��Ʈ�ڽ� ����
        {
            playerhitboxSword.SetActive(true);
        }
        else
        {
            playerhitboxSword.SetActive(false);
            controll.SetBool("HitCheck", false);
        }

        if (hitboxShield) // ���� ��Ʈ�ڽ� ����
        {
            playerhurtboxShield.SetActive(true);
        }
        else
        {
            playerhurtboxShield.SetActive(false);
        }

        if (hurtbox) // �����϶�
        {
            playerhurtbox.SetActive(true);
        }
        else
        {
            playerhurtbox.SetActive(false);
        }

        // V ��ư �Է�
        if (vbutton)
        {
            controll.SetBool("Vbutton", true);
        }
        else
        {
            controll.SetBool("Vbutton", false);
        }


        // ���� ��� �ִϸ��̼� 
        // E Ű �Է��� �޴´�.
        if (Input.GetKeyDown(KeyCode.E))
        {
            // �ǰ� �� �ִ��� Ȯ��
            if (playerManager.hp < 200)
            {
                // �ִϸ��̼��� ������ ��� ������ ��ǿ� �ִ��� Ȯ���Ѵ�.
                // ���� ���� �Լ��� ����Ͽ� ������ �ִ��� Ȯ���Ѵ�.
                if (playerManager.healingpostion())
                {
                    // ���Ⱑ ���� ������ Ȯ���Ѵ�
                    if (controll.GetBool("isWpUse"))
                    {
                        // ���� ������ ������ ���⸦ ���� �ִ´�.
                        controll.SetBool("isWpUse", false);
                    }
                    if (!controll.GetCurrentAnimatorStateInfo(0).IsTag("potionusing"))
                    {
                        // ������ �Դ� �ִϸ��̼����� �����ϱ� ���� Ʈ���� �۵�
                        controll.SetTrigger("Potionuse");
                    }
                }
            }
        }
        // ����� ��� �� ���� ü���� ������ ��
        if (controll.GetCurrentAnimatorStateInfo(0).IsTag("potionusing")) // �ִϸ��̼��� ��� ������
        {
            if (controll.IsInTransition(0)) // �ִϸ��̼��� ������ �ٸ� �ִϸ��̼����� ����
            {
                hppostionuse = false; // �Ҹ� bool false
            }
            else
            {
                if (!hppostionuse)
                {
                    // ������ 1�� �Ҹ���
                    playerManager.healingpostionuse();
                    hppostionuse = true;
                }
                    
                
            }

            hpuptimer += Time.deltaTime;
            if (hpuptimer >= 0.1f)
            {
                playerManager.hp++;
                hpuptimer = 0;
                // ���� ��ƼŬ
                healingparticle.GetComponent<ParticleSystem>().Play();
            }
        }

    }
    
    // �ٴ��� �Ǵ��ؼ� �Ķ���͸� ��ȯ
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
    
    // �齺��, ������� �Ҷ� ����Ʈ
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
