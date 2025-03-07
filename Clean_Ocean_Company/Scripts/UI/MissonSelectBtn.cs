using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissonSelectBtn : MonoBehaviourPunCallbacks
{
    public TMP_Text roomName;
    public TMP_Text playernumber;

    Interection_RoomMake interection_RoomMake;

    private void Start()
    {
        interection_RoomMake = GameObject.FindObjectOfType<Interection_RoomMake>();
        CurrnetPlayer();
    }

    public void MissonSeclet()
    {
        int missonId = int.Parse(gameObject.name);
        Interection_RoomMake roommake = GameObject.FindAnyObjectByType<Interection_RoomMake>();
        roommake.currentMissonIndex = missonId;
        roommake.overHere.interactable = true;
        SoundManger soundManger = GameObject.FindObjectOfType<SoundManger>();
        soundManger.UISFXPlayRandom(8, 10);
    }

    public void MissonSecletNew()
    {
        int missonId = int.Parse(gameObject.name);
        Interection_RoomMake roommake = GameObject.FindAnyObjectByType<Interection_RoomMake>();
        roommake.currnetRoomName = roomName.text;
        roommake.overHere.interactable = true;
        SoundManger soundManger = GameObject.FindObjectOfType<SoundManger>();
        soundManger.UISFXPlayRandom(8, 10);
    }

    public void CurrnetPlayer()
    {
        for (int i = 0; i < interection_RoomMake.roomLists.Count; i++)
        {
            if (roomName.text == interection_RoomMake.roomLists[i].Name)
            {
                playernumber.text = interection_RoomMake.roomLists[i].PlayerCount.ToString();
                break;
            }

            playernumber.text = "0";
        }

        if (0 == interection_RoomMake.roomLists.Count)
        {
            playernumber.text = "0";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        CurrnetPlayer();
    }
}
