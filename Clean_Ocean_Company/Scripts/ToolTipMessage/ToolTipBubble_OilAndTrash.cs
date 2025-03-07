using UnityEngine;

public class ToolTipBubble_OilAndTrash : MonoBehaviour
{
    private Vector3 lastPosition; // 오브젝트의 이전 위치를 저장
    public GameObject ToolTipCanvas; // 자식으로 있는 Canvas 컴포넌트를 참조

    void Start()
    {
        // 시작 시 오브젝트의 위치 저장
        lastPosition = transform.position;
    }

    void Update()
    {
        // 현재 위치와 이전 위치를 비교
        if (transform.position != lastPosition)
        {
            // 오브젝트가 움직였으면
            if (ToolTipCanvas != null)
            {
                // 자식 Canvas 삭제
                Destroy(ToolTipCanvas.gameObject);
            }
        }
    }
}

