using UnityEngine;
using UnityEngine.Events;

public class ObjectDetector : MonoBehaviour
{
    [System.Serializable]
    public class RaycastEvent : UnityEvent<Transform> { } // Event Class 정의 (등록되는 Event Method는 Transform 매개변수 1개를 가지는 Method)

    [HideInInspector]
    public RaycastEvent raycastEvent = new RaycastEvent(); // Event Class Instance 생성 및 Memory 할당

    private Camera maincamera; // 광선을 생성하기 위한 Camera
    private Ray ray; // 생성된 광선 정보 저장을 위한 Ray
    private RaycastHit hit; // 광선에 부딪힌 Object 정보 저장을 위한 RaycastHit

    private void Awake()
    {
        // MainCamera Tag를 가지고 있는 Object 탐색 후 Camera Component 정보 전달
        maincamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Mouse 왼쪽 버튼 클릭 할 경우
        {
            // Camera 위치에서 화면의 Mouse 위치를 관통하는 광선 생성
            // ray.origin: 광선(= Camera) 시작 위치
            // ray.direction: 광선의 진행 방향
            ray = maincamera.ScreenPointToRay(Input.mousePosition);

            // 2D 모니터를 통해 3D 월드의 Object를 Mouse로 선택하는 방법
            // 광선에 부딪히는 Object를 검출 해서 hit에 저장
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // 부딪힌 Object의 Transform 정보를 매개변수로 Event 호출
                raycastEvent.Invoke(hit.transform);
            }
        }
    }
}