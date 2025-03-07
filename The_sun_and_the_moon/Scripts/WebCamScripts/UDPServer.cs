using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

// LandMark 저장 할 구조
[Serializable]
public class LandMarkFormat
{
    public float x;
    public float y;
    public float z;
    public float visibility;
}

[Serializable]
public class LandMarkData
{
    public List<LandMarkFormat> data;
}

public class UDPServer : MonoBehaviour
{
    // Port
    public int serverPort = 5005;

    // UDPServer Controller
    UdpClient udpServer;

    // EndPoint
    IPEndPoint remoteEndPoint;

    // LandMarkData
    public LandMarkData landmark;

    void Start()
    {
        //transform = GetComponent<Transform>();
        StartUDPServer();
    }

    void Update()
    {

    }

    // Start UDP server
    void StartUDPServer()
    {
        udpServer = new UdpClient(serverPort);
        remoteEndPoint = new IPEndPoint(IPAddress.Any, serverPort);

        print("Begin Server!! Client에서 들어오는 응답 기다리는 중...");

        // 응답이 들어오면 실행되는 Function 등록
        udpServer.BeginReceive(ReceiveData, null);
    }

    void ReceiveData(IAsyncResult result)
    {
        // 응답온 Data를 byte 배열로 받자.
        byte[] receiveByte = udpServer.EndReceive(result, ref remoteEndPoint);

        // byte Data를 string으로 변경하자.(UTF-8)
        string receiveMessage = Encoding.UTF8.GetString(receiveByte);
        print(receiveMessage);

        // receiveMessage가 Array의 Json으로 들어와서 Key 값을 만들어줘야 JsonUtility.FromJson 으로 사용이 가능하다.
        // (무조건은 아니며, receiveMessage를 확인 후!)
        receiveMessage = "{ \"data\" : " + receiveMessage + "}";

        // jsonStringData --> LandMarkData로 변환
        landmark = JsonUtility.FromJson<LandMarkData>(receiveMessage);

        // 다음 응답이 들어오면 실행되는 Function 등록
        udpServer.BeginReceive(ReceiveData, null);
    }

    private void OnDestroy()
    {
        // Server 종료
        udpServer.Close();
        print("UDP Server 종료");
    }
}
