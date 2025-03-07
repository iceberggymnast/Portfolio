using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemJsonReader : MonoBehaviour
{
    public List<TextAsset> itemDataJson;

    private void Start()
    {
        string json = itemDataJson[0].text;
        ItemDataArray.itemDataInstance = JsonUtility.FromJson<ItemDataArray>(json);

        /*for (int i = 0; i < itemDataJson.Count; i++)  
        {
            string json = itemDataJson[i].text;
            ItemData itemData = JsonUtility.FromJson<ItemData>(json);
            ItemDataArray.itemDataInstance.itemDatas.Add(itemData);
        }*/
    }

}
