using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TreasureBillboard : MonoBehaviourPun
{
    public Camera myMainCamera;

    void Start()
    {
        // 코루틴으로 카메라 설정 대기
        StartCoroutine(InitializeCamera());
    }


    IEnumerator InitializeCamera()
    {
        // PlayerInfo.instance나 player가 null일 때 대기
        while (PlayerInfo.instance == null || PlayerInfo.instance.player == null)
        {
            yield return null;

            // 초기화가 완료되면 카메라를 설정
            //myCamera = PlayerInfo.instance.player.transform.GetChild(1).GetChild(0).GetComponent<Camera>();
            //print(PlayerInfo.instance.player.transform.GetChild(1).gameObject.name);
            //print(PlayerInfo.instance.player.transform.GetChild(1).GetChild(0).gameObject.name);

        }

        // PlayerInfo.instance.player가 초기화되면 카메라 설정
        myMainCamera = PlayerInfo.instance.player.transform.GetChild(1).GetChild(0).GetComponent<Camera>();
    }

    void Update()
    {
        // myUICamera가 null이 아닌 경우에만 실행
        if (myMainCamera != null)
        {
            // 오브젝트가 카메라를 바라보도록 설정
            transform.LookAt(myMainCamera.transform.position);

            // 캔버스가 카메라를 바라보도록 설정
            //Vector3 direction = myMainCamera.transform.position - transform.position;
            //transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
