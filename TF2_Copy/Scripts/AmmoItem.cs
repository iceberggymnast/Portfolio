using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        if (transform.parent == null)
        {
            Rigidbody rb = transform.GetComponentInChildren<Rigidbody>();
            if(rb != null)
            {
                rb.isKinematic = false;
            }
            //transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
            // Åº¾àÀÌ Ã¤¿öÁü..
            //Destroy(gameObject);
        }
    }
}
