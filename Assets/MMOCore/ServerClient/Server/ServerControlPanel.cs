using MMOCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerControlPanel : MonoBehaviour
{
    [SerializeField] private Image _serverStatusImage;
    [SerializeField] private Text _setverStatusText;
    [SerializeField] private GameObject _serverLaunchPanel;
    [SerializeField] private Server _server;

    [SerializeField] private Text _clientCounter;

    private void OnEnable()
    {
        _server.OnServerStarted += server_OnServerStarted;
    }

    private void OnDisable()
    {
        _server.OnServerStarted -= server_OnServerStarted;
        _server.OnClientConnected -= server_OnClientConnected;
    }

    private void server_OnClientConnected(MMOClientConnection obj)
    {
        _clientCounter.text = "Client count : " + _server.clients.Count.ToString();
    }

    private void server_OnServerStarted()
    {
        _serverLaunchPanel.SetActive(false);
        _serverStatusImage.color = Color.green;
        _setverStatusText.text = "Server Started";
        _server.OnClientConnected += server_OnClientConnected;
    }

}
