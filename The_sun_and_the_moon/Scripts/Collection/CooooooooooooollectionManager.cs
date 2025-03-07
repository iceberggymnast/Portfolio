using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.ObjectModel;


public class CooooooooooooollectionManager : MonoBehaviour
{
    //수집품 데이터를 관리할 리스트
    public List<CooooooooooooollectionData.CollectionInfo> collectionList = new List<CooooooooooooollectionData.CollectionInfo>();
    //수집품 박스를 관리
    public List<CooooooooooooollectionData.CollectionInfo> collectionBox = new List<CooooooooooooollectionData.CollectionInfo>();
    //수집품 데이터를 저장할 csv파일
    public TextAsset csvFile;

    //수집품 생길 position
    //public List<GameObject> CollPos;
    //수집품 생길 때 같이 생기는 파란 연기 이펙트
    public GameObject collectionEffect;

    //수집품 정보 팝업창 UI요소들
    public GameObject CollectionPopup;            //정보 팝업창
    public Image PopupCollectionImage;            //정보 팝업창의 수집품이미지
    public TextMeshProUGUI PopupCollectionName;               //정보 팝업창의 수집품이름
    public TextMeshProUGUI PopupCollectionSmallName;               //정보 팝업창의 수집품작은이름
    public TextMeshProUGUI PopupCollectionDescription;               //정보 팝업창의 수집품에 대한 설명

    //수집품 박스 UI요소들
    public GameObject CollectionBox;            //수집품 박스
    public List<Image> BoxCollectionImage;            //수집품 박스의 수집품이미지
    public List<TextMeshProUGUI> BoxCollectionName;               //수집품 박스의 수집품이름
    public List<TextMeshProUGUI> BoxCollectionSmallName;               //정보 팝업창의 수집품작은이름
                                                                           //public List<TextMeshProUGUI> BoxCollectiondescription;               //정보 팝업창의 수집품에 대한 설명

    PlayerState playerState;

    private void Awake()
    {
        ReadCSV();
    }


    void Start()
    {
        //ReadCSV();

        //팝업창 비활성화
        CollectionPopup.SetActive(false);
        //수집품 박스창도 비활성화
        CollectionBox.SetActive(false);

        if (QuestManager.questManager.myPlayer != null)
        {
            //커서 관련
            playerState = QuestManager.questManager.myPlayer.GetComponent<PlayerState>();

            playerState.CursorState();   //일단 꺼뒀는데 나중에 어떻게 시작하냐에 따라 달라질 수 있음
        }


    }

    void Update()
    {
        OpenCollectionBox();
    }

    //수집품을 클릭했을 때 호출될 함수(수집품 정보 팝업)
    [PunRPC]
    public void OnCollectionClick(int collectionID)
    {
        //수집품 ID로 데이터를 찾기
        CooooooooooooollectionData.CollectionInfo collection = CollectionRemember.col.collectionList.Find(item => item.CollectionID == collectionID);
        if(collection != null)
        {
            //팝업창 UI 업데이트
            PopupCollectionImage.sprite = collection.CollectionSprite;     // 수집품 이미지 설정
            PopupCollectionName.text = collection.CollectionName;           // 수집품 이름 설정
            PopupCollectionSmallName.text = collection.CollectionSmallName; // 수집품 작은 이름 설정
            PopupCollectionDescription.text = collection.CollectionDescription; // 팝업창에 설명 설정
            collection.isHaveCollection = true;

            //팝업창 활성화
            CollectionPopup.SetActive(true);

            //마우스 커서 뜨게 하기
            if (playerState != null)
            {
                playerState.isNPCtalking = true;
                playerState.isOpenUI = true;
            }
            else
            {
                playerState = QuestManager.questManager.myPlayer.GetComponent<PlayerState>();
                playerState.isNPCtalking = true;
                playerState.isOpenUI = true;
            }
        }
    }

    //수집품 클릭 시 호출되어 수집품 박스에 해당 수집품 정보를 저장해둘 함수(수집품 박스창)
    [PunRPC]
    public void OnCollectionClick_BoxUploading(int collectionID)
    {
        //수집품 ID로 데이터를 찾기
        CooooooooooooollectionData.CollectionInfo collection = CollectionRemember.col.collectionList.Find(item => item.CollectionID == collectionID);

        //수집품 이름이 같은 지부터 확인하고 정보 넣을 자리 찾기
        //BoxImage의 자식들의 번호가 수집품의 id와 동일한지 봐야함
        for (int i =0; i < CollectionBox.transform.GetChild(3).transform.GetChild(5).childCount; i++)
        {
            //클릭한 수집품의 아이디와 i가 같다면
            if(i == collectionID)
            {
                //i번째 자식 오브젝트 가져오기
                Transform child = CollectionBox.transform.GetChild(3).transform.GetChild(5).transform.GetChild(i);

                //i번째 자식 오브젝트에 get한 수집품 정보 넣어주기
                //1. Default이미지 비활성화 먼저 해주기
                GameObject DefaultImage = child.transform.GetChild(4).gameObject;
                DefaultImage.SetActive(false);
                //2. get한 수집품 이미지 넣어주기
                Image CollectionImage = child.transform.GetChild(1).GetComponent<Image>();
                CollectionImage.sprite = collection.CollectionSprite;
                //3. get한 수집품 이름 넣어주기
                TextMeshProUGUI CollectionName = child.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                CollectionName.text = collection.CollectionName;
                //4. get한 수집품 sub이름 넣어주기
                TextMeshProUGUI CollectionSmallname = child.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
                CollectionSmallname.text = collection.CollectionSmallName;
            }
        }
        
    }
    

    //C키 누르면 열리는 수집품 박스
    public void OpenCollectionBox()
    {
        //C키 누르면 수집품 박스 열린다. 대신 수집품 팝업 캔버스가 안열렸을 때만 열린다.
        if (!CollectionPopup.activeSelf && Input.GetKeyDown(KeyCode.C))
        {
            CollectionBox.gameObject.SetActive(true);

            for (int i = 0; i < CollectionRemember.col.collectionList.Count; i++)
            {
                if (CollectionRemember.col.collectionList[i].isHaveCollection)
                {
                    OnCollectionClick_BoxUploading(i);
                }
            }


            //마우스 커서 뜨게 하기
            if (playerState != null)
            {
                playerState.isOpenUI = true;
            }
            else
            {
                playerState = QuestManager.questManager.myPlayer.GetComponent<PlayerState>();
                playerState.isOpenUI = true;
            }
            print("수집품 박스 열림");
        }
        //수집품 박스가 열려있는 상태에서 C키 누르면 수집품 박스 받힌다.
        else if (CollectionBox.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CollectionBox.gameObject.SetActive(false);
                //마우스 커서 뜨게 하기
                if (playerState != null)
                {
                    playerState.isOpenUI = false;
                }
                else
                {
                    playerState = QuestManager.questManager.myPlayer.GetComponent<PlayerState>();
                    playerState.isOpenUI = false;
                }
            }
        }
    }



    //수집품 팝업창 닫기
    public void Exit_collectionPopup()
    {
        //팝업창 비활성화
        CollectionPopup.SetActive(false);

        // 마우스 커서 안뜨게 하기
        if (playerState != null)
        {
            playerState.isNPCtalking = false;
            playerState.isOpenUI = false;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //수집품 박스창 닫기
    public void Exit_collectionBox()
    {
        //수집품 박스창 비활성화
        CollectionBox.SetActive(false);

        // 마우스 커서 안뜨게 하기
        if (playerState != null)
        {
            playerState.isNPCtalking = false;
            playerState.isOpenUI = false;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    //수집품 정보를 csv파일을 읽어 데이터 리스트에 저장해놓는다.
    void ReadCSV()
    {
        StringReader reader = new StringReader(csvFile.text);
        bool isFirstLine = true;

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();

            //첫 줄이 헤더인 경우 넘어감
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }

            //쉼표로 구분된 데이터를 분리
            string[] values = line.Split(',');

            if (values.Length == 5)   //csv의 필드 수에 맞추어 수정
            {
                int collectionId = int.Parse(values[0]);
                string collectionName = values[1];
                string collectionSmallName = values[2];
                string collectionDescription = values[3].Replace("@","");
                string spriteName = values[4];

                //CollectionInfo 객체 생성 및 값 설정
                CooooooooooooollectionData.CollectionInfo collectionInfo= new CooooooooooooollectionData.CollectionInfo();
                collectionInfo.CollectionDataAdd(collectionId, collectionName, collectionSmallName, collectionDescription, spriteName);

                //스프라이트 이미지를 Resources폴더에서 불러오기
                Sprite sprite = Resources.Load<Sprite>("CeciliaSprites/" + spriteName);
                if(sprite != null)
                {
                    collectionInfo.CollectionSprite = sprite;
                }
                else
                {
                    print("스프라이트를 찾을 수 없음");
                }

                //리스트에 추가
                collectionList.Add(collectionInfo);
            }
        }
    }


}
