using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MediapipeMoveTrigger : MonoBehaviour
{
    MediapipeTrashFactory trashFactory;

    public RectTransform checkImage;

    public List<GameObject> posList;

    GameObject currentPos;

    int currentPosIndex = 3;


    private void Start()
    {
        trashFactory = GameObject.FindWithTag("TrashParent").GetComponent<MediapipeTrashFactory>();

        for (int i = 1; i < 6; i++)
        {
            posList.Add(GameObject.Find("PosList").transform.GetChild(i).gameObject);
        }
    }

    public void SetCheckPos(int type)
    {
        if (type == 0)
        {
            if (currentPosIndex == 1)
            {
                return;
            }
            checkImage.localPosition = new Vector3(checkImage.localPosition.x - 50, checkImage.localPosition.y, checkImage.localPosition.z);
            currentPosIndex--;
            trashFactory.trashImage.SetActive(false);
            trashFactory.trashImage = trashFactory.trashImages[currentPosIndex - 1];
            trashFactory.trashImage.SetActive(true);
            trashFactory.trashPos = (MediapipeTrashFactory.TrashPos)currentPosIndex - 1;
            trashFactory.SetTrashEvent();
        }
        else if (type == 1)
        {
            if (currentPosIndex == 6)
            {
                return;
            }
            checkImage.localPosition = new Vector3(checkImage.localPosition.x + 50, checkImage.localPosition.y, checkImage.localPosition.z);
            currentPosIndex++;
            trashFactory.trashImage.SetActive(false);
            trashFactory.trashImage = trashFactory.trashImages[currentPosIndex - 1];
            trashFactory.trashImage.SetActive(true);
            trashFactory.trashPos = (MediapipeTrashFactory.TrashPos)currentPosIndex - 1;
            trashFactory.SetTrashEvent();
        }
    }
}
