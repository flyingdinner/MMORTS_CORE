using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MMOCore.ClientScripts
{
    public class Client : MonoBehaviour
    {
        public event Action<Client> OnConnected;
        public event Action<Client> OnDisconnected;
        public event Action<string> OnReceiveResponse;

        [field: SerializeField] public string server { get; private set;}
        [field: SerializeField] public int port { get; private set; }

        private TcpClient client;
        private NetworkStream stream;

        public void StartClient( string ip , int p)
        {
            server = ip;
            port = p;
            ConnectToServer();
        }

        public void SendMesegeButton(string messege)
        {
            AsyncSendMassegeButton(messege);
        }

        private async void AsyncSendMassegeButton(string messege = "ping")
        {
            await SendStringMessage(messege);
        }

        void OnApplicationQuit()
        {
            DisconnectFromServer();
        }

        async void ConnectToServer()
        {
            client = new TcpClient();
            await client.ConnectAsync(server, port);
            stream = client.GetStream();
            Debug.Log(">> Client >> Connected to server.");
            OnConnected?.Invoke(this);
            // Получаем ответ
            while (true)
            {
                await ReceiveResponse();
            }
        }

        void DisconnectFromServer()
        {
            stream?.Close();
            client?.Close();
        }

        async Task SendStringMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(data, 0, data.Length);
            Debug.Log($"Sent message: {message}");
        }

        async Task ReceiveResponse()
        {
            byte[] buffer = new byte[256];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            await Task.Delay(250);
            Debug.Log($"Received response from server: {response}");
            OnReceiveResponse?.Invoke(response);
        }
    }
}
