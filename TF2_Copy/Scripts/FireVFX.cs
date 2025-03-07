using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireVFX : MonoBehaviour
{
    GameObject def_c_proscreen;
    public ParticleSystem gunfire;
    PlayerFire playerFire;

    AnimationPlaySound fireSound;

    void Start()
    {
        def_c_proscreen = GameObject.Find("def_c_proscreen");
        fireSound = GetComponent<AnimationPlaySound>();
        playerFire = GameObject.Find("Player").GetComponent<PlayerFire>();
    }


    void Update()
    {
        transform.position = def_c_proscreen.transform.position + transform.forward * 0.2f;
        transform.rotation = def_c_proscreen.transform.rotation;
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerFire.currentBullet != 0 && Time.timeScale != 0)
        {
            fireSound.SoundsPlay();
            gunfire.Play();
        }
    }
}
