using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoseName
{
    nose,
    left_eye_inner,
    left_eye,
    left_eye_outer,
    right_eye_inner,
    right_eye,
    right_eye_outer,
    left_ear,
    right_ear,
    mouth_left,
    mouth_right,
    left_shoulder,
    right_shoulder,
    left_elbow,
    right_elbow,
    left_wrist,
    right_wrist,
    left_pinky,
    right_pinky,
    left_index,
    right_index,
    left_thumb,
    right_thumb,
    left_hip,
    right_hip,
    left_knee,
    right_knee,
    left_ankle,
    right_ankle,
    left_heel,
    right_heel,
    left_foot_index,
    right_foot_index,

    pose_max
}

public class MediaPipeData : MonoBehaviour
{
    // point Prefab
    public GameObject pointFactory;

    // UDPServer
    public UDPServer udpServer;

    // 모든 point 담을 변수
    public Transform[] allPoints;

    void Start()
    {
        // allPoints 를 몇 개 담을지
        allPoints = new Transform[(int)PoseName.pose_max];

        // point를 pos_max개 만들자.
        for (int i = 0; i < (int)PoseName.pose_max; i++)
        {
            GameObject point = Instantiate(pointFactory);
            // 만들어진 point를 나의 자식으로 하자.
            point.transform.parent = transform;
            // 만들어진 point의 이름을 해당 되는 PoseName 으로 변경
            point.name = ((PoseName)i).ToString();
            // 만들어진 point의 transform을 allPoints에 담는다.
            allPoints[i] = point.transform;
        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    for (int i = 0; i < (int)PoseName.pose_max; i++)
        //    {
        //        allPoints[i].parent = null;
        //    }
        //}

        // 모든 LandMarkData를 담을 변수
        LandMarkData landMark = udpServer.landmark;

        // Data가 들어있다면...
        if (landMark.data.Count == (int)PoseName.pose_max)
        {
            // landMark의 위치 값을 point의 위치 값으로 설정
            for (int i = 0; i < allPoints.Length; i++)
            {
                Vector3 pos = new Vector3(landMark.data[i].x, -landMark.data[i].y, landMark.data[i].z);
                allPoints[i].position = pos;
            }
        }
    }
}
