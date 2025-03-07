using System.Collections;
using UnityEngine;

public class HandEffectManager : MonoBehaviour
{
    // 손 오브젝트에 연결된 자식 이펙트들
    public GameObject leftHandEffect;
    public GameObject rightHandEffect;

    public HandsPunchList handsPunchList;
    public float effectDuration = 0.5f; // 이펙트가 활성화될 시간

    private void Start()
    {
        // 시작 시 이펙트만 비활성화 (부모는 그대로 유지)
        SetEffectActive(false);

        // handsPunchList가 할당되어 있는지 확인
        if (handsPunchList == null)
        {
            Debug.LogError("handsPunchList가 할당되지 않았습니다!");
        }
    }

    private void Update()
    {
        // handsPunchList가 null이 아닌지 확인
        if (handsPunchList == null)
        {
            Debug.LogError("handsPunchList가 null입니다!");
            return;
        }

        // Punch 결과에 따라 왼손 또는 오른손 이펙트를 활성화
        string result = handsPunchList.GetTigerPunchResult();
        if (result == "LEFT")
        {
            StartCoroutine(ActivateEffect(leftHandEffect));
        }
        else if (result == "RIGHT")
        {
            StartCoroutine(ActivateEffect(rightHandEffect));
        }
    }

    // 자식 이펙트를 활성화한 후 일정 시간 뒤 비활성화
    private IEnumerator ActivateEffect(GameObject handEffect)
    {
        if (handEffect == null)
        {
            Debug.LogError("handEffect가 null입니다!");
            yield break;
        }

        handEffect.SetActive(true); // 자식 이펙트 활성화
        yield return new WaitForSeconds(effectDuration); // 일정 시간 대기
        handEffect.SetActive(false); // 자식 이펙트 비활성화
    }

    // 이펙트만 비활성화하는 함수
    private void SetEffectActive(bool isActive)
    {
        if (leftHandEffect != null)
        {
            leftHandEffect.SetActive(isActive);
        }

        if (rightHandEffect != null)
        {
            rightHandEffect.SetActive(isActive);
        }
    }
}
