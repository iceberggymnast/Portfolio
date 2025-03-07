using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ItemPickUp : MonoBehaviour
{
    PlayerFire playerfire;
    public Vector3 vel;
    public bool onPlatform;


    void Start()
    {
        playerfire = GameObject.Find("Player").GetComponent<PlayerFire>();
        
    }

    
    void Update()
    {
       
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            print("ÃÑ¾Ë");
            playerfire.playerHaveBullet += 100;
            playerfire.bulletText[2].text = playerfire.playerHaveBullet.ToString();
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<VelocityCalculator>() != null)
        {
            vel = other.gameObject.GetComponent<VelocityCalculator>().GetVelocity();
            onPlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<VelocityCalculator>() != null)
        {
            vel = other.gameObject.GetComponent<VelocityCalculator>().GetVelocity();
            onPlatform = false;
        }
    }

}
