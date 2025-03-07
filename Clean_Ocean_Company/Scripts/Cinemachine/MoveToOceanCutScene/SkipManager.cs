using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static Interaction_NextScene;

public class SkipManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnClickSkipButton()
    {
        PhotonNetwork.LoadLevel("StartScene");
    }
}
