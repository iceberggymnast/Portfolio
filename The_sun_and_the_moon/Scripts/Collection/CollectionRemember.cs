using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionRemember : MonoBehaviour
{
    public static CollectionRemember col;

    public List<CooooooooooooollectionData.CollectionInfo> collectionList;
    public CooooooooooooollectionManager colMGR;

    private void Awake()
    {
        if(col == null)
        {
            col = this;
            DontDestroyOnLoad(col);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (colMGR == null) return;
        if (collectionList.Count == 0)
        {
            collectionList = new List<CooooooooooooollectionData.CollectionInfo>(colMGR.collectionList);
        }
    }

}
