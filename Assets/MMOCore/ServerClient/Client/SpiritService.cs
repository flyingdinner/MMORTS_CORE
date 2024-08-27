using MMOCore.ServerScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

namespace MMOCore.ClientScripts
{
    public class SpiritService : MonoBehaviour
    {
        //[field: SerializeField] public SpiritManager spiritManager { get; private set; }
        [field:SerializeField] public List<Spirit> spirits {  get; private set; }
        [field: SerializeField] public ClientConnectionFacade connection { get; private set; }

        [SerializeField] private GameObject _spiritBaseGO;

        private void OnDisable()
        {
            connection = null;
            connection.OnReceiveMessegeFromServer -= Connection_OnReceiveMessegeFromServer;
        }
     
        public void Initialize(ClientConnectionFacade facade)
        {
            spirits = new List<Spirit>();
            connection = facade;
            connection.OnReceiveMessegeFromServer += Connection_OnReceiveMessegeFromServer;
        }

        public Spirit SpawnSpirit( SpiritSE spiritSE)
        {
            Spirit spawned = new Spirit ();

            GameObject spiritGO = Instantiate(_spiritBaseGO, Vector3.zero, Quaternion.identity);
            spawned.handler = spiritGO.GetComponent<SpiritStatusHandler>();
            spawned.spiritSE = spiritSE;

            spirits.Add(spawned);
            return spawned;
        }

        private void Connection_OnReceiveMessegeFromServer(MessageBase message)
        {
            string key = message.MessageKey();
           
            switch (key)
            {
                case "C000:":
                    //TO DO
                    Debug.Log("DeSerializeToJsonMassege : " + key + " : CommandBase");
                    spirits[0].handler.StartDoCommandFromServer(message as CommandBase);
                    break;

                case "S000:":
                    OnSpriritStatusUpdate(message as MessageSpriritStatus);
                    break;
            }
        }

        private void OnSpriritStatusUpdate(MessageSpriritStatus message)
        {
            foreach(var spirit in spirits)
            {
                if(spirit.spiritSE.id == message.SpiritSE.id)
                {
                    UpdateSpiritStatus(message,spirit);
                    return;
                }
            }

            UpdateSpiritStatus(message,SpawnSpirit(message.SpiritSE));
        }

        private void UpdateSpiritStatus(MessageSpriritStatus message, Spirit spirit)
        {
            Debug.Log(" >>>> UpdateSpiritStatus : ");
            spirit.handler.UpdateStus(message.Status);
        }

    }
}