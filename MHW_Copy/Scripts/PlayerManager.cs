using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager pm;
    GameObject player;
    GameObject sword;
    GameObject swordUn;
    Animator controll;
    GameObject playerModel;
    public int hp = 200;
    public int mp = 150;
    public int life = 3;
    public GameObject hpbar;
    public GameObject mpbar;
    public Vector3 starpos;

    public Camera maincamera;
    public Camera deadcamera;
    public GameObject restartpos;

    // ���׹̳� ������ ������
    float staminarestoretimer;
    float staminarestoretimer2;
    bool isRestoringStamina;

    public bool blackon;
    public GameObject guardvfx;

    // ������ ��� ���� ������ 
    public int healingpostioncount = 8;

    void Awake()
    {
        pm = this;
        player = GameObject.Find("PlayerHunter");
        sword = GameObject.Find("sword");
        swordUn = GameObject.Find("swordUn");
        playerModel = GameObject.Find("test2");
        controll = playerModel.GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;

        if (maincamera == null)
        {
            maincamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
        
        if (deadcamera == null)
        {
            deadcamera = GameObject.Find("Dead Camera").GetComponent<Camera>();
            deadcamera.enabled = false;
        }
        else
        {
            deadcamera.enabled = false;
        }

        if (hpbar == null)
        {
            GameObject.Find("HP_full");
        }

        if (mpbar == null) 
        {
            GameObject.Find("ST_full");
        }


    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Staminarestore();       
        PlayerDead ();
        Uibarmanager();
        PlayerRespawn();

        // ���� ���� ���θ� ���̰� �ϴ� ����
        if (controll.GetBool("isWpVis")) 
        {
            sword.SetActive(true);
            swordUn.SetActive(false);
        }
        else
        {
            sword.SetActive(false);
            swordUn.SetActive(true);
        }
    }

    public void PlayerHurt (int playerHurtDamage, int hitAnimatedNumbers)
    {
        // ���� ���� Ȯ��
        if (controll.GetBool("Guard"))
        {
            // ���带 �ϰ� ������ ü�� ��� ���׹̳��� �Ҹ���.
            // ���׹̳��� �ִ��� Ȯ���ϰ� ����ϸ� ���׹̳��� �Ҹ�
            if (mp > playerHurtDamage)
            {
                if (guardvfx != null)
                {
                    guardvfx.GetComponent<ParticleSystem>().Play();
                    guardvfx.GetComponent<AudioSource>().Play();
                }
                mp -= playerHurtDamage;
            }
            else
            {
                // ���׹̳��� �����ϹǷ� ü���� ���̰� ��� ���
                controll.SetInteger("hurtnum", -1);
                hp = hp - playerHurtDamage;
            }
        }
        else // ���带 ���ϸ� �ǰ� ��� ���
        {
            // 0. �⺻��, 1. ��ȿ, 2. ���� 3. �°� ������
            controll.SetInteger("hurtnum", hitAnimatedNumbers);
            hp = hp - playerHurtDamage;
        }
    }

    //  �÷��̾� ��� ��� ���
    public void PlayerDead ()
    {
        if(hp < 1)
        {   
            controll.SetInteger("hurtnum", -2);
        }
    }

    public void Restartmove()
    {
        if (restartpos == null)
        {
            restartpos = GameObject.Find("Restartpos");
        }
        gameObject.transform.position = restartpos.transform.position;
    }

    // ������ ���� �ð��ڿ� ���� ��ġ�� �� �� ü���� ȸ�������ش�.
    public void PlayerRespawn()
    {
        if (controll.GetCurrentAnimatorStateInfo(1).IsName("Dead"))
        {
            // ī�޶� ó��
            maincamera.enabled = false;
            deadcamera.enabled = true;
            // ���� ó��
            if (life > 0)
            {
                
                if (controll.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.9f)
                {
                    Restartmove();
                    controll.SetInteger("hurtnum", 0);
                    gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                    hp = 200;
                    life--;
                    maincamera.enabled = true;
                    deadcamera.enabled = false;
                }
                else if (life == 1)
                {
                    life--;
                }
            }
        }
    }

    // ü�¹ٿ� ���׹̳� ���� �ƿ�ǲ�� �������ִ� �Լ�
    public void Uibarmanager()
    {
        Image hpbarimg = hpbar.GetComponent<Image>();
        Image spbarimg = mpbar.GetComponent<Image>();

        // ü�°� ���¹̳� ���� 0~1 ���̷� ��ȯ
        float inputhp = (float)hp / 200.0f;
        float inputsp = (float)mp / 150.0f;

        hpbarimg.fillAmount = inputhp;
        spbarimg.fillAmount = inputsp;
    }

    // ���׹̳��� ����ϴ� ����� �ϸ� ���׹̳��� ��� �ʿ��� ���׹̳����� ���� ���׹̳��� ������ false�� ������.
    public bool StaminaManager (int staminause)
    {
        if (mp > staminause)
        {
            // ������ �ִϸ��̼��� ������ �Ǹ� ���׹̳��� �Ҹ���.
            // mp -= staminause;
            isRestoringStamina = false;
            return true;
        }
        else if (mp <= staminause)
        {
            return false;
        }
        return false;
    }

    // ���׹̳��� �پ��� ������ ȸ���Ѵ�.
    public void Staminarestore ()
    {
        // isRestoringStamina�� false�϶� ȸ�� Ÿ�̸Ӱ� ���ư��� ���� �ð��� ������ ture�� �ٲ��ش�.  
        if (!isRestoringStamina)
        {
            if (staminarestoretimer > 0.8f)
            {
                isRestoringStamina = true;
                staminarestoretimer = 0;
            }
            else
            {
                staminarestoretimer += Time.deltaTime;
            }
        }
        else
        {
            // isRestoringStamina true�϶� ȸ���� �� �� �ִ�.
            if (mp < 150) // 0.0255�ʸ��� ȸ���� �� �ִ�.
            {
                staminarestoretimer2 += Time.deltaTime;
                if (staminarestoretimer2 >= 0.0255)
                {
                    if (!controll.GetBool("Guard"))
                    {
                        mp++;
                    }
                    staminarestoretimer2 = 0;
                }
            }
            else if (mp >= 150)
            {
                mp = 150;
                isRestoringStamina = false;
            }
        }
    }

    // ���� ��� �Լ�
    public bool healingpostion()
    {
        // ������ ������ Ȯ��
        if (healingpostioncount > 0)
        {
            return true;
        }
        else
        {
            // ������ ������ false�� ��ȯ
            return false;
        }
    }

    public void healingpostionuse()
    {
        // ���� �Ѱ��� �Ҹ���
        healingpostioncount--;
    }

}
