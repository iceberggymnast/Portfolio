using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foword : MonoBehaviour
{

    public GameObject hitPos;
    public float speed;

    float distance;

    public AudioSource ricecakeEat;

    bool playsound;

    private void Start()
    {
        hitPos = GameObject.Find("Stones hit");
    }
    void Update()
    {
        transform.eulerAngles += new Vector3(10, 0, 0);

        Vector3 dir = hitPos.transform.position - transform.position;
        distance = dir.magnitude;
        dir.Normalize();

        transform.position += dir * speed * Time.deltaTime;

        if (distance < 0.1f)
        {
            if (!playsound)
            {
                playsound = true;
                ricecakeEat.Play();
                hitPos.transform.parent.transform.position += new Vector3(0, -1, 0);
                ParticleSystem party = hitPos.transform.GetComponent<ParticleSystem>();
                party.Play();
                Destroy(transform.GetChild(0));
            }
        }
    }
}
