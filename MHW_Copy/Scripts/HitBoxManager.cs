using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    public bool isHit = false;
    public GameObject[] hitBox;

    float currentTime;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
        {
            for (int i = 0; i < hitBox.Length; i++)
            {
                hitBox[i].SetActive(false);


            }
            
            currentTime += Time.deltaTime;
            if (currentTime > 0.4f)
            {
                for (int i = 0; i < hitBox.Length; i++)
                {
                    hitBox[i].SetActive(true);


                }
                currentTime = 0;
                isHit = false;
            }
        }

    }

}
