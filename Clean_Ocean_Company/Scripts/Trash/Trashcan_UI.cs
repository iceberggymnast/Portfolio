using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Trashcan_UI : MonoBehaviour
{
    // 쓰레기통 캔버스에 있는 쓰레기통 이미지
    public RectTransform trashcan_image;

    // 한 행에 최대 몇 개의 이미지가 들어갈지 설정
    public int maxWidth = 5;

    // 최대 높이 설정
    public int maxHeight = 4;

    // 사용된 위치를 저장할 리스트
    List<Vector2> occupiedPositions = new List<Vector2>();

    // 쓰레기통의 크기
    private Vector2 trashcanSize = new Vector2(300, 400);

    // 쓰레기 프리팹 
    public GameObject trashPrefab;

    // 만들어질 위치
    public RectTransform factoryPos;

    public TextMeshProUGUI trashPercentageText;

    public GameObject o2chargerUI;

    private void Start()
    {
        if (PlayerInfo.instance.trashcan_UI  == null)
        {
            PlayerInfo.instance.trashcan_UI = this;
        }
    }

    public void AddTrashImage(Sprite trashSprite, string trashName, int trashPoint, string type)
    {
        GameObject go = Instantiate(trashPrefab, trashcan_image);
        RectTransform goRect = go.GetComponent<RectTransform>();
        goRect.anchoredPosition = factoryPos.anchoredPosition + new Vector2 ( 0, UnityEngine.Random.Range(-10.0f, 10.0f));
        go.name = trashName;
        Image imageComponent = go.GetComponent<Image>();
        imageComponent.sprite = trashSprite;

        go.GetComponent<InTrashcanTrashItems>().trashPoint = trashPoint;

        go.GetComponent<SpriteRenderer>().sprite = trashSprite;

        if(type == "oil")
        {
            //print("오일인거 확인");
            go.transform.localScale = new Vector3(1, 1, 1); // 스케일을 (1, 1, 1)로 설정
            StartCoroutine(FadeOutAndDestroy(go, imageComponent));
        }
    }

    private IEnumerator FadeOutAndDestroy(GameObject go, Image imageComponent)
    {
        float fadeDuration = 0.5f; // 0.5초 동안 서서히 사라짐
        float elapsedTime = 0f;
        Color originalColor = imageComponent.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            imageComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        //go.transform.localScale = new Vector3(3,3,3); // 스케일을 (1, 1, 1)로 설정
        //Debug.Log("이미지 삭제됨: " + go.name); // 삭제 전에 로그로 확인
        Destroy(go); // 0.5초 후 오브젝트 삭제

        // 삭제 후 확인
        yield return new WaitForSeconds(0.1f); // 잠시 대기 후 상태 점검
    }

    //// 다음에 쌓일 위치 계산
    //private Vector2 CalculateNextPosition()
    //{
    //    int currentColumn = occupiedPositions.Count % maxWidth; // 현재 열 계산
    //    int currentRow = occupiedPositions.Count / maxWidth; // 현재 행 계산

    //    // 아래에서부터 쌓이도록 위치 계산
    //    Vector2 nextPosition = new Vector2(
    //        (currentColumn * 75) - (trashcanSize.x / 2) + 35, // X 위치
    //        -(trashcanSize.y / 2) + (currentRow * 75) + 35  // Y 위치
    //    );

    //    // 최대 수용 가능 공간 확인
    //    int totalCapacity = maxWidth * maxHeight;

    //    // 포지션이 가득 차지 않았고, 경계 내에 있는지 확인
    //    if (occupiedPositions.Count < totalCapacity && IsInsideTrashcanBounds(nextPosition) && !IsOccupied(nextPosition))
    //    {
    //        return nextPosition; // 사용할 수 있는 다음 위치 반환
    //    }
    //    return Vector2.zero; // 사용할 수 없는 경우
    //}

    //// 랜덤한 위치에 쓰레기 이미지를 배치하는 함수
    //private void PlaceTrashRandomly(Sprite trashSprite, string trashName)
    //{
    //    GameObject TrashImage = new GameObject(trashName);
    //    TrashImage.transform.SetParent(trashcan_image, false);
    //    TrashImage.tag = "Trash";

    //    Image imageComponent = TrashImage.AddComponent<Image>();
    //    imageComponent.sprite = trashSprite;

    //    //TrashImage게임오브젝트에 CanvasGroup컴포넌트 추가해주기
    //    TrashImage.AddComponent<CanvasGroup>();

    //    TrashImage.AddComponent<InTrashcanTrashItems>();
    //    BoxCollider2D boxCollider2D = TrashImage.AddComponent<BoxCollider2D>();
    //    boxCollider2D.size = new Vector2(70, 70);
    //    boxCollider2D.isTrigger = true;

    //    RectTransform rectTransform = TrashImage.GetComponent<RectTransform>();
    //    rectTransform.sizeDelta = new Vector2(70, 70);

    //    // 랜덤한 위치 생성 (쓰레기통 내부)
    //    Vector2 randomPosition;
    //    do
    //    {
    //        randomPosition = new Vector2(
    //            Random.Range(-trashcanSize.x / 2 + 35, trashcanSize.x / 2 - 35), // X 위치
    //            Random.Range(-trashcanSize.y / 2 + 35, trashcanSize.y / 2 - 35)  // Y 위치
    //    );
    //    } while (IsOccupied(randomPosition));

    //    // 랜덤 위치에 이미지 배치
    //    rectTransform.anchoredPosition = randomPosition;
    //    occupiedPositions.Add(randomPosition); // 선택된 위치를 리스트에 추가
    //}

    //// 경계 내에 있는지 확인하는 함수
    //private bool IsInsideTrashcanBounds(Vector2 position)
    //{
    //    float halfWidth = trashcanSize.x / 2;
    //    float halfHeight = trashcanSize.y / 2;

    //    return position.x >= -halfWidth + 35 && position.x <= halfWidth - 35 &&
    //           position.y >= -halfHeight + 35 && position.y <= halfHeight - 35;
    //}

    //// 겹치는지 확인하는 함수
    //private bool IsOccupied(Vector2 position)
    //{
    //    foreach (Vector2 pos in occupiedPositions)
    //    {
    //        if (Vector2.Distance(pos, position) < 35) // 이미지 크기 절반보다 약간 큰 범위에서 겹치는지 확인
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}
