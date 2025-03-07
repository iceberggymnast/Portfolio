using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeSlide : MonoBehaviour
{

    public CharacterController cc;
    public float slideSpeed = 20.0f;

    Vector3 slideDirection;

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>() != null)
        {
            cc = other.gameObject.GetComponent<CharacterController>();
        }

        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().loop = true;

    }

    private void OnTriggerStay(Collider other)
    {
        if(cc != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(cc.gameObject.transform.position, Vector3.down, out hit, 2.5f, ~(1 << 8)))
            {
                Vector3 normal = hit.normal;
                slideDirection = Vector3.ProjectOnPlane(Vector3.down, normal);
            }
            cc.Move(slideDirection * slideSpeed * Time.deltaTime);
            cc.Move(cc.gameObject.transform.forward * Time.deltaTime);
        }
    }

    public AudioSource audioSource;
    private void OnTriggerExit(Collider other)
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().loop = false;
        audioSource.Play();
    }
}
