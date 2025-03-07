using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<Transform> camList = new List<Transform>();

    public float rotX;

    void Start()
    {
        camList.Add(transform.GetChild(0));
        camList.Add(transform.GetChild(1));
    }


    public PlayerSlide playerSlide;

    void Update()
    {
        camList[0].localEulerAngles = new Vector3(-rotX, 0, playerSlide.zrecoillerp + playerSlide.runLerp + playerSlide.zhurtLerp);
    }
}
