using System.Collections; // IEnumerator를 사용하기 위한 네임스페이스
using UnityEngine;

public class TigerHandMovement : MonoBehaviour
{
    public GameController gameController; // 점수 관리를 위한 GameController

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 FollowObject일 때
        if (collision.gameObject.CompareTag("FollowObject"))
        {
            // 점수 갱신
            gameController.Score += 1;

            // 충돌한 TigerHand를 삭제
            Destroy(gameObject);

            // FollowObject를 삭제
            Destroy(collision.gameObject);
        }
    }
}
