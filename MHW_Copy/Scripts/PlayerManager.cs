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

    // 스테미너 관리용 변수들
    float staminarestoretimer;
    float staminarestoretimer2;
    bool isRestoringStamina;

    public bool blackon;
    public GameObject guardvfx;

    // 아이템 사용 관련 변수들 
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

        // 무기 장착 여부를 보이게 하는 설정
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
        // 가드 여부 확인
        if (controll.GetBool("Guard"))
        {
            // 가드를 하고 있으면 체력 대신 스테미나를 소모함.
            // 스테미나가 있는지 확인하고 충분하면 스테미나만 소모
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
                // 스테미나가 부족하므로 체력이 깎이고 모션 출력
                controll.SetInteger("hurtnum", -1);
                hp = hp - playerHurtDamage;
            }
        }
        else // 가드를 안하면 피격 모션 출력
        {
            // 0. 기본값, 1. 포효, 2. 움찔 3. 맞고 엎어짐
            controll.SetInteger("hurtnum", hitAnimatedNumbers);
            hp = hp - playerHurtDamage;
        }
    }

    //  플레이어 사망 모션 출력
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

    // 죽으면 일정 시간뒤에 스폰 위치로 간 뒤 체력을 회복시켜준다.
    public void PlayerRespawn()
    {
        if (controll.GetCurrentAnimatorStateInfo(1).IsName("Dead"))
        {
            // 카메라 처리
            maincamera.enabled = false;
            deadcamera.enabled = true;
            // 스폰 처리
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

    // 체력바와 스테미나 바의 아웃풋을 관리해주는 함수
    public void Uibarmanager()
    {
        Image hpbarimg = hpbar.GetComponent<Image>();
        Image spbarimg = mpbar.GetComponent<Image>();

        // 체력과 스태미나 값을 0~1 사이로 변환
        float inputhp = (float)hp / 200.0f;
        float inputsp = (float)mp / 150.0f;

        hpbarimg.fillAmount = inputhp;
        spbarimg.fillAmount = inputsp;
    }

    // 스테미나를 사용하는 모션을 하면 스테미나를 깎고 필요한 스테미나보다 현재 스테미나가 적으면 false를 리턴함.
    public bool StaminaManager (int staminause)
    {
        if (mp > staminause)
        {
            // 구르기 애니메이션이 시작이 되면 스테미나를 소모함.
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

    // 스테미나가 줄어들어 있으면 회복한다.
    public void Staminarestore ()
    {
        // isRestoringStamina가 false일때 회복 타이머가 돌아가서 일정 시간이 지나면 ture로 바꿔준다.  
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
            // isRestoringStamina true일때 회복을 할 수 있다.
            if (mp < 150) // 0.0255초마다 회복할 수 있다.
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

    // 포션 사용 함수
    public bool healingpostion()
    {
        // 포션의 갯수를 확인
        if (healingpostioncount > 0)
        {
            return true;
        }
        else
        {
            // 포션이 없으면 false를 반환
            return false;
        }
    }

    public void healingpostionuse()
    {
        // 포션 한개를 소모함
        healingpostioncount--;
    }

}
