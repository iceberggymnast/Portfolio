using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class bulletTrail : MonoBehaviour
{
    public GameObject hitVFX;

    float time;
    bool hit;
    public float life;
    public float speed;
    Rigidbody rb;


    RaycastHit hit2;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Physics.Raycast(transform.position, transform.forward, out hit2);
    }

    void Update()
    {
        gameObject.transform.position += transform.forward * speed * Time.deltaTime;

        if (!hit)
        {
            time += Time.deltaTime;
            //rb.velocity += transform.forward * speed * Time.deltaTime;

            if (time >= life)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 dir = hit2.normal;
        Vector3 pos = hit2.point;
        GameObject go = Instantiate(hitVFX);
        go.transform.position = pos + (transform.forward * - 0.01f );
        go.transform.rotation = Quaternion.LookRotation(dir);
        hit = true;
        Destroy(gameObject);
    }


    

}
