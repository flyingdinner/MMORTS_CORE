using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;


namespace MMOCore.ClientScripts
{
    public class Spirit
    {
        public SpiritSE spiritSE;
        public SpirtStatusSE Status;
        public SpiritStatusHandler handler;
    }

    public class SpiritManager : MonoBehaviour
    {
        public event Action OnSpiritManagerStatusCheng;

        [field: SerializeField] public MMOCore.ClientScripts.SpiritService spiritService { get; private set; }

        [field: SerializeField] public ClientConnectionFacade connection { get; private set; }

        [field: SerializeField] public List<Spirit> mySpirits { get; private set; }

        [SerializeField] private CommandBase _testCommand;

        private void OnDisable()
        {
            connection.OnReceiveMessegeFromServer -= Connection_OnReceiveMessegeFromServer;
        }

        public void Initialize(ClientConnectionFacade facade)
        {
            connection = facade;
            mySpirits = new List<Spirit>();
            connection.OnReceiveMessegeFromServer += Connection_OnReceiveMessegeFromServer;
            //StartCoroutine(IE_TestProcess());

        }

        private IEnumerator IE_TestProcess()
        {
            yield return new WaitForSeconds(2f);

            _testCommand.spiritID = spiritService.spirits[0].spiritSE.id;
            spiritService.spirits[0].handler.StartDoCommandPreration(_testCommand);
            connection.SendMassegeToServer(_testCommand);
        }

        private void Connection_OnReceiveMessegeFromServer(MessageBase message)
        {
            string key = message.MessageKey();

            switch (key)
            {
                case "M000:":

                    //Debug.Log("DeSerializeToJsonMassege : " + key + " : MessagePingPong");

                    break;

                case "C000:":
                    //Debug.Log("DeSerializeToJsonMassege : " + key + " : CommandBase");

                    break;

                case "S000:":
                    //Debug.Log("DeSerializeToJsonMassege : " + key + " : MessageSpriritStatus");

                    break;
            }
        }


        public void AddToSpiritsList(Spirit spirit)
        {
            mySpirits.Add(spirit);
            OnSpiritManagerStatusCheng?.Invoke();
        }

    }
}