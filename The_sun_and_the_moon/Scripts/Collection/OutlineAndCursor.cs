using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;

public class OutlineAndCursor : MonoBehaviour
{
    //[Header("월드에 있는 수집품들")]
    //수집품 리스트
    //public List<GameObject> Collectionslist;

    //Main Canvas
    public Canvas canvas;

    //커서 껐다가 켜기 위해서 가져옴
    PlayerState playerState;

    //[Header("수집품 정보 띄워주는 캔버스")]
    ////수집품 정보 띄워주는 캔버스
    //public GameObject InfoCanvas;
    //public Image Img_pickedCollection;
    //public TextMeshProUGUI Name_pickedCollection;
    //public TextMeshProUGUI Info01_pickedCollection;
    //public TextMeshProUGUI Info02_pickedCollection;

    ////bool isOpenInfoCanvas = false;
    //[Header("수집품 박스")]
    ////각 수집품의 이름 리스트
    //public List<TextMeshProUGUI> CollectionsName;
    ////각 수집품의 설명 리스트
    //public List<TextMeshProUGUI> CollectionsInfo_01;  //큰 글씨 설명
    //public List<TextMeshProUGUI> CollectionsInfo_02;  //작은 글씨 설명
    ////각 수집품의 이미지 리스트
    //public List<Image> CollectionsImg;
    ////아직 못찾은 수집품 이미지는 물음표 이미지로
    //public List<Image> questionMark_img;

    //C키 눌렀을 때 수집품 박스 켜지고 한번 더 누르면 수집품 박스 꺼짐
    //bool isOpenCollectionBox = false;

    //수집품에 마우스 커서 뒀을 때 수집품 이름 나오게 하는 함수에 필요한 변수들
    public Camera Camera;
    public TextMeshProUGUI Name_UI;
    //public Canvas CollectionBox;
    GameObject lastSelectedCollection;

    //외곽선 맞은 오브젝트에 있는 Outline
    Outline outline;

    void Start()
    {
        // Player 생성 시 Player의 자식인 Camera를 찾아서 사용
        if (QuestManager.questManager.myPlayer != null)
        {
            Transform playerTransform = QuestManager.questManager.myPlayer.transform;

            // playerTransform에서 Camera를 찾아 할당
            Camera = playerTransform.GetComponentInChildren<Camera>();

            if (Camera == null)
            {
                Debug.LogError("Player의 자식으로 카메라가 존재하지 않습니다.");
            }
        }

        // UI 상태 설정
        playerState = QuestManager.questManager.myPlayer.GetComponent<PlayerState>();


        //처음에 UI켜졌는지 확인하고, 커서 킬지 말지 결정
        if (playerState != null)
        {

        playerState.CursorState();   //일단 꺼뒀는데 나중에 어떻게 시작하냐에 따라 달라질 수 있음
        }

        ////처음에는 수집품 정보창을 비활성화한다.
        //InfoCanvas.SetActive(false);

        ////처음에는 수집품 박스를 비활성화한다.
        //CollectionBox.gameObject.SetActive(false);

        ////수집품 리스트의 이름을 텍스트로 설정한다.
        //for (int i=0;i< Collectionslist.Count && i < CollectionsName.Count; i++)
        //{
        //    CollectionsName[i].text = Collectionslist[i].name;
        //}

        ////처음에는 수집이 된 수집품이 없으므로 수집품 이름을 반투명하게 표시한다.
        //for (int i=0; i < CollectionsName.Count; i++)
        //{
        //    SetTextAlpha(CollectionsName[i], 0.5f);
        //    //수집품 설명1, 설명2, 이미지는 비활성화한다.
        //    SetTextAlpha(CollectionsInfo_01[i], 0.0f);
        //    //CollectionsInfo_01[i].gameObject.SetActive(false);
        //    SetTextAlpha(CollectionsInfo_02[i], 0.0f);
        //    //CollectionsInfo_02[i].gameObject.SetActive(false);
        //    //Image CollectionsImage = CollectionsImg[i].GetComponent<Image>();
        //    SetImageAlpha(CollectionsImg[i], 0.0f);
        //    SetImageAlpha(questionMark_img[i], 1.0f); // 기본적으로 물음표 이미지는 활성화 상태로 설정
        //}
        ////선택된 수집품인지 확인한다.
        //CollectableCheck();
    }

    

    void Update()
    {
        if (playerState == null)
        {
            if (QuestManager.questManager.myPlayer != null)
            {
                playerState = QuestManager.questManager.myPlayer.GetComponent<PlayerState>();
            }
        }

        if (!playerState.isOpenUI)
        {
            SeeCollectionName();
        }
        //OpenOrCloseCollectionBox();
    }

    //특정 오브젝트 위 커서가 올라가면 이름이나 외곽선을 표시하는 함수
    public void SeeCollectionName()
    {
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;

        if (Physics.Raycast(ray, out hitinfo))
        {
            // 이전에 선택된 오브젝트와 현재 레이캐스트로 맞춘 오브젝트가 다르면 외곽선 초기화
            if (lastSelectedCollection != hitinfo.collider.gameObject && lastSelectedCollection != null)
            {
                Outline lastOutline = lastSelectedCollection.GetComponent<Outline>();
                
                if (lastOutline != null)
                {
                    DoorOpen doorOpen = lastSelectedCollection.transform.gameObject.GetComponent<DoorOpen>();
                    if (doorOpen != null && doorOpen.locked) return;

                    //외곽선 컬러도 초록색으로 초기화
                    Color DefaultColor = new Color(76/255, 255/255, 0);
                    outline.OutlineColor = DefaultColor;
                    lastOutline.UpdateMaterialProperties();
                    Debug.Log("이전 외곽선을 초기화");
                }
                lastSelectedCollection = null;

            }

            //아이템이나, NPC나, 문에 커서를 두면 NPC 오브젝트 외곽선 활성화
            if (hitinfo.collider.gameObject.layer == 9 || hitinfo.collider.gameObject.layer == 10 || hitinfo.collider.gameObject.layer == 11)
            {
                //아이템이던, npc던, 문 오브젝트던 외곽선 활성화하는 건 공통으로 하되.
                outline = hitinfo.collider.gameObject.GetComponent<Outline>();
                DoorOpen doorOpen = hitinfo.collider.gameObject.GetComponent<DoorOpen>();
                //if (doorOpen.locked) return;
                if (outline)
                {
                    //커서에 닿으면 외곽선 두께를 8로 조정
                    outline.outlineWidth = 8.0f;
                    //커서에 닿으면 외곽선 컬러 노란색으로 바꿔주기
                    outline.OutlineColor = Color.yellow;
                    outline.UpdateMaterialProperties();
                    print("외곽선 적용");
                    lastSelectedCollection = hitinfo.collider.gameObject;
                    

                }
                else
                {
                    print("맞은 NPC에 outline컴포넌트 없음");
                }
                //아이템이면 이름 UI띄워주기
                if(hitinfo.collider.gameObject.layer == 9)
                {
                    //레이를 맞은 위치의 아이템에 붙은 DropItem 스크립트를 가져와서 그 스크립트내의 itemID를 itemManager의 itemList의 id와 비교하여 일차하는 item의 itemName을 띄운다.
                    ItemManager itemManager = FindObjectOfType<ItemManager>();
                    GameObject RayItem = hitinfo.collider.gameObject;
                    Name_UI.text = itemManager.itemList[hitinfo.transform.GetComponent<DropItem>().itemID].itemName;
                    Name_UI.gameObject.SetActive(true);

                    //오브젝트의 중앙 위치를 구하기
                    Vector3 ItemCenter = hitinfo.collider.gameObject.GetComponent<Renderer>().bounds.center;
                    ItemCenter.y += 0.1f;
                    //월드 위치를 스크린 좌표로 변환
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(ItemCenter);
                    //스크린 좌표를 캔버스 로컬 좌표로 변환
                    Vector2 canvasPosition;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPosition, canvas.worldCamera, out canvasPosition);

                    Name_UI.rectTransform.anchoredPosition = canvasPosition;

                    lastSelectedCollection = RayItem;
                }
            }
            else
            {
                //레이에 맞았는데 수집품 레이어에 속하지 않는 경우, 텍스트 비활성화
                Name_UI.text = "";
                // 레이가 맞지 않았을 때도 외곽선 끔
                if (lastSelectedCollection != null)
                {
                    Outline lastOutline = lastSelectedCollection.GetComponent<Outline>();
                    if (lastOutline != null)
                    {
                        //외곽선 컬러도 초록색으로 초기화
                        Color DefaultColor = new Color(76 / 255, 255 / 255, 0);
                        outline.OutlineColor = DefaultColor;
                        lastOutline.UpdateMaterialProperties();
                        Debug.Log("외곽선을 초기화했습니다.");
                    }
                    lastSelectedCollection = null;

                }
            }
        }
        else
        {
            Name_UI.text = "";
        }
    }
}
