using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NaviManager : MonoBehaviour
{
    // 컴포넌트들
    public LineRenderer mLineRenderer; // 경로를 시각적으로 보여줄 라인 렌더러
    public List<NavMeshAgent> mNavAgent;     // 네브 메시 에이전트, 자동으로 경로를 계산해 이동

    //각 에이전트틀이 있는 장소
    public int mNavAgent_PosNum;

    // 네비게이션 관련 변수들
    private Vector3 mTargetPos;         // 목표 위치

    //경로를 저장할 변수
    NavMeshPath navMeshPath;

    public float delay = 1.0f;

    private void Start()
    {
        mTargetPos = Vector3.zero;
        navMeshPath = new NavMeshPath();

        //시작할 때 라인을 그려주고
        StartCoroutine(UpdateNavi(delay));
    }
    private void Update()
    {
        //어머니 퀘스트(1,2,3)
        //퀘스트 1: 어머니랑 대화
        //퀘스트 2: 떡 재료 구해오기
        //퀘스트 3: 어머니 안마게임
        //퀘스트 1번이 완료가능하거나, 진행중일 때 | 퀘스트 2번이 완료가능하거나, 받을 수 있을 때 | 퀘스트 3번이 완료가능하거나, 진행중이거나, 퀘스트를 받을 수 있을 때
        if (QuestManager.questManager.questList[1].questState == QuestData.QuestState.canBeCompleted | QuestManager.questManager.questList[1].questState == QuestData.QuestState.progress 
            | QuestManager.questManager.questList[2].questState == QuestData.QuestState.canBeCompleted | QuestManager.questManager.questList[2].questState == QuestData.QuestState.canReceived
            | QuestManager.questManager.questList[2].questState == QuestData.QuestState.canBeCompleted | QuestManager.questManager.questList[2].questState == QuestData.QuestState.canReceived
            | QuestManager.questManager.questList[3].questState == QuestData.QuestState.canBeCompleted | QuestManager.questManager.questList[3].questState == QuestData.QuestState.progress | QuestManager.questManager.questList[3].questState == QuestData.QuestState.canReceived)
            {
                //mTargetPos를 어머니에게로 설정
                mNavAgent_PosNum = 0;
            }
        //퀘스트 2번이 진행중일 때 - 어머니 떡 만들 재료 구해오기
        else if(QuestManager.questManager.questList[2].questState == QuestData.QuestState.progress)
        {
            //햇님이에게 보여줄 라인랜더러 타겟 위치는 시장으로
            if(PhotonNetwork.NickName == "해님")
            {
                //mTargetPos를 시장으로 지정
                mNavAgent_PosNum = 3;
            }
            //달님이에게 보여줄 라인랜더러 타겟 위치는 집에 있는 아이템들도(창고, 부엌)
            if(PhotonNetwork.NickName == "달님")
            {
                //마당에 있는 팥 위치 쪽으로 라인랜더러 그리는데, 필요한 팥 갯수를 비교해서 덜 모았으면 계속 해당 아이템 쪽으로 라인랜더러 그리고
                if (QuestManager.questManager.questList[2].objectives[3].currentobjectivesNumber < QuestManager.questManager.questList[2].objectives[3].requireNumber)
                {
                    //mTargetPos를 마당으로 지정
                    mNavAgent_PosNum = 4;
                }
                //부엌에 있는 기름병이랑 쌀 쪽으로 라인랜더러 그리는데, 필요한 기름병이랑 쌀 갯수를 비교해서 덜 모았으면 계속 해당 아이템쪽으로 라인랜더러 그리기
                //objectives[2] = 쌀 모아오는 거(쌀 아이디는 3), objectives[4] = 기름병 모아오는 거(기름병 아이디는 5)
                if (QuestManager.questManager.questList[2].objectives[2].currentobjectivesNumber < QuestManager.questManager.questList[2].objectives[3].requireNumber
                    | QuestManager.questManager.questList[2].objectives[4].currentobjectivesNumber < QuestManager.questManager.questList[2].objectives[4].requireNumber)
                {
                    //targetPos를 부엌으로 지정
                    mNavAgent_PosNum = 5;
                }
            }
        }
        else if (QuestManager.questManager.questList[4].questState == QuestData.QuestState.progress)
        {
            mNavAgent_PosNum = 1;
        }
        else if (QuestManager.questManager.questList[6].questState == QuestData.QuestState.progress)
        {
            mNavAgent_PosNum = 2;   
        }
        //그렇지 않다면(나머지 퀘스트에는 가이드라인 안 그린다.)
        else
        {
            //경로를 안 그려준다.
            mLineRenderer.positionCount = 0;
            return;
        }

        InitNaviManager(0.1f);
    }

    // 네비게이션 관리자 초기화 함수
    public void InitNaviManager(float updateDelay)
    {
        // 시작 위치와 목적지 설정
        SetDestination();
    }

    // 일정 시간마다 경로를 업데이트하는 코루틴
    private IEnumerator UpdateNavi(float updateDelay)
    {
        // 지연 시간을 설정
        while (true)
        {

            // 목표 위치로 이동 경로 설정
            mNavAgent[mNavAgent_PosNum].CalculatePath(mTargetPos, navMeshPath);

            // 경로를 따라 라인을 그림
            DrawPath();

            // 지정된 시간만큼 대기
            yield return new WaitForSeconds(updateDelay);
        }
    }

    // 목적지 설정 함수
    public void SetDestination()
    {
        mTargetPos = QuestManager.questManager.myPlayer.transform.position;
    }


    // 경로를 따라 라인을 그리는 함수
    private void DrawPath()
    {
        // 라인 렌더러의 포지션 개수를 에이전트의 경로 코너 개수로 설정
        mLineRenderer.positionCount = navMeshPath.corners.Length + 2;
        mLineRenderer.SetPosition(0, mNavAgent[mNavAgent_PosNum].transform.position);  // 첫 번째 포지션은 현재 위치

        // 경로에 코너가 2개 미만일 경우 경로를 그릴 필요가 없음
        /*if (navMeshPath.corners.Length < 2)
        {
            return;
        }*/

        // 각 코너를 따라 라인을 그리기
        for (int i = 0; i < navMeshPath.corners.Length; i++)
        {
            mLineRenderer.SetPosition(i + 1, navMeshPath.corners[i]);
        }

        mLineRenderer.SetPosition(mLineRenderer.positionCount - 1, mTargetPos);
    }
}
