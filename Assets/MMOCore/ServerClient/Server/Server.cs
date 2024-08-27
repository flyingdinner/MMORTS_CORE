using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MMOCore
{
    [Serializable]
    public class MMOClientConnection
    {
        public string name;
        public TcpClient client;

        public MMOClientConnection(string n, TcpClient tcpc)
        {
            name = n;
            client = tcpc;
        }
    }

    [SerializeField]
    public class MessageFromClient
    {
        public string name;
        public bool bussy = false;
        public bool completed = false;
        public MMOClientConnection client;
        public MessageBase messageBase;

        public MessageFromClient (MMOClientConnection c, MessageBase mb)
        {
            name = mb.MessageKey();
            client = c;
            messageBase = mb;
            bussy = true;
            completed = false;
        }
    }

    [SerializeField]
    public class MessageToClientsContainer
    {
        public string name;
        public bool bussy = false;
        public bool completed = false;
        public List<MMOClientConnection> listeners;
        public MessageBase messageBase;

        public MessageToClientsContainer( List<MMOClientConnection> listerensList, MessageBase mb)
        {
           
            name = mb.MessageKey();
            listeners = listerensList;
            messageBase = mb;
            bussy = true;
            completed = false;
        }
    }
    //--------------------------------------------
    //              Server 
    //--------------------------------------------
    public class Server : MonoBehaviour
    {
        public static Server Instance { get; private set; }

        public event Action OnServerStarted;
        public event Action<MMOClientConnection> OnClientConnected;
        public event Action<MMOClientConnection> OnClientDisconected;
        public event Action<MessageFromClient> OnMessageFromClient;

        [field: SerializeField] public int port { get; private set; }

        [field: SerializeField] public List<MMOClientConnection> clients = new List<MMOClientConnection>();

        [field: SerializeField] public List<MessagePingPong> messegesToWait_PingPong = new List<MessagePingPong>();

        [field: SerializeField] public List<MessageFromClient> messageFromClientWaitList = new List<MessageFromClient>();

        private List<MMOClientConnection> _waitToAddList = new List<MMOClientConnection>();
        private List<MMOClientConnection> _waitToRemoveList = new List<MMOClientConnection>();
        private TcpListener server;
        private bool isRunning = false;

        private void Awake()
        {
            Instance = this;
        }

        public void Update()
        {
            if (!isRunning)
                return;
            
            if(_waitToAddList.Count > 0)
            {
                AddClientLoop();
            }

            if(_waitToRemoveList.Count > 0)
            {
                RemoveClientLoop();
            }

            if (messageFromClientWaitList.Count > 0)
            {
                ClientWaitListLoop();
            }
        }

        public void StartServerButton()
        {
            StartServer();
            OnServerStarted?.Invoke();
        }

        private void RemoveClientLoop()
        {
            foreach (var client in _waitToRemoveList)
            {
                OnClientDisconected?.Invoke(client);
            }

            _waitToRemoveList = new List<MMOClientConnection>();
        }

        public void AddClientLoop()
        {
            foreach (var client in _waitToAddList)
            {
                OnClientConnected?.Invoke(client);
                Debug.Log("Client connected: " + client.name);
            }

            _waitToAddList = new List<MMOClientConnection>();
        }

        private void ClientWaitListLoop()
        {
            foreach(MessageFromClient clientm in messageFromClientWaitList)
            {
                if (clientm.completed) continue;
                if (!clientm.bussy) continue;

                OnMessageFromClient?.Invoke(clientm);
                clientm.bussy = false;
                clientm.completed = true;
            }
        }


        public void SetPort(string text)
        {
            string digitsOnly = new string(text.Where(char.IsDigit).ToArray());
            port = int.Parse(digitsOnly);
        }

        void OnApplicationQuit()
        {
            StopServer();
        }

        public void AddMessageToClientsContainer(MessageToClientsContainer message)
        {
            foreach(MMOClientConnection connection in message.listeners)
            {
                SendMessageToClient(connection, message.messageBase);
            }
        }

        public void SendMessageToClient(MMOClientConnection connection, MessageBase message)
        {
            Task.Run(() => ATask_SendMessageToClient(connection,message));
        }

        private async Task ATask_SendMessageToClient(MMOClientConnection connection , MessageBase message)
        {
            NetworkStream stream = connection.client.GetStream();

            byte[] response = Encoding.UTF8.GetBytes(JsonHelper.SerializeToJsonMassege(message));
            await stream.WriteAsync(response, 0, response.Length);
            Debug.Log("Sent message : " + message.MessageKey());
        }

        void StartServer()
        {
            if (isRunning)
            {
                Debug.LogWarning("Server is already running.");
                return;
            }

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(ipAddress, port);
            server.Start();
            isRunning = true;
            Debug.Log("Server started. Waiting for connections...");
            Task.Run(() => ListenForClients());
        }

        void StopServer()
        {
            isRunning = false;
            server?.Stop();

            foreach (var client in clients)
            {
                client.client.Close();
            }
            clients.Clear();
        }

        async Task ListenForClients()
        {
            try
            {
                while (isRunning)
                {
                    TcpClient client = await server.AcceptTcpClientAsync();
                    ClientConnected(client);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error in ListenForClients: " + ex.Message);
            }
        }

        private void ClientConnected(TcpClient client)
        {
            MMOClientConnection mmoclient = new MMOClientConnection(client.Client.RemoteEndPoint.ToString(), client);
            clients.Add(mmoclient);
            _waitToAddList.Add(mmoclient);
            Task.Run(() => ListenClient(mmoclient));            
        }

        async Task ListenClient(MMOClientConnection mmoclient)
        {
            Debug.Log("Server >> Client connected: " + mmoclient.name);
            try
            {
                while (isRunning && mmoclient.client.Connected)
                {
                    await Task.Run(() => HandleClient(mmoclient));
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error in ListenClient: " + ex.Message);
            }
            finally
            {
                ClientDisconnected(mmoclient);
            }
        }

        async Task HandleClient(MMOClientConnection mmoclient)
        {
            NetworkStream stream = mmoclient.client.GetStream();

            byte[] buffer = new byte[256];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead == 0)
            {
                return;
            }

            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);           
            MessageBase messageBase = JsonHelper.DeSerializeMassege(message);

            if(messageBase == null)
            {
                Debug.Log("messageBase == null");
            }
            else
            {
                MessageFromClient mfc = new MessageFromClient(mmoclient, messageBase);

                AddInWaitList(mfc);
            }
        }

        private void ClientDisconnected(MMOClientConnection mmoclient)
        {
            if (clients.Contains(mmoclient))
            {
                clients.Remove(mmoclient);
                mmoclient.client.Close();
                _waitToRemoveList.Add(mmoclient);
                Debug.Log($"Client {mmoclient.name} disconnected.");
            }
        }


        private void AddInWaitList(MessageFromClient messageBase)
        {

            for (int i = 0; i < messageFromClientWaitList.Count; i++)
            {
                if (messageFromClientWaitList[i].bussy) continue;

                messageFromClientWaitList[i] = messageBase;
                return;
            }

            messageFromClientWaitList.Add(messageBase);
        }
    }

}
