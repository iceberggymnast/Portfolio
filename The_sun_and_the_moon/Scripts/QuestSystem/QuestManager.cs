using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuestManager : MonoBehaviourPunCallbacks
{
    // 싱글턴 퀘스트 매니저
    public static QuestManager questManager;

    // 퀘스트 데이터를 관리할 리스트
    public List<QuestData.QuestInfo> questList = new List<QuestData.QuestInfo>();

    // 진행 가능한 퀘스트 리스트
    public List<QuestData.QuestInfo> QuestcanReceive = new List<QuestData.QuestInfo>();

    // 진행중이나 완료한 퀘스트를 관리할 리스트
    public List<QuestData.QuestInfo> questProgress = new List<QuestData.QuestInfo>();
    public List<QuestData.QuestInfo> questComplete = new List<QuestData.QuestInfo>();

    // 퀘스트 데이터를 저장할 CSV 파일
    public TextAsset csvFile;

    // 아이템 매니져 컴포넌트
    public ItemManager itemManager;

    // 다이얼로그 시스템
    public DialogSystem dialogSystem;

    // 클라이언트가 조작하는 player 모델링
    public GameObject myPlayer;

    // Npc 오브젝트를 저장해놓은 리스트
    public List<GameObject> npcList;

    // 싱글턴 패턴 구현
    private void Awake()
    {
        transform.parent = null;
        if (questManager == null)
        {
            questManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (questManager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 시작할때 CSV 파일을 읽어옴
        ReadCSV();
        // 아이템 매니져 컴포넌트 가져오기
        itemManager = GetComponent<ItemManager>();
        //다이얼 로그 시스템 받아오기
        dialogSystem = GetComponent<DialogSystem>();
        // 퀘스트를 받을 수 있는지 체크
        QuestCanAccept();
    }

    // 퀘스트 수락을 위한 함수 (호출해서 쓰세요)
    [PunRPC]
    public void QuestAccept(int questId)
    {
        // 퀘스트가 이미 진행중이거나 완료했는지 확인함 
        for (int i = 0; i < questProgress.Count; i++)
        {
            if (questProgress[i].questId == questId)
            {
                print("퀘스트가 이미 진행중인 상태입니다.");
                return;
            }
        }

        for (int i = 0; i < questComplete.Count; i++)
        {
            if (questComplete[i].questId == questId)
            {
                print("퀘스트가 이미 완료한 상태입니다.");
                return;
            }
        }

        // 해당 퀘스트가 받을 수 없는 상태인지 체크
        if (questList[questId].questState == QuestData.QuestState.cantReceived)
        {
            print("해당 퀘스트를 받을 조건이 충족되지 않았습니다.");
            return;
        }

        // questProgress리스트에 해당 퀘스트를 추가함
        questProgress.Add(questList[questId]);
        QuestcanReceive.Remove(questList[questId]);
        // 추가한 퀘스트의 상태를 progress로 변경함
        questProgress[questProgress.Count - 1].questState = QuestData.QuestState.progress;
        QuestProgressItemCheck();
        print(questProgress[questProgress.Count - 1].questName + "을 수락하였습니다.");
    }

    [PunRPC]
    // 아이템이 필요하지 않은 미션의 목표진행도 추가 (호출해서 쓰세요)
    public void QuestAddProgress(int questId, int objectiveCount, int processCount)
    {
        for(int i = 0; i < questProgress.Count; i++)
        {
            if (questProgress[i].questId == questId)
            {
                questProgress[i].objectives[objectiveCount].currentobjectivesNumber += processCount;
                QuestProgressCheck();
                break;
            }
        }
    }

    // 아이템이 필요한 미션의 아이템 갯수 추적
    public void QuestProgressItemCheck()
    {
        // 퀘스트들 중 아이템이 필요한 퀘스트들을 확인한다.
        for(int i = 0; i < questProgress.Count; i++) // 진행중인 퀘스트 리스트 i에 대한 for문
        {
            for (int j = 0; j < questProgress[i].objectives.Count; j++) // 퀘스트 i 번째에서의 목표 j 번째에 대한 for 문
            {
                // 해당 클래스의 아이템 id가 있는지 확인
                for (int  k = 0; k < itemManager.itemInventory.Count; k++)
                {
                    // 아이템이 필요한 퀘스트 목표이면서 해당 아이템을 가지고 있을 때
                    if (questProgress[i].objectives[j].itemRequire && questProgress[i].objectives[j].itemId == itemManager.itemInventory[k].itemId)
                    {
                        // 갯수를 비교하여 퀘스트의 목표 수집수랑 동기화를 시켜준다.
                        questProgress[i].objectives[j].currentobjectivesNumber = itemManager.itemInventory[k].itemHaveNum;
                        break;
                    }
                }
            }
        }
        // 체크가 끝나면 완료 여부를 확인한다.
        QuestProgressCheck();
    }

    // 퀘스트를 받을 수 있는 상태인지 체크
    public void QuestCanAccept()
    {
        // requireId의 값이 0이면 무조건 수락 가능한 상태
        for (int i = 0;i < questList.Count;i++)
        {
            if(questList[i].requireId == 0)
            {
                if (questList[i].questState == QuestData.QuestState.cantReceived)
                {
                    QuestcanReceive.Add(questList[i]);
                    questList[i].questState = QuestData.QuestState.canReceived;
                }
            }
        }

        // 선행 퀘스트를 깻다면 수락 가능한 상태로 변경
        for (int i = 0; i < questList.Count; i++)
        {
            for(int j = 0; j < questComplete.Count;  j++)
            {
                if (questList[i].requireId == questComplete[j].questId)
                {
                    if (questList[i].questState == QuestData.QuestState.cantReceived)
                    {
                        QuestcanReceive.Add(questList[i]);
                        questList[i].questState = QuestData.QuestState.canReceived;
                    }
                }
            }
        }
    }

    // 퀘스트의 진행도를 체크하여 완료 가능한지 여부 판단
    public void QuestProgressCheck()
    {

        for (int i = 0;i < questProgress.Count;i++)
        {
            // 해당 퀘스트가 진행중이거나 보고 가능한 상태인지 확인한다.
            if (questProgress[i].questState == QuestData.QuestState.progress || questProgress[i].questState == QuestData.QuestState.canBeCompleted)
            {
                int progress = 0; // 목표의 완료 갯수
                // 해당 퀘스트의 목표의 갯수만큼 체크
                for (int j = 0;j < questProgress[i].objectives.Count; j++)
                {
                    // 해당 목표의 완료 갯수를 체크
                    if(questProgress[i].objectives[j].currentobjectivesNumber >= questProgress[i].objectives[j].requireNumber)
                    {
                        progress++;
                    }
                }
                // 목표 완료 갯수를 체크 한 다음 progress의 값과 objectives의 갯수랑 같다면 완료 가능한 상태
                if (questProgress[i].objectives.Count == progress)
                {
                    questProgress[i].questState = QuestData.QuestState.canBeCompleted;
                    print(questProgress[i].questName + "의 퀘스트는 완료 가능한 상태입니다");
                }
                else
                {
                    questProgress[i].questState = QuestData.QuestState.progress;
                }
            }
        }
    }

    [PunRPC]
    // 퀘스트를 완료할 때 호출하는 함수 (호출해서 쓰세요)
    public bool QuestClear(int questId)
    {
        // 해당 퀘스트가 완료 가능한 상태인지 확인한다.
        if (questList[questId].questState == QuestData.QuestState.canBeCompleted)
        {
            // 해당 퀘스트를 찾는다
            for(int i = 0; i < questProgress.Count; i++)
            {
                if (questProgress[i].questId == questId)
                {
                    // 아이템을 요구했던 퀘스트라면 인벤토리에서 아이템을 해당 갯수만큼 제거한다.
                    for(int j = 0; j < questProgress[i].objectives.Count; j++)
                    {
                        if (questProgress[i].objectives[j].itemRequire)
                        {
                            itemManager.ItemRemove(questProgress[i].objectives[j].itemId, questProgress[i].objectives[j].requireNumber);
                        }
                    }
                    // 보상을 지급한다.
                    for (int j = 0; j < questProgress[i].reward.Count; j++)
                    {
                        itemManager.ItemAdd(questProgress[i].reward[j].itemId, questProgress[i].reward[j].rewardNumber);
                    }

                    // 클리어로 변경한다.
                    questProgress[i].questState = QuestData.QuestState.completed;
                    // 클리어 리스트로 이동시킨다.
                    questComplete.Add(questProgress[i]);
                    // 진행중인 리스트에서 제거한다.
                    questProgress.RemoveAt(i);
                    // 받을 수 있는 퀘스트가 있는지 체크
                    QuestCanAccept();
                    return true;
                }
            }
        }
        else
        {
            print(questList[questId].questName + "은 완료 불가능한 상태이거나 이미 클리어 하였습니다.");
            return false;
        }
        return false;
    }

    // CSV 파일을 읽어와서 questList에 불러옴
    void ReadCSV()
    {
        StringReader reader = new StringReader(csvFile.text);
        bool isFirstLine = true;

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();

            // 첫 줄이 헤더인 경우 넘어감
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }

            // 쉼표로 구분된 데이터를 분리
            string[] values = line.Split(',');

            if (values.Length >= 11) // CSV의 필드 수에 맞추어 수정
            {
                int questId = int.Parse(values[0]); // 퀘스트의ID
                string questName = values[1]; // 퀘스트 제목
                string questDescription = values[2]; // 퀘스트 설명
                int npcid = int.Parse(values[3]); // 퀘스트를 받을 수 있는 NPC의ID
                int requireId = int.Parse(values[4]); // 선행 퀘스트의 ID
                string isNeedItem = values[5]; // 아이템필요여부
                string itemid = values[6]; // 아이템ID
                string needItemnum = values[7]; // 아이템요구갯수
                string rewardid = values[8]; // 퀘스트보상아이템id
                string ewardnumber = values[9]; // 보상갯수
                string shortDes = values[10];

                // QuestInfo 객체 생성 및 값 설정
                QuestData.QuestInfo questInfo = new QuestData.QuestInfo();
                questInfo.QuestAdd(questId, questName, questDescription, npcid, requireId, isNeedItem, itemid, needItemnum, rewardid, ewardnumber, shortDes);

                // 리스트에 추가
                questList.Add(questInfo);
            }
        }
    }
}
