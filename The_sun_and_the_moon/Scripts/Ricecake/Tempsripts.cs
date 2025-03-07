using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tempsripts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ClearSkip());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ClearSkip()
    {
        yield return new WaitForSeconds(2);
        QuestManager.questManager.QuestAddProgress(8, 0, 1);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(2);

        }
    }
}
