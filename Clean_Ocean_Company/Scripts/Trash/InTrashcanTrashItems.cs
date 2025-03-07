using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InTrashcanTrashItems : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    CoverAnimation coverAnimation;
    public int trashPoint;

    bool isDrag;

    GameObject door;

    //사운드
    AudioSet audioSet;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        coverAnimation = FindAnyObjectByType<CoverAnimation>();

        audioSet = GetComponent<AudioSet>();
    }

    // 이미지 드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        //쓰레기 선택 소리
        audioSet.OBJSFXPlay(0, false);


        // 드래그 시작 시 투명도를 낮추고 레이캐스트 비활성화
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // 드래그 시작할 때는 2D리지드바디 Body Type을 kinematic으로 바꿔주기
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        isDrag = true;
    }

    // 이미지 드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        // 마우스 위치에 맞춰 이미지 이동
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    // 이미지 드래그 끝날 때
    public void OnEndDrag(PointerEventData eventData)
    {
        //쓰레기 놓는 소리
        audioSet.OBJSFXPlay(1, false);

        // 드래그가 끝났을 때 다시 원래 상태로
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        isDrag = false;

        //쓰레기통 뚜껑 닫히기
        if (door != null)
        {
            door.transform.localEulerAngles = new Vector3(0, 0, 0);
            audioSet.OBJSFXPlay(3, false);

        }
    }

    // 트리거와 충돌할 때 호출되는 메서드
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("쓰레기 이미지와 충돌한 객체: " + other.name);

        if (other.CompareTag("Trashcan")) //쓰레기 이미지에 "Trashcan" 태그가 붙어있는지 확인
        {
            Debug.Log("쓰레기통과 충돌!");

            //쓰레기통 뚜껑 여는 소리
            audioSet.OBJSFXPlay(2, false);


            if (other.name == "silver")
            {
                coverAnimation.index = 0;
            }
            else if (other.name == "glass")
            {
                coverAnimation.index = 1;
            }
            else if(other.name == "plastic")
            {
                coverAnimation.index = 2;
            }
            else if(other.name == "clothes")
            {
                coverAnimation.index = 3;
            }

            //내 코드였는데 위에 처럼 해창님이 수정해주심
            ////쓰레기통 뚜껑 열리기
            ////for문으로 충돌한 쓰레기통 이미지 확인
            //GameObject trashCanGameObject = GameObject.Find("TrashCan");
            //string triggerTrashcanName = other.name;
            //print("열린 뚜껑종류:" + other.name);
            //for (int i = 0; i < trashCanGameObject.transform.childCount; i++)
            //{
            //    if (trashCanGameObject.transform.GetChild(i).name == other.name)
            //    {
            //        //뚜껑
            //        door = trashCanGameObject.transform.GetChild(i).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject;
            //        //충돌한 쓰레기통 오브젝트 뚜껑열기
            //        door.transform.localEulerAngles = new Vector3(90, 0, 0);
            //    }
            //}            

            // 쓰레기 수거 함수 호출
            Interaction_TrashBin trashBinScript = FindObjectOfType<Interaction_TrashBin>();
            if (!isDrag)
            {
                //포인트는 쓰레기 이미지에 TextMeshProUGUI의 text에 strint형으로 저장되어있으므로, 이걸 int로 변환해서 RecyclingGame의 point매개변수로 넣어줘야 함
                trashBinScript.RecyclingGame(gameObject.name, other.gameObject, gameObject, trashPoint);
            }
        }

    }

    // 트리거에서 벗어날 때 호출되는 메서드
    void OnTriggerExit2D(Collider2D other)
    {
        // 쓰레기통과 충돌이 끝났을 때 뚜껑 닫기 및 사운드
        coverAnimation.index = -1;
        audioSet.OBJSFXPlay(3, false);
    }
}
