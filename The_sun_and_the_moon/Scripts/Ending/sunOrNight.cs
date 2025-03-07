using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class sunOrNight: MonoBehaviour
{
    SunAndNight sunAndNight;

    private void Start()
    {
    
       sunAndNight = FindObjectOfType<SunAndNight>();
        sunAndNight.TrunNight();

    }

    public void sun()
    {
        sunAndNight.TrunMoring();
    }

    public void Night()
    {
        sunAndNight.TrunNight();

    }

    public void Half()
    {
        sunAndNight.TurnsuonMat();
    }

    public void next()
    {
        QuestManager.questManager.dialogSystem.NextConversation();
    }

}
