using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t : MonoBehaviour
{
    public GameObject aa;
    public GameObject bb;
    public GameObject cc;

    // Start is called before the first frame update
    void Start()
    {
        int amount1 = aa.GetComponent<TrashSpawnerBase>().maxTrashAmout;
        int amount2 = bb.GetComponent<TrashSpawnerBase>().maxTrashAmout;
        int amount3 = cc.GetComponent<TrashSpawnerBase>().maxTrashAmout;

        int result = amount1 + amount2 + amount3;
        print(result);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
