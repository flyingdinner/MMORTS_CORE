using UnityEngine;

namespace MMOCore.ServerScripts
{
    public class ServerClientsLoader : MonoBehaviour
    {
        //public event Action<MMOClientConnection,ClientEntityBase> OnClientLoaded;

        [SerializeField] private Server server;
        [SerializeField] private Transform spawnParent;
        [SerializeField] private GameObject clientOnServerPrefab;
        [SerializeField] private ServerClientsOnLineHolder serverClientsOnLineHolder;

        private void OnEnable()
        {
            server.OnServerStarted += Server_OnServerStarted;    
        }
        private void OnDisable()
        {
            server.OnServerStarted -= Server_OnServerStarted;
            server.OnClientConnected -= Server_OnClientConnected;
        }

        private void Server_OnServerStarted()
        {
            Debug.Log("ServerClientsLoader_Server_OnServerStarted");
            server.OnServerStarted -= Server_OnServerStarted;
            server.OnClientConnected += Server_OnClientConnected;
        }

        //>-------------------------------
        //     Create client here
        //>-------------------------------
        private void Server_OnClientConnected(MMOClientConnection client)
        {
            Debug.Log("ServerClientsLoader_OnClientConnected : ");

            GameObject clientgo = Instantiate(clientOnServerPrefab,Vector3.zero,Quaternion.identity);
            clientgo.transform.parent = spawnParent;
            clientgo.transform.localPosition = Vector3.zero;

            ClientEntityBase clientEntityBase = clientgo.GetComponent<ClientEntityBase>();
            clientEntityBase.Initialize(client);

            //------------------------------------------
            //      TO DO : load data from Data Base
            //------------------------------------------

            serverClientsOnLineHolder.OnClientLoaded(client, clientEntityBase);

        }
    }
}