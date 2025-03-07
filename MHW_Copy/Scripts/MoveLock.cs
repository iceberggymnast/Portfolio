using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLock : MonoBehaviour
{
    public GameObject playercamera;
    public GameObject plateranimation;
    void Start()
    {

    }

    void Update()
    {
        playercamera.GetComponent<PlayerCamera>().cameralock = true;
        plateranimation.GetComponent<Playeranimation>().cantClick = true;
        plateranimation.GetComponent<Playeranimation>().inputLock = true;
    }
}
