using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooooooooooooollection : MonoBehaviour
{
    //수집품을 y축으로 회전시키는 변수들
    public int CollectionID;
    public float rotationSpeed = 50.0f;


    private void OnEnable()
    {
        
    }

    void Update()
    {
        //y축을 기준으로 오브젝트 회전
        transform.Rotate(0,rotationSpeed*Time.deltaTime,0);

        // 수집 여부 판단해서 제거
        if (CollectionRemember.col != null)
        {
            if (CollectionRemember.col.collectionList[CollectionID].isHaveCollection)
            {
                Destroy(gameObject);
            }
        }
    }


}
