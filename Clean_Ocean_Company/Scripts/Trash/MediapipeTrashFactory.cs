using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediapipeTrashFactory : MonoBehaviour
{
    public GameObject mediapipeTrash;
    public GameObject factoryPos;

    private int minX = 200;
    private int maxX = 1720;

    private int minY = 200;
    private int maxY = 880;

    public List<GameObject> trashImages;

    List<Dictionary<TrashPos, bool>> keyTrashList;

    public GameObject trashImage;


    public List<List<GameObject>> trashObjectsList;

    public enum TrashPos
    {
        forward, back, right, left, up, down
    }

    public TrashPos trashPos;

    public int maxTrasjCount = 0;



    private void Start()
    {
        trashObjectsList = new List<List<GameObject>>();

        keyTrashList = new List<Dictionary<TrashPos, bool>>();
        Dictionary<TrashPos, bool> keyTrash = new Dictionary<TrashPos, bool>();
        foreach (TrashPos pos in System.Enum.GetValues(typeof(TrashPos)))
        {
            keyTrash[pos] = false;


        }
        keyTrashList.Add(keyTrash);

        SetTrashEvent();
    }

    public void SetTrashEvent()
    {
        if (keyTrashList[0][trashPos] == true)
        {
            // 해당 위치에 이미 쓰레기가 생성되었으면 이벤트 건너뜀
            //Debug.Log($"{trashPos}에 이미 쓰레기가 생성되었습니다. 이벤트를 건너뜁니다.");
            return;
        }


        switch (trashPos)
        {
            case TrashPos.forward:
                SetRandomEvent(TrashPos.forward);

                break;

            case TrashPos.back:
                SetRandomEvent(TrashPos.back);

                break;

            case TrashPos.right:
                SetRandomEvent(TrashPos.right);

                break;

            case TrashPos.left:
                SetRandomEvent(TrashPos.left);

                break;

            case TrashPos.up:
                SetRandomEvent(TrashPos.up);

                break;

            case TrashPos.down:
                SetRandomEvent(TrashPos.down);

                break;
        }

        // 해당 위치에 쓰레기가 생성되었음을 표시
        keyTrashList[0][trashPos] = true;
    }

    void SetRandomEvent(TrashPos trashPos)
    {
        // 매번 새로운 trashObjects 리스트를 생성
        List<GameObject> trashObjects = new List<GameObject>();
        Dictionary<TrashPos, bool> keyTrash = new Dictionary<TrashPos, bool>();

        trashImage = trashImages[System.Convert.ToInt32(trashPos)];
        RectTransform rectTransform = trashImage.GetComponent<RectTransform>();

        

        

        minY = -(int)((rectTransform.sizeDelta.y - 200) / 2);
        maxY = (int)((rectTransform.sizeDelta.y - 200) / 2);

        int objectCount = 0;

        switch (trashPos)
        {
            case TrashPos.forward:
                minX = -(int)((rectTransform.sizeDelta.x - 500) / 2);
                maxX = (int)((rectTransform.sizeDelta.x - 500) / 2);
                objectCount = 4;
                break;

            case TrashPos.back:
                minX = -(int)((rectTransform.sizeDelta.x - 500) / 2);
                maxX = (int)((rectTransform.sizeDelta.x - 500) / 2);
                objectCount = 4;
                break;

            case TrashPos.right:
                minX = -(int)((rectTransform.sizeDelta.x - 200) / 2);
                maxX = (int)((rectTransform.sizeDelta.x - 200) / 2);
                objectCount = 6;
                break;

            case TrashPos.left:
                minX = -(int)((rectTransform.sizeDelta.x - 200) / 2);
                maxX = (int)((rectTransform.sizeDelta.x - 200) / 2);
                objectCount = 6;
                break;

            case TrashPos.up:
                minX = -(int)((rectTransform.sizeDelta.x - 200) / 2);
                maxX = (int)((rectTransform.sizeDelta.x - 200) / 2);
                objectCount = 6;
                break;

            case TrashPos.down:
                minX = -(int)((rectTransform.sizeDelta.x - 200) / 2);
                maxX = (int)((rectTransform.sizeDelta.x - 200) / 2);
                objectCount = 6;
                break;
        }

        

        for (int i = 0; i < objectCount; i++)
        {
            trashObjects.Add(FactoryTrash(minX, maxX, minY, maxY, trashImage));
        }

        trashObjectsList.Add(trashObjects);
    }

    GameObject FactoryTrash(int minX, int maxX, int minY, int maxY, GameObject image)
    {
        int x = Random.Range(minX, maxX);
        int y = Random.Range(minY, maxY);

        Vector3 dir = new Vector3(x, y, 0);
        GameObject trash = Instantiate(mediapipeTrash, new Vector3(x, y, 0), Quaternion.identity);
        trash.transform.SetParent(image.transform, false);

        return trash;
    }

}
