using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class OilSpawner : MonoBehaviour
{

    //기름 덩어리 모델링들 3가지
    public List<GameObject> oilPrefabs;
    public List<string> oilListName = new List<string>() { "Oil_green", "Oil_purple", "Oil_yellow" };

    List<Vector3> pos = new List<Vector3>();
    List<Vector3> rot = new List<Vector3>();
    List<Vector3> scale = new List<Vector3>();

    float timer;
    float setTime = 5f;


    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            timer += Time.deltaTime;

            if (timer > setTime)
            {
                timer = 0;

                RespawnOilBlob();
            }
        }
    }

    public void RespawnOilBlob()
    {
        if (pos.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, oilListName.Count);
            string selceted = oilListName[randomIndex];

            GameObject go = PhotonNetwork.Instantiate("Trash/" + selceted, pos[0], Quaternion.Euler(rot[0]));
            go.transform.localScale = scale[0];

            pos.Remove(pos[0]);
            rot.Remove(rot[0]);
            scale.Remove(scale[0]);
        }
    }

    public void addList(Vector3 posValue, Vector3 rotValue, Vector3 scaleValue)
    {
        pos.Add(posValue);
        rot.Add(rotValue);
        scale.Add(scaleValue);
    }


    //// 기름덩어리를 다시 생성하는 코루틴
    //private IEnumerator RespawnOilBlobCorutine(Vector3 position, Vector3 scale, float delay)
    //{
    //    yield return new WaitForSeconds(delay); // 지정된 시간 기다림
    //    print("기름덩어리 다시 생성했는지 확인");

    //    //사라진 자리에 랜덤으로 새로운 기름덩어리를 다시 생성
    //    int randomIndex = UnityEngine.Random.Range(0, oilListName.Count);
    //    string selceted = oilListName[randomIndex];
    //    GameObject selectedTrashPrefab = oilPrefabs[randomIndex];
    //    GameObject newOilBlob = PhotonNetwork.Instantiate("Trash/" + selceted, position, Quaternion.identity); // 저장된 위치에 새로운 기름덩어리 생성
    //    newOilBlob.transform.localScale = scale; // 저장된 크기로 크기 설정
    //}
}
