using System.Collections.Generic;
using UnityEngine;

namespace MMOCore.ServerScripts
{
    public class ServerClientsOnLineHolder : MonoBehaviour
    {
        [field: SerializeField] public List<ClientEntityBase> entityInGame;                
        
        [SerializeField] private SpiritService _spiritService;

        private Dictionary<ClientEntityBase, MMOClientConnection> _clientEntityInGame;
        private Server server;

        private void OnEnable()
        {
            entityInGame = new List<ClientEntityBase>();
            _clientEntityInGame = new Dictionary<ClientEntityBase, MMOClientConnection>();
        }

        private void OnDisable()
        {
        }

        //------------------------------------------------------------
        //              client spawned on server
        //------------------------------------------------------------
        public void OnClientLoaded(MMOClientConnection connnection, ClientEntityBase entity)
        {
            if(server == null)
                server = Server.Instance;

            if (_clientEntityInGame.ContainsValue(connnection))
                return;

            _clientEntityInGame.Add(entity, connnection);

            if (entity.spiritsToLoad?.Count > 0)
            {
                CreateSpiritsForClient(entity, entity.spiritsToLoad);
            }
            else
            {
                CreateSpiritsForClient(entity, _spiritService.GetStartSpirits());                
            }
        }

        private void CreateSpiritsForClient(ClientEntityBase client , List<SpiritSE> SpiritsToCreate)
        {
            //TODO spawn
            foreach(SpiritSE spirit in SpiritsToCreate)
            {
                SpiritBase sb = _spiritService.SpawnSpirit(client, spirit);
                client.AddSpiritsToControll(sb);

            }

        }
    }
}