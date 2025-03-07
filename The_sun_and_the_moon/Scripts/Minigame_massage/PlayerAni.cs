using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAni : MonoBehaviour
{
    public CharacterController CC;
    public Animator anim;

    void Start()
    {

    }

    void Update()
    {

    }

    //ai로부터 result값이 갱신될 때마다 잼잼 애니메이션 실행하는 함수
    public void MassageAnimation()
    {
        anim.SetTrigger("isMassage");
    }
}
