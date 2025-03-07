using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger : MonoBehaviour
{
    public DoorOpen door;
    public AudioSource BGM; // 호랑이가 문을 열고 나올 때 재생되는 BGM

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoorOpen()
    {
        if(door != null)
        {
            door.isOpen = true;
            BGM.Play();

        }
    }
}
