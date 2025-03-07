using System.Collections;
using UnityEngine;

namespace DistantLands
{
    public class fishResident : MonoBehaviour
    {
        public Transform DestroyPos; // 목표 위치 (Transform)

        private float moveDuration = 500f; // 물고기가 이동하는 데 걸리는 시간
        public GameObject HeartText;

        // 자식 개체 중 'Oilbolb' 태그가 있는지 확인
        public bool hasOil = false;
        private bool hasRotated = false; // 회전 여부를 체크하는 flag

        void Start()
        {
        }

        void Update()
        {
            tttt(); // 기름 태그 확인
        }

        void tttt()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                // 각 자식의 태그가 Oilbolb인지 확인
                if (transform.GetChild(i).CompareTag("Oilbolb_Quest"))
                {
                    hasOil = true; // 기름이 있으면 flag를 true로 설정
                    break; // 기름이 있으면 더 이상 체크할 필요 없음
                }
                else
                {
                    hasOil = false; // 기름이 없으면 flag를 false로 설정
                }
            }

            if (!hasOil && !hasRotated) // 기름이 없고, 회전을 아직 하지 않았다면
            {
                print("고마워");
                PlayerInfo.instance.PointPlusOrMinus(100);
                HeartText.gameObject.SetActive(true);
                MoveTowardsDestroyPos(); // 이동 메서드 호출
            }
        }

        void MoveTowardsDestroyPos()
        {
            // DestroyPos 방향으로 회전
            Vector3 direction = (DestroyPos.position - transform.position).normalized; // 목표 방향 벡터 계산
            Quaternion targetRotation = Quaternion.LookRotation(direction); // 목표 회전 계산

            // 회전을 부드럽게 적용
            StartCoroutine(RotateTowards(targetRotation));

            // 물고기를 목표 위치로 이동
            StartCoroutine(MoveToPosition(DestroyPos.position));

            // 회전이 완료되었으므로 flag를 true로 설정
            hasRotated = true;
        }

        IEnumerator RotateTowards(Quaternion targetRotation) // IEnumerator 사용
        {
            float rotationTime = 1f; // 회전하는 데 걸리는 시간
            float elapsed = 0f;

            Quaternion initialRotation = transform.rotation;

            while (elapsed < rotationTime)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / rotationTime;

                // 회전 보간
                transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
                yield return null;
            }

            transform.rotation = targetRotation; // 최종 회전 적용
        }

        IEnumerator MoveToPosition(Vector3 targetPosition)
        {
            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                // 시간 비율 계산
                float t = elapsedTime / moveDuration;

                // 현재 위치에서 목표 위치로의 선형 보간
                transform.position = Vector3.Lerp(transform.position, targetPosition, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition; // 최종 위치 적용
            Destroy(gameObject); // 물고기를 비활성화
        }
    }
}
