using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSet : MonoBehaviour
{
    // 3D 월드에 배치할때 쓰는 스크립트

    public List<AudioClip> clipSFX;
    [SerializeField]
    public List<AudioSource> audioSources;
    public float volume = 1;

    // 사운드 리스트에 있는 사운드를 재생
    public void OBJSFXPlay(int index, float volumeValue, bool addPlay = false)
    {
        if (addPlay)
        {
            int audioIndex = AddAudioSources();
            audioSources[audioIndex].volume = volumeValue;
            audioSources[audioIndex].clip = clipSFX[index];
            audioSources[audioIndex].Play();
        }
        else
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (audioSources[i].clip == clipSFX[index])
                {
                    if (audioSources[i].isPlaying)
                    {
                        return;
                    }
                }
            }
            int audioIndex = AddAudioSources();
            audioSources[audioIndex].volume = volumeValue;
            audioSources[audioIndex].clip = clipSFX[index];
            audioSources[audioIndex].Play();
        }
    }

    public void OBJSFXPlay(int index, bool addPlay = false)
    {
        if (addPlay)
        {
            int audioIndex = AddAudioSources();
            audioSources[audioIndex].volume = volume;
            audioSources[audioIndex].clip = clipSFX[index];
            audioSources[audioIndex].Play();
        }
        else
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (audioSources[i].clip == clipSFX[index])
                {
                    if (audioSources[i].isPlaying)
                    {
                        return;
                    }
                }
            }
            int audioIndex = AddAudioSources();
            audioSources[audioIndex].volume = volume;
            audioSources[audioIndex].clip = clipSFX[index];
            audioSources[audioIndex].Play();
        }
    }

    // 사운드 리스트에 있는 재생중인 사운드를 찾아 정지
    public void OBJSFXStop(int index)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].clip == clipSFX[index])
            {
                audioSources[i].Stop();
            }
        }
    }

    // 사운드 리스트에 있는 사운드의 범위를 랜덤으로 재생
    public void OBJSFXPlayRandom(int minIndex, int maxIndex, float volumeValue, bool addPlay = false)
    {
        if (addPlay)
        {
            int audioIndex = AddAudioSources();
            audioSources[audioIndex].volume = volumeValue;
            int random = UnityEngine.Random.Range(minIndex, maxIndex);
            audioSources[audioIndex].clip = clipSFX[random];
            audioSources[audioIndex].Play();
        }
        else
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                for (int j = 0; j < maxIndex - minIndex; j++)
                {
                    if (audioSources[i].clip == clipSFX[j + minIndex])
                    {
                        if (audioSources[i].isPlaying)
                        {
                            return;
                        }
                    }
                }
            }
            int audioIndex = AddAudioSources();
            audioSources[audioIndex].volume = volumeValue;
            int random = UnityEngine.Random.Range(minIndex, maxIndex);
            audioSources[audioIndex].clip = clipSFX[random];
            audioSources[audioIndex].Play();
        }
    }

    public void OBJSFXPlayRandom(int minIndex, int maxIndex, bool addPlay = false)
    {
        if (addPlay)
        {
            int audioIndex = AddAudioSources();
            audioSources[audioIndex].volume = volume;
            int random = UnityEngine.Random.Range(minIndex, maxIndex);
            audioSources[audioIndex].clip = clipSFX[random];
            audioSources[audioIndex].Play();
        }
        else
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                for (int j = 0; j < maxIndex - minIndex; j++)
                {
                    if (audioSources[i].clip == clipSFX[j + minIndex])
                    {
                        if (audioSources[i].isPlaying)
                        {
                            return;
                        }
                    }
                }
            }
            int audioIndex = AddAudioSources();
            audioSources[audioIndex].volume = volume;
            int random = UnityEngine.Random.Range(minIndex, maxIndex);
            audioSources[audioIndex].clip = clipSFX[random];
            audioSources[audioIndex].Play();
        }
    }

    public int AddAudioSources()
    {
        for (int i = audioSources.Count - 1; i >= 0; i--)
        {
            if (audioSources[i].isPlaying == false)
            {
                Destroy(audioSources[i]);
                audioSources.Remove(audioSources[i]);
            }
        }
        audioSources.Add(gameObject.AddComponent<AudioSource>());
        audioSources[audioSources.Count - 1].playOnAwake = false;
        audioSources[audioSources.Count - 1].spatialBlend = 1.0f;
        return audioSources.Count - 1;
    }

    [TextArea(1, 3)]
    public string memo;
}
