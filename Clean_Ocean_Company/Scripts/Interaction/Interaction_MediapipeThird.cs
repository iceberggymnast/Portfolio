using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_MediapipeThird : MonoBehaviour
{
    Interaction_Base interaction_Base;

    public GameObject MediapipeCanvas;

    public Transform mediapipeStartPos;

    public Camera mediapipeCamera;

    public MediapipePorpoise mediapipePorpoise;

    public MediapipeThirdManager mediapipeThirdManager;

    public Interaction_UICamera_Controller interaction_UICamera_Controller;

    private void OnEnable()
    {
        interaction_Base = GetComponent<Interaction_Base>();
        interaction_Base.action = OpenMediapipe;
        mediapipeStartPos = GameObject.FindWithTag("MediapipeStartPos").transform;
    }

    void OpenMediapipe()
    {
        interaction_UICamera_Controller.IsStart = false;
        MediapipeCanvas.SetActive(true);
        mediapipeCamera.gameObject.SetActive(true);
        mediapipeThirdManager.gameObject.SetActive(true);
        mediapipePorpoise.SetStartSetting();
        mediapipeThirdManager.SetRadius();
        mediapipeThirdManager.ExecutePythonFile();
        mediapipeThirdManager.CloseOtherUI();
        //Camera.main.transform.position = mediapipeStartPos.transform.position;
        CameraRotate cameraRotate = GameObject.FindObjectOfType<CameraRotate>();
        PlayerMove playerMove = GameObject.FindObjectOfType<PlayerMove>();

        cameraRotate.useRotX = false;
        cameraRotate.useRotY = false;
        playerMove.isMove = false;
    }
}
