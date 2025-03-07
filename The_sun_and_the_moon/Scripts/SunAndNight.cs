using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAndNight : MonoBehaviour
{
    // 아침
    public Material sunMat;
    public GameObject sunObj;
    public AudioSource noonBGM;  // 해 떨어지기 전 BGM

    // 저녁
    public Material moonMat;
    public GameObject moonObj;
    public GameObject moonObj2;
    public AudioSource midnightBGM;  // 해 떨어진 후 BGM

    // 반반
    public Material suonMat;


    void Start()
    {
        DynamicGI.UpdateEnvironment();

        // 기본으로 아침 BGM 재생
        if (noonBGM != null)
        {
            noonBGM.Play();
        }

        // 어머니를 보내는 퀘스트를 깬 이후 씬이 로드되면 저녁으로 로드 됨
        if (QuestManager.questManager.questList[3].questState == QuestData.QuestState.completed)
        {
            // 밤으로 변경
            TrunNight();
        }
    }

    public void TrunNight()
    {
        RenderSettings.skybox = moonMat;
        sunObj.SetActive(false);
        moonObj.SetActive(true);
        moonObj2.SetActive(true);

        // 기존 아침 BGM 멈추기
        if (noonBGM != null && noonBGM.isPlaying)
        {
            noonBGM.Stop();
        }

        // 밤 BGM 재생
        if (midnightBGM != null)
        {
            midnightBGM.Play();
        }

        // GI 업데이트
        DynamicGI.UpdateEnvironment();
    }
    
    public void TrunMoring()
    {
        RenderSettings.skybox = sunMat;
        sunObj.SetActive(true);
        moonObj.SetActive(false);
        moonObj2.SetActive(false);
        // GI 업데이트
        DynamicGI.UpdateEnvironment();
    }

    public void TurnsuonMat()
    {
        RenderSettings.skybox = suonMat;
        sunObj.SetActive(false);
        moonObj.SetActive(false);
        moonObj2.SetActive(false);
        // GI 업데이트
        DynamicGI.UpdateEnvironment();
    }
}
