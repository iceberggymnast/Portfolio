using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    // UI나 배경음악 등 통상적으로 쓰는 사운드용 스크립트

    public List<AudioClip> clipBGM;
    public List<AudioClip> clipSFX;

    public AudioSource bGM;
    public AudioSource sFX;

    public float volumeBGM = 1;
    public float volumeSFX = 1;

    private void Update()
    {

    }

    public void BGMSetting(int index)
    {
        bGM.volume = volumeBGM;
        bGM.clip = clipBGM[index];
        bGM.Play();
    }

    public void UISFXPlay(int index)
    {
        sFX.volume = volumeSFX;
        sFX.clip = clipSFX[index];
        sFX.Play();
    }

    public void UISFXPlayRandom(int minIndex, int maxIndex)
    {
        if (!sFX.isPlaying)
        {
            sFX.volume = volumeSFX;
            int random = UnityEngine.Random.Range(minIndex, maxIndex);
            sFX.clip = clipSFX[random];
            sFX.Play();
        }
    }

    [TextArea(1, 3)]
    public string memo;

}
