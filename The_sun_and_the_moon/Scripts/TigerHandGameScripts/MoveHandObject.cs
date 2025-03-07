using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveHandObject : MonoBehaviour
{
    private bool moveUpL = false;
    private bool moveUpR = false;
    //손이 문 밖으로 나간 상태에서는 Z값이 5
    //손이 문 안으로 들어간 상태에서는 Z값이 -3
    private float OutDoor_Z =5.0f;
    private float InDoor_Z = -8.0f;
    //밖일 때 
    public bool isOut = false;

    public float moveSpeed = 15.0f;

    public Transform left;
    public Transform right;


    private void Start()
    {
        // Set initial state
        bool initialMoveUpL = false;
        SetMoveUp(initialMoveUpL);
        // Start the coroutine
        //StartCoroutine(TimerCoroutine());

        //랜덤으로 왼손 또는 오른손이 나오는 함수 호출
        StartCoroutine(TimerCoroutine());
    }

    private void Update()
    {
        MoveMent();
    }

    //일단 호랑이 손(왼/오)
    // 손이 이동하는 로직
    void MoveMent()
    {
        //moveUpL = true일 때 실행할건데, 밑의 함수에서 랜덤으로 왼/오 결정하는데 왼손을 움직이겠다고 결과가 나왔을 때
        if (moveUpL)
        {
            //만약에 손 기준의 z값이 OutDoor_Z(5)보다 작을 때만
            if (left.localPosition.z < OutDoor_Z)
            {
                // 문 밖으로 이동
                left.localPosition += Vector3.forward * moveSpeed * Time.deltaTime;
            }
            // 만약 문 밖에 도달했으면
            if (left.localPosition.z >= OutDoor_Z)
            {
                //손의 위치를 OutDoor_Z로 설정
                left.localPosition = new Vector3(left.localPosition.x, left.localPosition.y, OutDoor_Z); // 정확한 위치로 설정
                //moveUpL = true로 바꿔주기
                moveUpL = false;
            }
        }
        //moveUpL = false일 때 실행할건데,
        else
        {
            if (left.localPosition.z > InDoor_Z)
            {
                // 문 안으로 이동
                left.localPosition -= Vector3.forward * moveSpeed * Time.deltaTime;
            }
            // 문 안에 도달했으면
            if (left.localPosition.z <= InDoor_Z)
            {
                left.localPosition = new Vector3(left.localPosition.x, left.localPosition.y, InDoor_Z); // 정확한 위치로 설정
                moveUpL = true;
            }
        }

        if (moveUpR)
        {
            if (right.localPosition.z < OutDoor_Z)
            {
                // 문 밖으로 이동
                right.localPosition += Vector3.forward * moveSpeed * Time.deltaTime;
            }
            // 문 밖에 도달했으면
            if (right.localPosition.z >= OutDoor_Z)
            {
                right.localPosition = new Vector3(right.localPosition.x, right.localPosition.y, OutDoor_Z); // 정확한 위치로 설정
                moveUpR = false;
            }
        }
        else
        {
            if (right.localPosition.z > InDoor_Z)
            {
                // 문 안으로 이동
                right.localPosition -= Vector3.forward * moveSpeed * Time.deltaTime;
            }
            // 문 안에 도달했으면
            if (right.localPosition.z <= InDoor_Z)
            {
                right.localPosition = new Vector3(right.localPosition.x, right.localPosition.y, InDoor_Z); // 정확한 위치로 설정
                moveUpR = true;
            }
        }
    }

    private IEnumerator TimerCoroutine()
    {
        while (true)
        {
            // Wait for a random time
            float randomTime = Random.Range(1f, 3f);
            yield return new WaitForSeconds(randomTime);

            // Determine which hand should move
            if (!moveUpL && !moveUpR) // Only set a hand to move if no hand is moving
            {
                bool randomMoveUpL = Random.value > 0.5f;
                SetMoveUp(randomMoveUpL);
            }
        }
    }

    // Public method to set both moveUpL and moveUpR
    public void SetMoveUp(bool moveUpLValue)
    {
        moveUpL = moveUpLValue;
        moveUpR = !moveUpLValue; // Ensure the other boolean is false
    }

    // Public method to set moveUpL
    public void SetMoveUpL(bool value)
    {
        moveUpL = value;
        moveUpR = !value; // Ensure moveUpR is false
    }

    // Public method to set moveUpR
    public void SetMoveUpR(bool value)
    {
        moveUpR = value;
        moveUpL = !value; // Ensure moveUpL is false
    }
}
