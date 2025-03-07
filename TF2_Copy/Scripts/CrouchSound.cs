using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchSound : MonoBehaviour
{
    public AudioSource crouchdown;
    public AudioSource crouchstand;
    public PlayerMove playermove;

    void LateUpdate()
    {
        if (playermove.isSit && Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouchdown.Play();
        }
        else if (!playermove.isSit && Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouchstand.Play();
        }
    }
}
