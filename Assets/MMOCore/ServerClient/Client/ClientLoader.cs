using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMOCore.ClientScripts
{
    //-----
    //  Client Loader
    //-----
    public class ClientLoader : MonoBehaviour
    {
        [field: SerializeField] public MMOCore.ClientScripts.SpiritService spiritService {  get; private set; }        
        [field: SerializeField] public SpiritManager spiritManager { get; private set;}

        [Header("-- SetOnConnection --")]
        [SerializeField] private ClientConnectionFacade _connection;

        public void Initialize(ClientConnectionFacade facade)
        {
            _connection = facade;
            spiritService.Initialize(facade);
            spiritManager.Initialize(facade);
        }

    }
}