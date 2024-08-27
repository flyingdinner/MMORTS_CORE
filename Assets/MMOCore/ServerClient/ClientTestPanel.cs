using TMPro;
using UnityEngine;

namespace MMOCore.ClientScripts
{
    public class ClientTestPanel : MonoBehaviour
    {
        [field: SerializeField] public Client client { get; private set; }

        [SerializeField] private GameObject _buttonConnect;
        [SerializeField] private GameObject _buttonDisconnect;
        [SerializeField] private GameObject _buttonTest;
        [SerializeField] private TextMeshProUGUI _textResponse;

        public void StartTestClientButton()
        {
            //server = "127.0.0.1";
            //port = 8080;
            client.StartClient("127.0.0.1", 8080);
            client.OnConnected += Client_OnConnected;
            _buttonConnect.SetActive(false);
        }        

        private void Client_OnConnected(Client obj)
        {
            client.OnConnected -= Client_OnConnected;
            _buttonTest.SetActive(true);
            client.OnReceiveResponse += Client_OnReceiveResponse;
        }

        private void Client_OnReceiveResponse(string obj)
        {
            _textResponse.text = obj;
        }

    }
}