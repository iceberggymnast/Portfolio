using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationControll : MonoBehaviour
{
    Animator animator;
    EnemyAi enemyAi;
    NavMeshAgent agent;
    Transform playerChestTr;
    Transform playerHandTr;
    public Transform gunGrabPos;
    public bool crouch;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        enemyAi = GetComponent<EnemyAi>();
        agent = GetComponent<NavMeshAgent>();
        playerChestTr = animator.GetBoneTransform(HumanBodyBones.UpperChest);
        playerHandTr = animator.GetBoneTransform(HumanBodyBones.LeftHand);
    }

    void Update()
    {
        Walking();
        

        crouch = enemyAi.enemyseat;
        if (crouch)
        {
            animator.SetBool("Crouch", true);
        }
        else
        {
            animator.SetBool("Crouch", false);
        }
    }

    void LateUpdate()
    {
        Aim();
    }



    public Vector3 relativeVecChest;

    void Aim()
    {
        if (enemyAi.detected)
        {
            Vector3 chestDir = (enemyAi.player.transform.position - playerChestTr.transform.position).normalized;
            playerChestTr.LookAt(playerChestTr.position + chestDir);
            playerChestTr.rotation = playerChestTr.rotation * Quaternion.Euler(relativeVecChest);
        }
        
    }

    void Walking()
    {
        animator.SetFloat("Walking", agent.velocity.magnitude);
    }

    public AudioSource reloadSound;

    public void ReloadAnimation()
    {   
        animator.SetTrigger("Reload");
        reloadSound.Play();
    }

    public void FireAnimation()
    {
        animator.SetTrigger("Fire");
    }

}
