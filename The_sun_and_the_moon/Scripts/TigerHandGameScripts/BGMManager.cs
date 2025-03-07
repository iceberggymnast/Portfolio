using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource bgmAudioSource; // 배경 음악을 재생할 AudioSource
    public AudioClip scoreIncreaseClip; // 점수 상승 시 재생될 오디오 클립
    public AudioClip actionClip; // 'LEFT', 'RIGHT', 'MISS' 시 재생될 오디오 클립

    void Start()
    {
        if (bgmAudioSource == null)
        {
            Debug.LogError("배경 음악 AudioSource가 할당되지 않았습니다.");
        }

        // 배경 음악이 자동으로 재생되도록 설정
        if (bgmAudioSource != null && !bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Play();
        }
    }

    // 점수 상승 시 BGM 재생 함수
    public void PlayScoreIncreaseBGM()
    {
        if (bgmAudioSource != null && scoreIncreaseClip != null)
        {
            AudioSource.PlayClipAtPoint(scoreIncreaseClip, Camera.main.transform.position);
        }
        else
        {
            Debug.LogError("점수 상승 AudioClip이 할당되지 않았습니다.");
        }
    }

    // 동작 시 BGM 재생 함수
    public void PlayActionBGM()
    {
        if (bgmAudioSource != null && actionClip != null)
        {
            AudioSource.PlayClipAtPoint(actionClip, Camera.main.transform.position);
        }
        else
        {
            Debug.LogError("동작 AudioClip이 할당되지 않았습니다.");
        }
    }
}
