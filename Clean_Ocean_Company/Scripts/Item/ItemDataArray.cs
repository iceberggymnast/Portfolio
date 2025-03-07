using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataArray : MonoBehaviour
{
    public static ItemDataArray itemDataInstance;

    [ArrayElementTitle("item_Name")]
    public List <ItemData> itemDatas;
    //public ItemData[] itemDatas;

    private void Awake()
    {
        if (itemDataInstance == null) 
        {
            itemDataInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
