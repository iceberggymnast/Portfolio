using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAddon : MonoBehaviour
{
    GameObject playerobj;
    PlayerMove playerMove;
    GameObject playerHand;
    public GameObject walk;
    public GameObject run;
    
    Transform playernearposDefaultValue;
    void Start()
    {
        playerobj = GameObject.Find("Player");
        playerHand = GameObject.Find("v_rspn101_with_player");
        playernearposDefaultValue = playerHand.transform;
        playerMove = playerobj.GetComponent<PlayerMove>();
    }

    private void Update()
    {
        transform.position = playerobj.transform.position;
        JumpCamera();
        WalkandRun();
    }

    // 점프하고 착지할때 손 움직임 표현
    public GameObject jump;
    public GameObject ground;
    bool groundon;
    float jumpcount;

    void JumpCamera()
    {
        if (jumpcount != playerMove.CurrentJumpCount)
        {
            if (jumpcount < playerMove.CurrentJumpCount)
            {
                groundon = true;
                Animation animationComponent = jump.GetComponent<Animation>();
                animationComponent["JumpOffset"].time = 0;
                animationComponent.Play("JumpOffset");
            }
            jumpcount = playerMove.CurrentJumpCount;

        }

        if (playerMove.Cc.collisionFlags == CollisionFlags.CollidedBelow && groundon)
        {
            groundon = false;
            ground.GetComponent<Animation>().Play();
        }

        playerHand.transform.localEulerAngles = (jump.transform.eulerAngles * -1f) + ground.transform.localEulerAngles;
        playerHand.transform.localPosition = ground.transform.localPosition;
    }

    // 걷는 소리
    void WalkandRun()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.E))
        {
            if (playerMove.Cc.collisionFlags == CollisionFlags.CollidedBelow)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    run.GetComponent<Animation>().Play();
                }
                else
                {
                    walk.GetComponent<Animation>().Play();
                }
            }
        }
    }

}

