using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpeaker : MonoBehaviour
{
    public List<AudioClip> AudioClipList = new List<AudioClip>();

    public Dictionary<string, AudioClip> keyAudios = new Dictionary<string, AudioClip>();

    public AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < AudioClipList.Count; i++)
        {
            keyAudios[i.ToString()] = AudioClipList[i];
        }
    }



    public void PlayAudioClip(int clipNum)
    {
        if (keyAudios.ContainsKey(clipNum.ToString()))
        {
            audioSource.clip = keyAudios[clipNum.ToString()];
            audioSource.Play();
        }
    }

}
