using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mainost : MonoBehaviour
{
    // 메인 로고 캔버스가 활성화되면 음악 재생
    public GameObject mainlogo;
    bool play;
    void Start()
    {
        
    }

    void Update()
    {
        if (mainlogo.activeSelf)
        {
            if (!play)
            {
                gameObject.GetComponent<AudioSource>().Play();
                play = true;
            }
        }
    }
}
