using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScripts : MonoBehaviour
{
    // Start is called before the first frame update
    Animation clip;
    void Start()
    {
        clip = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!clip.IsPlaying("platform_1"))
        {
            Destroy(gameObject);
        }
    }
}
