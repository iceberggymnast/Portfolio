using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restartbutton : MonoBehaviour
{
    public GameObject player;
    public GameObject gameover;
    Animator animator;

    public void Start()
    {
        if (animator != null)
        {
            GameObject.Find("test2");
        }

        animator = player.GetComponent<Animator>();
    }

    public void Update()
    {
        if (animator != null) 
        { 
        //int dead = animator.GetInteger("hurtnum"); 
        //if (dead == -1)
        //{
        //    gameover.SetActive(true);
        //}
        }

    }

    public void Restart()
    {
        SceneManager.LoadScene(2);
    }
}
