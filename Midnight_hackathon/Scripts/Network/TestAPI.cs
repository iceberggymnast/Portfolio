using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAPI : MonoBehaviour
{
    private TCPClient client;
    private string testTCPIP = "192.168.1.39";
    async void Start()
    {
        client = new TCPClient();
        await client.ConnectToServer(testTCPIP, 12345);
        Debug.Log("Connected to server");

        await client.SendMessage("Hello, Server!");
    }

    void OnApplicationQuit()
    {
        client.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PoseDetectionApi.GetCurrentPose((result) =>
            {
                print("Success!");
            });
        }   
        
    }
}
