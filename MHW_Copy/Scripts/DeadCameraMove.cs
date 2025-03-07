using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class DeadCameraMove : MonoBehaviour
{
    public Camera deadCameara;
    Vector3 startpos = new Vector3(0.5f, 0, 3.5f);
    Vector3 endpos = new Vector3(0, 0, 3.5f);
    Vector3 startrot = new Vector3(0, 190, 0);
    Vector3 endrot = new Vector3(0, 180, 0);
    float timer;
    public float speed = 10.0f;

    public Image balckimg;
    //float colorA = 0f;
    void Start()
    {
        if (deadCameara == null)
        {
            gameObject.GetComponent<Camera>();
        }

        if (balckimg == null)
        {
            balckimg = GameObject.Find("Black").GetComponent<Image>();
        }
    }

    void Update()
    {
        if (deadCameara.enabled == true)
        {
            timer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startpos, endpos, speed * timer);
            transform.localEulerAngles = Vector3.Lerp(startrot, endrot, speed * timer);
        }
        else
        {
            timer = 0;
        }

    }
}
