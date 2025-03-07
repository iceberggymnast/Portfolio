using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMoveInLobby : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotSpeed = 120f;
    public bool isMoving = true;
    private GameObject playerParent;
    private GameObject playerModel;
    private Rigidbody playerRigidbody;
    Quaternion playerRotation;

    Transform cameraPos;

    Vector3 moveDirection;
    Vector3 dir;

    Animator animator;

    private void Awake()
    {
        playerParent = this.gameObject;
        playerRigidbody = GetComponent<Rigidbody>();
        //playerModel = playerParent.transform.GetChild(0).gameObject;
        //cameraPos = transform.GetChild(1);
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            print(playerParent.transform.GetChild(0).gameObject.name);
        }
    }

    public void MoveInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        dir = new Vector3(h, 0, v);
        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }

        if (animator != null)
        {
            animator.SetFloat("mig", dir.magnitude);
        }

        //playerRigidbody.velocity = dir * moveSpeed * Time.deltaTime;

        RotationInput();
    }

    public void RotationInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        moveDirection = new Vector3 (h, 0, v);
        if (moveDirection.magnitude > 1)
        {
            moveDirection.Normalize();
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree"))
        {
            animator.SetBool("RightTurn", false);
            animator.SetBool("LeftTurn", false);
            animator.SetBool("180Turn", false);
            return;
        }

        if (h >= 0.1f) // D 입력
        {
            // 0이면 오른쪽 턴
            // 90이면 아무것도 안함
            // 180이면 왼쪽 턴
            // 270이면 180도 턴
            if (transform.eulerAngles.y <= 0.001f)
            {
                animator.SetBool("RightTurn", true);
                animator.SetBool("LeftTurn", false);
                animator.SetBool("180Turn", false);
            }
            else if (transform.eulerAngles.y == 90)
            {
                animator.SetBool("RightTurn", false);
                animator.SetBool("LeftTurn", false);
                animator.SetBool("180Turn", false);
            }
            else if (transform.eulerAngles.y == 180)
            {
                animator.SetBool("LeftTurn", true);
                animator.SetBool("180Turn", false);
                animator.SetBool("RightTurn", false);
            }
            else if (transform.eulerAngles.y == 270)
            {
                animator.SetBool("180Turn", true);
                animator.SetBool("LeftTurn", false);
                animator.SetBool("RightTurn", false);

            }
        }
        else if (h <= -0.1f) // A 입력
        {
            // 0이면 왼쪽 턴
            // 90이면 180도 턴 
            // 180이면 오른쪽 턴
            // 270이면 아무것도 안함
            if (transform.eulerAngles.y <= 0.001f)
            {
                animator.SetBool("LeftTurn", true);
                animator.SetBool("180Turn", false);
                animator.SetBool("RightTurn", false);
            }
            else if (transform.eulerAngles.y == 90)
            {
                animator.SetBool("180Turn", true);
                animator.SetBool("LeftTurn", false);
                animator.SetBool("RightTurn", false);
            }
            else if (transform.eulerAngles.y == 180)
            {
                animator.SetBool("RightTurn", true);
                animator.SetBool("LeftTurn", false);
                animator.SetBool("180Turn", false);
            }
            else if (transform.eulerAngles.y == 270)
            {
                animator.SetBool("RightTurn", false);
                animator.SetBool("LeftTurn", false);
                animator.SetBool("180Turn", false);
            }
        }
        else if (v >= 0.1f) // W 입력
        {
            // 0이면 아무것도 안함
            // 90이면 왼쪽 턴 
            // 180이면 180 턴
            // 270이면 오른쪽 턴 안함
            if (transform.eulerAngles.y <= 0.001f)
            {
                animator.SetBool("RightTurn", false);
                animator.SetBool("LeftTurn", false);
                animator.SetBool("180Turn", false);
            }
            else if (transform.eulerAngles.y == 90)
            {
                animator.SetBool("LeftTurn", true);
                animator.SetBool("180Turn", false);
                animator.SetBool("RightTurn", false);
            }
            else if (transform.eulerAngles.y == 180)
            {
                animator.SetBool("180Turn", true);
                animator.SetBool("LeftTurn", false);
                animator.SetBool("RightTurn", false);
            }
            else if (transform.eulerAngles.y == 270)
            {
                animator.SetBool("RightTurn", true);
                animator.SetBool("LeftTurn", false);
                animator.SetBool("180Turn", false);
            }
        }
        else if ( v <= -0.1f) // S 입력
        {
            // 0이면 180 턴
            // 90이면 오른쪽 턴 
            // 180이면 아무것도안함
            // 270이면 왼쪽 
            if (transform.eulerAngles.y <= 0.001f)
            {
                animator.SetBool("180Turn", true);
                animator.SetBool("LeftTurn", false);
                animator.SetBool("RightTurn", false);
            }
            else if (transform.eulerAngles.y == 90)
            {
                animator.SetBool("RightTurn", true);
                animator.SetBool("LeftTurn", false);
                animator.SetBool("180Turn", false);
            }
            else if (transform.eulerAngles.y == 180)
            {
                animator.SetBool("RightTurn", false);
                animator.SetBool("LeftTurn", false);
                animator.SetBool("180Turn", false);
            }
            else if (transform.eulerAngles.y == 270)
            {
                animator.SetBool("LeftTurn", true);
                animator.SetBool("180Turn", false);
                animator.SetBool("RightTurn", false);
            }
        }

        /*
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            targetRotation.Normalize();
            playerParent.transform.rotation = Quaternion.Slerp(playerParent.transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
        } */
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;
        MoveInput();
    }

    private void LateUpdate()
    {
        //transform.position = playerModel.transform.position;
    }

    public void Check()
    {
        print(transform.eulerAngles.y);
        if (transform.eulerAngles.y > 225 && transform.eulerAngles.y < 315)
        {
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, 270, transform.eulerAngles.z);
            transform.DORotate(new Vector3(transform.eulerAngles.x, 270, transform.eulerAngles.z), 0.2f);
        }
        else if (transform.eulerAngles.y > 45 && transform.eulerAngles.y < 135)
        {
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
            transform.DORotate(new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z), 0.2f);
        }
        else if (transform.eulerAngles.y > 135 && transform.eulerAngles.y < 225)
        {
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
            transform.DORotate(new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z), 0.2f);

        }
        else
        {
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            transform.DORotate(new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z), 0.2f);

        }
    }
}
