using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThrowRicecakeManager : MonoBehaviourPun
{
    public GameObject tiger;
    public GameObject ricecakePos1;
    public GameObject ricecakePos2;

    public int ramainRicecake = 5;
    public int score = 0;
    public int objectivesocr = 5;

    public TMP_Text remaintext;
    public TMP_Text socretext;

    public MiniGameCanvas miniGameCanvas;

    public AudioSource throwRiceCakeBGM;  // 인스펙터에서 연결


    void Update()
    {
        remaintext.text = ramainRicecake.ToString() + "개";
        socretext.text = score.ToString() + "개";

        if (Input.GetKeyDown(KeyCode.Space))
        {
            throwRiceCakeBGM.Play();  // BGM 재생
            if (PhotonNetwork.NickName == "해님")
            {
                if (ramainRicecake <= 0) return;
                ramainRicecake--;
                miniGameCanvas.socoreBarValue += 0.2f;
                PhotonNetwork.Instantiate("Ricecake", ricecakePos1.transform.position, ricecakePos1.transform.rotation);
            }
            else
            {
                if (ramainRicecake <= 0) return;
                ramainRicecake--;
                miniGameCanvas.socoreBarValue += 0.2f;
                PhotonNetwork.Instantiate("Ricecake", ricecakePos2.transform.position, ricecakePos2.transform.rotation);
            }
        }

        if (miniGameCanvas.socoreBarValue >= 1)
        {
            if (PhotonNetwork.NickName == "해님")
            {
                PhotonView pv1 = miniGameCanvas.GetComponent<PhotonView>();
                pv1.RPC(nameof(miniGameCanvas.WinPopup), RpcTarget.All, 0);
                //miniGameCanvas.StartCoroutine(miniGameCanvas.WinPopup(0));
                //miniGameCanvas.sunMoonWin[0] = true;
            }
            else
            {
                PhotonView pv1 = miniGameCanvas.GetComponent<PhotonView>();
                pv1.RPC(nameof(miniGameCanvas.WinPopup), RpcTarget.All, 1);
                //miniGameCanvas.StartCoroutine(miniGameCanvas.WinPopup(1));
                //miniGameCanvas.sunMoonWin[1] = true;
            }
        }

    }
}