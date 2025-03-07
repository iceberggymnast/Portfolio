using UnityEngine;
using Photon.Pun;

public class ESCManager : MonoBehaviour
{
    public GameObject escCanvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && escCanvas.activeSelf)
        {
            Time.timeScale = 1f;
            PlayerInfo.instance.isCusor = true;
            PhotonNetwork.LoadLevel("StartScene");
        }

        if (Input.GetKey(KeyCode.X) && escCanvas.activeSelf)
        {
            escCanvas.SetActive(false);
            PlayerInfo.instance.isCusor = false;
            Time.timeScale = 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            escCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
