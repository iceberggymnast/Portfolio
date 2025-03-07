using System.Collections;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    // Singleton 처리를 위한 instance 변수 선언
    private static ShakeCamera instance;

    // 외부에서 Get 접근만 가능하도록 Instance Property 선언
    public static ShakeCamera Instance => instance;

    private float shakeTime;
    private float shakeIntensity;

    // Main Camera Object에 Component로 적용시 Game 실행할 때 Memory 할당 / 생성자 Memory 실행
    // 이 때 자기 자신의 정보를 instance 변수에 저장
    public ShakeCamera()
    {
        // 자기 자신에 대한 정보를 static 형태의 instance 변수에 저장해서 외부에서 쉽게 접근할 수 있게 함.
        instance = this;
    }

    // 외부에서 Camera 흔들림을 조작할 때 호출되는 Method
    /// <param name="shakeTime"> Camera 흔들림 지속 시간(설정하지 않으면 기본 값 == 1.0f)
    /// <param name="shakeIntensity"> Camera 흔들림 세기 (값이 클 수록 세게 흔들리며, 설정하지 않으면 기본 값 == 0.1f)
    public void OnShakeCamera(float shakeTime = 1.0f, float shakeIntensity = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine("ShakeByPosition");
        StartCoroutine("ShakeByPosition");
    }

    // Camera를 shakeTime동안 shakeIntensity의 세기로 흔드는 Corroutine Function
    IEnumerator ShakeByPosition()
    {
        // 흔들리기 직전의 시작 위치 (흔들림 종료 후 돌아 올 위치)
        Vector3 startPosition = transform.position;

        while(shakeTime > 0.0f)
        {
            // 초기 위치로 부터 구 범위(Size 1) * shakeIntensity의 범위 내에서 Camera 위치 연동
            transform.position = startPosition + Random.insideUnitSphere * shakeIntensity;

            // 시간 감소
            shakeTime -= Time.deltaTime;

            yield return null;
        }
        transform.position = startPosition;
    }
}
