using System;
using System.Collections;
using UnityEngine;

public class Interaction_UICamera_Controller : MonoBehaviour
{
    bool isStart = false;
    Camera uiCamera;


    public bool IsStart
    {
        get 
        {   
            return isStart;
        }
        set
        {
            isStart = value;
            OnIsStartChanged?.Invoke(isStart);


            // IsStart 가 false 일 경우
            // UICameraController(false) 
            // UICamera 의 CullingMask 가 'UI' Layer 만 볼 수 있음

            // IsStart 가 true 일 경우
            // UICameraController(true) 
            // UICamera 의 CullingMask 가 'UI', 'ToolTipCanvas' Layer 들을 볼 수 있음

            if (!isStart)
            {
                IsStart = true;
                uiCamera.cullingMask = LayerMask.GetMask("UI");
            }
            else if (isStart)
            {
                isStart = false;
                uiCamera.cullingMask = LayerMask.GetMask("UI", "ToolTipCanvas");
            }
        }
    }

    public event Action<bool> OnIsStartChanged;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(() => PlayerInfo.instance?.player != null);

        CameraRotate cameraRotate = PlayerInfo.instance.player.GetComponentInChildren<CameraRotate>();
        GameObject uiCameraObj = cameraRotate.uiCamera;
        if (uiCameraObj != null)
        {
            uiCamera = uiCameraObj.GetComponent<Camera>();
        }
    }


}
