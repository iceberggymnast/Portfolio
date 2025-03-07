using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class QuestTrigger4 : MonoBehaviour
{
    public StoryObserver storyObserver;
    public DoorOpen door;
    bool trigger;

    public Volume volume;

    bool sun;
    bool moon;
    bool night;

    public SunAndNight sAN;

       void Update()
    {

        if (sun && moon && !trigger)
        {
            trigger = true;
            door.isOpen = false;
            if(!night)
            {
                night = true;
                StartCoroutine(TrunNight());
            }
        }
        if (trigger)
        {
            storyObserver.quest4time += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("진입 확인됨");
        if (QuestManager.questManager.questList[4].questState == QuestData.QuestState.progress)
        {
            print("4번 퀘스트 진행 중");
            if (other.gameObject.tag == "Player")
            {
                print("플레이어 확인 됨");
                PhotonView photonView = other.gameObject.GetComponent<PhotonView>();
                if (photonView.Owner.NickName == "해님")
                {
                    sun = true;
                    print("해님 집에 들어옴");
                }
                else
                {
                    moon = true;
                    print("달님 집에 들어옴");
                }
            }

        }
    }

    // 밤을 만들어주는 코루틴
    IEnumerator TrunNight()
    {
        yield return new WaitForSeconds(2);

        ColorAdjustments colorAdjustments;
        volume.profile.TryGet(out colorAdjustments);

        while (colorAdjustments.postExposure.value > -10.0f)
        {
            colorAdjustments.postExposure.value += Time.deltaTime * -5;
            yield return null;
        }

        sAN.TrunNight();
        yield return new WaitForSeconds(2);

        while (colorAdjustments.postExposure.value < 0.0f)
        {
            colorAdjustments.postExposure.value += Time.deltaTime * 5;
            yield return null;
        }

        colorAdjustments.postExposure.value = 0;
    }

}
