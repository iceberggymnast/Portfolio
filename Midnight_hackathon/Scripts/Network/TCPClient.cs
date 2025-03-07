using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class TCPClient
{
    private TcpClient client;
    private NetworkStream stream;

    public async Task ConnectToServer(string ipAddress, int port)
    {
        client = new TcpClient();
        await client.ConnectAsync(ipAddress, port);
        stream = client.GetStream();
    }

    public async Task SendMessage(string message)
    {
        if (stream != null)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received: " + response);
        }
    }

    public void Close()
    {
        stream.Close();
        client.Close();
    }
}