using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // �÷��̾ ��ư�� ���� �����̰� �ϰ� �ʹ�.
    public float moveSpeed = 10.0f; // �����̴� �ӵ� ����
    public float rollSpeed = 0.001f; // ������ �ӵ� ����
    public float sitMoveSpeedRatio = 0.8f; // ��
    public float runMoveSpeedRatio = 1.2f; // �۶� �ӵ��� ������ ����
    public float rotationSpeed = 0.02f; // ȸ���ϴ� �ӵ� ����
    public GameObject playerCamera; // �÷��̾� ī�޶� �Ҵ�� ����
    float wasdDrgree; // �Է¹��� ����Ű�� ����
    public float speedOffset = 1;

    public bool goup;

    //ĳ���� ��Ʈ�ѷ�
    CharacterController cc;
    float yVelocity = 0;

    //ĳ���� ��ǲ
    float h;
    float v;
    public bool outputLock = false;

    // �ִϸ��̼� üũ
    Animator controll;
    GameObject playerModel;

    void Start()
    {
        // playerCamera ������ �±װ� MainCamera�� ���� ������Ʈ�� �����Ѵ�.
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cc = GetComponent<CharacterController>();

        playerModel = GameObject.Find("test2");
        controll = playerModel.GetComponent<Animator>();
    }

    void Update()
    {
        // ������� �Է� �ޱ�
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (outputLock == false)
        {
            controll.SetBool("ismove", true);
            PlayerRotaion();
            PlayerMoving();
        }
        else
        {
            controll.SetBool("ismove", false);
        }
    }

        void PlayerRotaion()
    {
        // Ű �Է��� ���� ���� ȸ�� �ؾ� ��
        if (v != 0 || h != 0)
        {
            // wasd �Է¿� ���� ������ ���غ���
            Vector2 playerkeyinput = new Vector2(h, v);
            wasdDrgree = Vector2.SignedAngle(new Vector2(0, 1), playerkeyinput); // ������ ������ ���ϴ� �Լ� (0 ~ 360), ������ ����

            // ī�޶� �������� ������ ���� ������ �ٶ󺸰� �����
            // Quaternion.Slerp(�������� �� ��, Ÿ�� ��, �ӵ�)
            transform.rotation = Quaternion.Slerp(transform.rotation, playerCamera.transform.rotation * Quaternion.Euler(0, wasdDrgree * -1, 0), rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
    }

    void PlayerMoving()
    {
        // Ű �Է��� �޾��� �� �Է��� ���� ���� ī�޶� �������� �ش� �������� �ɾ
        Vector3 playerMoveKeyInput = new Vector3(h, 0, v); // �÷��̾� �Է� �� ���Ϳ� �Ҵ�
        float cameraDrgree = playerCamera.transform.eulerAngles.y; // ī�޶� �����̼��� �ޱ۷� Y �� �޾ƿ���
        Vector3 dir = Quaternion.Euler(0, cameraDrgree, 0) * playerMoveKeyInput; // �Է°��� ���Ϳ� ī�޶� ȸ�� �� ��ŭ ȸ�� ( A * B �ϸ� A���� B�� ����ŭ ���ư�)

        //dir���� ���̰� 1�� ������ ��ֶ�����

        if (Vector3.Magnitude(dir) > 1)
        {
            dir = dir.normalized;
        }

        // �߷� �� �߰�
        if (goup)
        {
            yVelocity = 0;
        }
        else
        {
            yVelocity = 0;
            yVelocity += -100f * Time.deltaTime;
        }
        dir.y = yVelocity;

        if (Input.GetKey(KeyCode.LeftShift)) // �ٱ� ���θ� �Ǵ��ϰ� �����̰� �����.
        { cc.Move(dir * moveSpeed * Time.deltaTime * runMoveSpeedRatio); }
        else { cc.Move(dir * moveSpeed * Time.deltaTime * speedOffset); }
    }

    public void Playerforward(float forwardspeed)
    {
        //print("���������ݰ���!");
        // �ִϸ��̼� �÷��� ��
        float lookR = transform.eulerAngles.y; // ���� �÷��̾��� Y �����̼� ��

        Vector3 rolldir = Quaternion.Euler(0, lookR, 0) * Vector3.forward; // �� ������ �������� * lookR���� ������ ��
        cc.Move(rolldir * forwardspeed * Time.deltaTime); // �� �������� ����

        //(rolldir);
    }

    public void Playerright(float rightspeed)
    {
        //print("���������ݰ���!");
        // �ִϸ��̼� �÷��� ��
        float lookR = transform.eulerAngles.y; // ���� �÷��̾��� Y �����̼� ��

        Vector3 rolldir = Quaternion.Euler(0, lookR + 90, 0) * Vector3.forward; // �� ������ �������� * lookR���� ������ ��
        cc.Move(rolldir * rightspeed * Time.deltaTime); // �� �������� ����

        //print(rolldir);
    }

    public void Playerup(float upSpeed)
    {
        cc.Move(new Vector3 (0, 1, 0) * upSpeed * Time.deltaTime);
        if (upSpeed == 0)
        {
            goup = false;
        }
        else
        {
            goup = true;
        }
    }


}

