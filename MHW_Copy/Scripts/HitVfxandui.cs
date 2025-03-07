using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVfxandui : MonoBehaviour
{
    public GameObject hitvfx;
    ParticleSystem hitpartocle;
    AudioSource hitaudio;
    Animator controll;

    void Start()
    {
        hitpartocle = hitvfx.GetComponent<ParticleSystem>();
        controll = GameObject.Find("test2").GetComponent<Animator>();
        hitaudio = hitvfx.GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (hitpartocle != null)
        { hitpartocle.Play(); }
        if (hitaudio != null)
        { hitaudio.Play(); }
        controll.SetBool("HitCheck", true);
    }
}
