using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    [Header("�̵�")]
    public float moveSpeed = 5.0f;
    public float runSpeed = 10.0f;
    
    [Header("�ɱ�")]
    public float sitSpeed = 2.0f;
    public float playerHeight = 2.0f;
    public float sitHeight = 1.0f;

    [Header("����")]
    public float yVeloccity = 2.0f;
    public float jumpPower = 5.0f;
    public int maxJumpCount = 2;

    [Header("�����̵�")]
    public float slideSpeed = 8.0f;
    public float slidingTime = 3.0f;
    
    [Header("�� Ÿ��")]
    public float wallSlideSpeed = 8.0f;
    public float wallViewAngle = 50;
    
    [Header("ȸ��")]
    public float rotSpeed = 200.0f;

    [Header("����")]
    public bool isRun = true;
    public bool isSit = true;
    public bool isSlide = true;
    public bool isWallRun = true;
    public bool isGround;

    float yPos;
    float currentJumpCount = 0;

    float rotX;
    float rotY;

    float currentTime;

    Vector3 gravityPower;

    float sitValue;

    ShakeObject shakeObject;
    PlayerFire playerFire;
    FollowCamera followCamera;
    CameraController cameraController;
    CharacterController cc;
    ItemPickUp itemPickUp;
    Animator playerAnim;
    
    
    void Start()
    {
        shakeObject = Camera.main.gameObject.GetComponent<ShakeObject>();
        playerFire = GetComponent<PlayerFire>();
        followCamera = Camera.main.gameObject.GetComponent<FollowCamera>();
        cameraController = GetComponent<CameraController>();
        cc = GetComponent<CharacterController>();
        itemPickUp = gameObject.GetComponentInChildren<ItemPickUp>();
        playerAnim = Camera.main.gameObject.GetComponentInChildren<Animator>();

        gravityPower = Physics.gravity;

        // ������ ȸ�� ���´�� �����ϰ� �ʹ�.
        rotX = transform.eulerAngles.x;
        rotY = transform.eulerAngles.y;

    }

    void Update()
    {
        currentTime += Time.deltaTime;

        MoveType();

        RotateType();

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRun = true;
            if(isSit)
            {
                isSit = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            currentTime = 0;

            // Toggle ���
            isSit = !isSit;

            if(isRun)
            {
                isSlide = true;
            }
        }
    }

    void MoveType()
    {
        if(itemPickUp.onPlatform)
        {
            cc.Move(itemPickUp.vel * Time.deltaTime);
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = transform.TransformDirection(dir);
        playerAnim.SetFloat("DirLength", dir.magnitude);
        dir.Normalize();

        // �߷� ����
        yPos += gravityPower.y * yVeloccity * Time.deltaTime;

        // �ٴڿ� ����� �� yPos�� ���� �ʱ�ȭ�Ѵ�.
        if (cc.collisionFlags == CollisionFlags.CollidedBelow)
        {
            yPos = 0;
            currentJumpCount = 0;
        }
        
        // �ٴڿ� ��Ҵ���, �� ��Ҵ��� Ȯ���Ѵ�.
        Ray groundRay = new Ray(transform.position, Vector3.down);
        
        if(Physics.Raycast(groundRay, 1.2f, 1<<6))
        {
            playerAnim.SetBool("isGround", true);
            isGround = true;
        }
        else
        {
            playerAnim.SetBool("isGround", false);
            isGround = false;
        }

        // Space Ű �Է��� ������, ���� �Ѵ�.
        if (Input.GetKeyDown(KeyCode.Space) && currentJumpCount < maxJumpCount)
        {
            yPos = jumpPower;
            currentJumpCount++;
            print("����!");
            
            
            if(isSit)
            {
                isSit = false;
            }
        }

        dir.y = yPos;


        // ���鸸 ���� �Ǿ��� ��
        if (cc.collisionFlags == CollisionFlags.CollidedSides)
        {
            isWallRun = true;
            playerAnim.SetBool("isWallRun", true);
        }
        else
        {
            isWallRun = false;
            playerAnim.SetBool("isWallRun", false);
        } 

        // �� �����̵�
        if (isWallRun)
        {
            yPos = 0;
            
            Ray ray = new Ray(transform.position, transform.right);
            RaycastHit wallInfo;
            bool rightWall = Physics.Raycast(ray, out wallInfo, wallViewAngle, 1<<6);
            
            if(rightWall)
            {
                print(wallInfo.collider.name);
                Vector3 wallGravity = wallInfo.normal * -1f;
                Vector3 wallResult = wallGravity * yVeloccity * Time.deltaTime * 200f;

                cc.Move(wallResult);

                // ������ ���� �Է��� �شٸ� ������ �̵��Ѵ�.
                if (v > 0)
                {
                    cc.Move(dir * wallSlideSpeed * Time.deltaTime);
                    
                    print(v);
                }
                // �Է��� �� �ָ� �Ʒ��� �ٷ� ��������.
                else 
                {
                    Vector3 downDir = new Vector3(0, 1, 0);
                    yPos += gravityPower.y * yVeloccity * Time.deltaTime;
                    downDir.y = yPos;
                    cc.Move(downDir * Time.deltaTime);
                    print(v);
                    
                }
            }
            
        }
        // �ٴ� �����̵�
        else if (isSlide)
        {
            isRun = false;
            
            Vector3 slideDir = transform.forward;
            slideDir.y = yPos;

            float currentSlide = slideSpeed * (1 - (currentTime / slidingTime));
            cc.Move(slideDir * currentSlide * Time.deltaTime);
            playerAnim.SetBool("isSlide", true);

            if (currentTime >= slidingTime || isSit == false)
            {
                isSlide = false;
                playerAnim.SetBool("isSlide", false);
                playerAnim.SetBool("isRun", false);
            }


            print("�����̵�!");
            

        }
        // �޸���
        else if(isRun)
        {
            cc.Move(dir * runSpeed * Time.deltaTime);

            playerFire.isAiming = false;
            Camera.main.fieldOfView = playerFire.originFov;
            
            playerAnim.SetBool("isRun", true);
            
            if (h == 0 && v == 0)
            {
                isRun = false;
                //StopCoroutine(shakeObject.cameraShake);
                shakeObject.shaking = false;
                followCamera.cameraType = FollowCamera.CameraType.BasicType;

                playerAnim.SetBool("isRun", false);
            }
        }
        // �ɱ�
        else if(isSit)
        {
            
            cc.Move(dir * sitSpeed * Time.deltaTime);
            playerAnim.SetBool("isSit", true);
        }
        else
        {
            cc.Move(dir * moveSpeed * Time.deltaTime);
            playerAnim.SetBool("isSit", false);
            
        }

        // �ɾҴ� �Ͼ ��, ������ ��ȭ
        if (isSit)
        {
            cc.height = Mathf.Lerp(playerHeight, sitHeight, currentTime * 8);
            playerAnim.SetFloat("SitValue", sitValue);
            sitValue = Mathf.SmoothStep(0, 1, currentTime * 5);
        }
        else
        {
            cc.height = Mathf.Lerp(sitHeight, playerHeight, currentTime * 8);
            playerAnim.SetFloat("SitValue", sitValue);
            sitValue = Mathf.SmoothStep(1, 0, currentTime * 5);

            if (currentTime < 1.0f) // �Ͼ �� ���� �浹 ����
            {
                cc.Move(new Vector3(0, 1, 0) * 0.05f);
            }
        }
    }

    public PlayerSlide playerSlide;

    // �÷��̾ ���콺 �Է��� �޾� �¿�� ȸ���ϰ� �ʹ�.
    void RotateType()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotX += (mouseY * rotSpeed * Time.deltaTime) - playerSlide.recoilcom.x;
        rotY += (mouseX * rotSpeed * Time.deltaTime) - playerSlide.recoilcom.y;

        if (rotX > 60.0f)
        {
            rotX = 60.0f;
        }
        else if(rotX < -60.0f)
        {
            rotX = -60.0f;
        }

        transform.eulerAngles = new Vector3(0, rotY, 0);
        cameraController.rotX = rotX;
    }

    public float CurrentJumpCount
    {
        get { return currentJumpCount; }
    }

    public CharacterController Cc
    {
        get { return cc; }
    }
}
