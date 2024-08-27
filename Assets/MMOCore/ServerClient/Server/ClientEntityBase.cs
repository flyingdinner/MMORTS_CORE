using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMOCore.ServerScripts
{
    public class ClientEntityBase : MonoBehaviour
    {
        [field: SerializeField] public MMOClientConnection mMOClientConnection {  get; private set; }
        [field: SerializeField] public List<ClientStateType> currentState { get; private set; }
        [field: SerializeField] public List<SpiritBase> spirits { get; private set; }
        [field: SerializeField] public List<SpiritSE> spiritsToLoad { get; private set; }

        [SerializeField] private Transform mainPoint;
        [SerializeField] private Vector3 lastPosition;

        public void Initialize(MMOClientConnection connection)
        {
            spirits = new List<SpiritBase>();

            currentState = new List<ClientStateType>
            {
                ClientStateType.init
            };

            mMOClientConnection = connection;
        }

        public void AddSpiritsToControll(SpiritBase sb)
        {
            spirits.Add(sb);
            sb.Initialize(mMOClientConnection);
        }

    }
}