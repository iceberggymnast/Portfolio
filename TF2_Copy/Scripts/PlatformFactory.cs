using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlatformFactory : MonoBehaviour
{
    public GameObject platform;
    GameObject go;
    public Transform pos;
    public float makeTime;
    float currntTime;
    float startTime;
    void Start()
    {
        // ÇÃ·§ÆûÀ» Ã³À½¿¡ ¿©·Á°³ ¸¸µé¾î ÁÜ
        for (int i = 0; i < 10; i++)
        {
            GameObject go = Instantiate(platform, pos);
            go.transform.position = transform.position;
            Animation animationComponent = go.GetComponent<Animation>();

            animationComponent["platform_1"].time = startTime;
            animationComponent.Play("platform_1");
            startTime = startTime + makeTime;
        }
    }

    void Update()
    {
        currntTime += Time.deltaTime;

        if (currntTime >= makeTime)
        {
            go = Instantiate(platform, pos);
            go.transform.position = transform.position;

            currntTime = 0;
            go.GetComponent<Animation>().Play();
        }
    }
}
