using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjScrollLock : MonoBehaviour
{
    public Vector3 minvec3;
    public Vector3 maxvec3;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = transform.localPosition.x;
        x = Mathf.Clamp(x, minvec3.x, maxvec3.x);
        float y = transform.localPosition.y;
        y = Mathf.Clamp(y, minvec3.y, maxvec3.y);
        float z = transform.localPosition.z;
        z = Mathf.Clamp(z, minvec3.z, maxvec3.z);

        transform.localPosition = new Vector3(x, y, z);
    }   
}
