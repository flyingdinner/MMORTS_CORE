using System;
using UnityEngine;

namespace MMOCore.ClientScripts
{
    public class ClientConnectionFacade : MonoBehaviour
    {
        public event Action<MessageBase> OnReceiveMessegeFromServer;

        public event Action OnConnected;
        public event Action OnDisconnected;

        [field: SerializeField] public ClientLoader clientLoader { get; private set; }
        [field:SerializeField] public Client client {  get; private set; }
        [field: SerializeField] public bool connected { get; private set; }

        [SerializeField] private string _ip;
        [SerializeField] private int _port;

        private void OnEnable()
        {
            client.OnConnected += Client_OnConnected;
            client.OnDisconnected += Client_OnDisconnected;
            client.OnReceiveResponse += Client_OnReceiveResponse;
        }

        private void OnDisable()
        {
            client.OnConnected -= Client_OnConnected;
            client.OnDisconnected -= Client_OnDisconnected;
            client.OnReceiveResponse -= Client_OnReceiveResponse;
        }
        public void StartConnect()
        {
            client.StartClient(_ip, _port);
        }
        private void Client_OnConnected(Client obj)
        {
            connected = true;
            OnConnected?.Invoke();
            clientLoader.Initialize(this);
        }           

        private void Client_OnDisconnected(Client obj)
        {
            connected = false;
            OnDisconnected?.Invoke();
        }

        public void SendMassegePingToServer(string massege)
        {
            SendMassegeToServer(new MessagePingPong());
        }

        public void SendMassegeToServer(MessageBase sclass)
        {
            string message = JsonHelper.SerializeToJsonMassege(sclass);
            client.SendMesegeButton(message);
        }        
        
        private void Client_OnReceiveResponse(string massege)
        {
            OnReceiveMessegeFromServer?.Invoke(JsonHelper.DeSerializeMassege(massege));
        }

    }
}