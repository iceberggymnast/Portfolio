using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_MediapipePorpoise : MonoBehaviour
{
    Interaction_Base interaction_Base;

    public GameObject MediapipeCanvas;

    public Transform mediapipeStartPos;


    private void OnEnable()
    {
        interaction_Base = GetComponent<Interaction_Base>();
        interaction_Base.action = OpenMediapipe;


        mediapipeStartPos = GameObject.FindWithTag("MediapipeStartPos").transform;
    }

    void OpenMediapipe()
    {
        MediapipeCanvas.SetActive(true);

        StartCoroutine(UIController.instance.FadeOut("InteractionCanvas", 0.02f));
        Camera.main.transform.position = mediapipeStartPos.transform.position;
        CameraRotate cameraRotate = GameObject.FindObjectOfType<CameraRotate>();
        PlayerMove playerMove = GameObject.FindObjectOfType<PlayerMove>();

        cameraRotate.useRotX = false;
        cameraRotate.useRotY = false;
        playerMove.isMove = false;
    }
}
