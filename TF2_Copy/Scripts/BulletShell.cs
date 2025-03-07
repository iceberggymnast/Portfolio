    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShell : MonoBehaviour
{
    float time = 0;
    bool collisionsound;
    public List<AudioSource> audioSources = new List<AudioSource>();
    int randomsound;

    void Start()
    {
        Rigidbody rb = GetComponentInChildren<Rigidbody>();
        Vector3 up = transform.forward * Random.Range(-0.5f, -1.5f);
        Vector3 dir = transform.right + up;
        float power = Random.Range(100.0f, 150.0f);
        rb.AddForce(dir * power);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > 3)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collisionsound)
        {
            randomsound = Random.Range(0, 8);
            audioSources[randomsound].Play();
            collisionsound = true;
        }
    }
}
