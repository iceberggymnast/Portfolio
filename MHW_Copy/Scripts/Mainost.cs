using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mainost : MonoBehaviour
{
    // ���� �ΰ� ĵ������ Ȱ��ȭ�Ǹ� ���� ���
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
