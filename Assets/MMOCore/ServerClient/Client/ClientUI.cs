using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MMOCore.ClientScripts
{
    public class ClientUI : MonoBehaviour
    {
        [SerializeField] private SpiritManager spiritManager;

        private void OnEnable()
        {
            spiritManager.OnSpiritManagerStatusCheng += SpiritManager_OnSpiritManagerStatusCheng;
        }

        private void OnDisable()
        {
            spiritManager.OnSpiritManagerStatusCheng -= SpiritManager_OnSpiritManagerStatusCheng;           
        }

        private void SpiritManager_OnSpiritManagerStatusCheng()
        {
            //TODO
        }

    }
}