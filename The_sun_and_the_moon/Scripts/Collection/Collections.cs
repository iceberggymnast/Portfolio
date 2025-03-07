//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Rendering.Universal;

//public class Collections : MonoBehaviour
//{
//    public bool isSelect = false;
//    CollectionMgr collectionMgr;
//    MeshCollider meshCollider;
//    MeshRenderer meshRenderer;

//    void Start()
//    {
//        collectionMgr = FindObjectOfType<CollectionMgr>();
//        meshCollider = GetComponent<MeshCollider>();
//        meshRenderer = GetComponent<MeshRenderer>();
//    }

//    void Update()
//    {
//    }

//    public void GetCollection()
//    {
//        isSelect = true;
//        //클릭된 수집품은 사라진다.
//        if (isSelect == true)
//        {
//            collectionMgr.CollectableCheck();
//            meshCollider.enabled = false;
//            meshRenderer.enabled = false;
//        }
//    }
//}
