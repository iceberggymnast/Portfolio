using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


public class CooooooooooooollectionData : MonoBehaviour
{
    [Serializable]

    public class CollectionInfo
    {
        public int CollectionID;   //수집품의 id
        public string CollectionName;   //수집품의 이름
        public string CollectionSmallName;   //수집품의 부제
        public string CollectionDescription;   //수집품 설명(먹었을 때 뜨는 팝업용)
        public Sprite CollectionSprite;             //수집품 아이콘 그림 

        public GameObject collectionGameObject; // 게임 오브젝트 필드 추가

        public bool isHaveCollection;


        public void CollectionDataAdd(int id, string name, string smallname, string description, string spritename)
        {
            CollectionID = id;
            CollectionName = name;
            CollectionSmallName = smallname;
            CollectionDescription = description;

            //스프라이트 불러오기
            Sprite[] sprites = Resources.LoadAll<Sprite>("CeciliaSprites");
            foreach (Sprite sprite in sprites)
            {
                if(sprite.name == spritename)
                {
                    CollectionSprite = sprite;
                    break;
                }
            }
        }
    }
}
