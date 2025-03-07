using Photon.Pun;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // 회전 속력
    public float rotSpeed = 200.0f;

    // 회전 값
    float rotX;
    public float rotY;

    // 회전 가능 여부
    public bool useRotX;
    public bool useRotY;


    public GameObject mainCamera;
    public GameObject uiCamera;

    PhotonView photonView;



    private void Start()
    {
        mainCamera = transform.GetChild(0).gameObject;
        uiCamera = transform.GetChild(1).gameObject;
        photonView = transform.parent.GetComponent<PhotonView>();
        if (photonView != null)
        {
            SetCamera(photonView.IsMine);
        }
    }

    void SetCamera(bool value)
    {
        mainCamera.SetActive(value);
        uiCamera.SetActive(value);
    }

    void Update()
    {
        if (photonView != null)
        {
            if (!photonView.IsMine) return;
        }
        //마우스의 움직임.
        float mx = Input.GetAxis("Mouse X");  // 좌우
        float my = Input.GetAxis("Mouse Y");  // 상하

        //회전 값을 변경 (누적)
        if (useRotX) rotX += my * rotSpeed * Time.deltaTime;  // 상하 회전
        if (useRotY) rotY += mx * rotSpeed * Time.deltaTime;  // 좌우 회전

        //rotX 값을 제한 (최소값, 최대값)
        rotX = Mathf.Clamp(rotX, -60, 60);

        //구해진 회전 값을 나의 회전 값으로 설정
        transform.localEulerAngles = new Vector3(-rotX, rotY, 0);  // 회전값 적용
    }
}
