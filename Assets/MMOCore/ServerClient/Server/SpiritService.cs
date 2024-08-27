using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

namespace MMOCore.ServerScripts
{    
    public class SpiritService : MonoBehaviour
    {
        [SerializeField] private GameObject _spiritGo;
        [SerializeField] private Server _server;
        [SerializeField] private float _maxVisibleRange = 50f;

        [field: SerializeField] private List<SpiritBase> spirits = new List<SpiritBase>();
        [field: SerializeField] public List<SpiritSE> spiritsOnStart;

        private const string BASE_SPIRIT_INDEX = "SpiritService:";
        private int _spiritNewIndex = 1;

        private void OnEnable()
        {
            _server.OnMessageFromClient += Server_OnMessageFromClient;
        }

        private void OnDisable()
        {
            _server.OnMessageFromClient -= Server_OnMessageFromClient;
            
            foreach (var spirit in spirits)            
                if(spirit != null)
                {
                    spirit.OnStatusCheng -= Spirit_OnStatusCheng;
                    spirit.OnCommandAbort -= Spirit_OnCommandAbort;
                }            
        }

        public List<SpiritSE> GetStartSpirits()
        {
            List<SpiritSE> spiritSEs = new List<SpiritSE>();

            foreach (var spirit in spiritsOnStart)
            {
                SpiritSE s = new SpiritSE ();

                _spiritNewIndex++;
                s.id = GetCurrentIndex();

                spiritSEs.Add(spirit);                
            }

            return spiritSEs;
        }

        private void Server_OnMessageFromClient(MessageFromClient message)
        {
            string key = message.messageBase.MessageKey();

            switch (key)
            {
                case "C000:":
                    Debug.Log("DeSerializeToJsonMassege : " + key + " : CommandBase");
                    spirits[0].AddCommand(message.messageBase as CommandBase);
                    break;
            }
        }

        private string GetCurrentIndex()
        {
            return BASE_SPIRIT_INDEX + _spiritNewIndex;
        }

        public SpiritBase SpawnSpirit(ClientEntityBase Client , SpiritSE spiritSE)
        {
            if(spiritSE.id == "-")
            {
                //create new spirit
                spiritSE.id = GetCurrentIndex();
            }

            SpiritBase spawned = null;

            GameObject spiritGO = Instantiate(_spiritGo, Vector3.zero, Quaternion.identity);
            spawned = spiritGO.GetComponent<SpiritBase>();
            spawned.OnStatusCheng += Spirit_OnStatusCheng;
            spawned.OnCommandAbort += Spirit_OnCommandAbort;
            spirits.Add(spawned);            
            return spawned;
        }


        private void Spirit_OnCommandAbort(CommandBase obj)
        {
           
        }

        private void Spirit_OnStatusCheng(SpiritBase chengedSpirit, MMOClientConnection start)
        {     
            // TO DO
            //update list

            List<MMOClientConnection> listenersDone = new List<MMOClientConnection>();
            MessageSpriritStatus message = chengedSpirit.GetStatus();           

            foreach(var spirit in spirits)
            {
                if (listenersDone.Contains(spirit.connection))
                    continue;
                
                if (Vector3.Distance( spirit.position, chengedSpirit.position) < _maxVisibleRange)
                {
                    Debug.Log(">>>>> Spirit_OnStatusCheng: " + chengedSpirit.name);
                    listenersDone.Add(spirit.connection);
                     _server.SendMessageToClient(spirit.connection, message);
                }
            }
        }
    }
}