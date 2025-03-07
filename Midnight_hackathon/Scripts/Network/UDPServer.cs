using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPServer : MonoBehaviour
{
    private UdpClient udpServer;
    private IPEndPoint remoteEndPoint;
    public int serverPort = 7777;

    // Use this PoseInfo
    public PoseInfo CurrentPoseInfo { get; private set; } = new PoseInfo();

    private void Start()
    {
        // Example: Start the UDP server on port 5555
        StartUDPServer(serverPort);
    }

    private void StartUDPServer(int port)
    {
        udpServer = new UdpClient(port);
        remoteEndPoint = new IPEndPoint(IPAddress.Any, port);

        Debug.Log("Server started. Waiting for messages...");

        // Start receiving data asynchronously
        udpServer.BeginReceive(ReceiveData, null);
    }

    private void ReceiveData(IAsyncResult result)
    {
        byte[] receivedBytes = udpServer.EndReceive(result, ref remoteEndPoint);
        // ProcessReceivedData(receivedBytes);
        string receivedMessage = Encoding.UTF8.GetString(receivedBytes);
        Debug.Log("Received from client: " + receivedMessage + $"/ bytes size: {receivedBytes.Length}");
        try
        {
            var info = PoseInfo.FromUdpResponse(receivedMessage);
            if (info != null)
            {
                CurrentPoseInfo = info;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"PoseInfo parsing error: {e.Message}");
        }
        // Process the received data

        // Continue receiving data asynchronously
        udpServer.BeginReceive(ReceiveData, null);
    }

    // 수신한 데이터를 BinaryReader로 처리
    private void ProcessReceivedData(byte[] data)
    {
        using (MemoryStream memoryStream = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(memoryStream))
            {
                // 예시: int, float, string 데이터 읽기
                try
                {
                    var x = reader.ReadSingle();
                    float floatValue = reader.ReadSingle();
                    // string stringValue = reader.ReadString();

                    Debug.Log($"받은 데이터 - int: {x}, float: {floatValue}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"데이터 읽기 중 오류 발생: {e.Message}");
                }
            }
        }
    }

    private void SendData(string message, IPEndPoint endPoint)
    {
        byte[] sendBytes = Encoding.UTF8.GetBytes(message);

        // Send the message to the client
        udpServer.Send(sendBytes, sendBytes.Length, endPoint);

        Debug.Log("Sent to client: " + message);
    }


    void OnApplicationQuit()
    {
        udpServer.Close();
        Debug.Log("UDP Server stopped.");
    }
}