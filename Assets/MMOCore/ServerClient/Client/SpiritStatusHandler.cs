using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.MemoryProfiler;
using UnityEngine;

namespace MMOCore.ClientScripts
{
    public class SpiritStatusHandler : MonoBehaviour
    {
        [field: SerializeField] public SpiritSE spiritSE { get; private set; }
        [field: SerializeField] public SpirtStatusSE statusSE { get; private set; }

        [field: SerializeField] public CommandBase commandWaitToDO { get; private set; }

        public void Initialize(SpiritSE sse)
        {
            spiritSE = sse;
        }

        public void UpdateStus(SpirtStatusSE status)
        {

            statusSE = status;
        }

        public void StartDoCommandPreration(CommandBase command)
        {
            Debug.Log(">>> StartDoCommand  Preration");
            commandWaitToDO = command;

        }

        public void StartDoCommandFromServer(CommandBase command)
        {
            Debug.Log(">>> StartDoCommand  FromServer");
            commandWaitToDO = command;
            StartCoroutine(IECommand_Move(command));
        }

        private IEnumerator IECommand_Move(CommandBase command)
        {
            // //SpiritStatus.Add(SpiritStatusType.move);
            statusSE.endPosition = command.targetPoint;
            statusSE.startPosition = transform.position;
            statusSE.elapsTime = 0f;
            statusSE.moveTime = command.commandTime;

           // OnStatusCheng?.Invoke(this, connection);


           yield return new WaitForEndOfFrame();

           while (statusSE.elapsTime < statusSE.moveTime)
           {
               transform.position = Vector3.Lerp(statusSE.startPosition, statusSE.endPosition, statusSE.elapsTime / statusSE.moveTime);
               statusSE.elapsTime += Time.deltaTime;
               yield return null;
          }

           // _spiritTransform.position = _currentStatus.endPosition;

            // End move event
           // _currentStatus.startPosition = _currentStatus.endPosition;
           // _currentStatus.elapsTime = 0f;
           // _currentStatus.moveTime = 0;
           // SpiritStatus.Remove(SpiritStatusType.move);

           // OnStatusCheng?.Invoke(this, connection);
        }
    }
}