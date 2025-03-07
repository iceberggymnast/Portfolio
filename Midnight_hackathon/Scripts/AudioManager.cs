using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioSource> sources = new List<AudioSource>();

    public void AudioPlay(int playnum)
    {
        sources[playnum].Play();
    }


}
