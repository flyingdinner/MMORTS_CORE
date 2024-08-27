using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMOCore
{
    [Serializable]
    public class SpiritSE
    {
        public string id = "-";
    }

    [Serializable]
    public class SpirtStatusSE
    {
        public Vector3 startPosition;
        public Vector3 endPosition;
        public float moveTime;
        public float elapsTime;
        public SpiritStatusType[] stetes;
    }
}

namespace MMOCore.ServerScripts

{
    public class SpiritBase : MonoBehaviour
    {
        public event Action<CommandBase> OnCommandAbort;
        public event Action<SpiritBase,MMOClientConnection> OnStatusCheng;
        
        [field: SerializeField] public List<SpiritStatusType> SpiritStatus {  get; private set; }
        [field: SerializeField] public string id { get; private set; }
        [field: SerializeField] public MMOClientConnection connection { get; private set; }
        [field: SerializeField] public SpiritSE spiritSE { get; private set; }

        [SerializeField] private Transform _spiritTransform;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private CommandBase _currentCommand;

        private SpirtStatusSE _currentStatus;
        private Coroutine _currentCoroutine;
        public Vector3 position => _spiritTransform.localPosition; 

        public void Initialize(MMOClientConnection connection)
        {
            _currentStatus = new SpirtStatusSE();
            this.connection = connection;

            OnStatusCheng?.Invoke(this, connection);
        }

        public MessageSpriritStatus GetStatus()
        {
            MessageSpriritStatus ms_status = new MessageSpriritStatus();
            ms_status.SpiritSE = spiritSE;
            ms_status.Status = _currentStatus;

            return ms_status;
        }

        public void AddCommand(CommandBase command)
        {
            if (_currentCommand != null)
            {
                AbortCommand();
            }

            _currentCommand = command;
            if(command.command == SpiritCommandType.move)
            {
                _currentCoroutine = StartCoroutine(IECommand_Move(command));
            }
        }

        public void SetPositionHard(Vector3 position)
        {

        }

        private void AbortCommand()
        {            
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }
            _currentCommand = null;
        }

        private IEnumerator IECommand_Move(CommandBase command)
        {
            SpiritStatus.Add(SpiritStatusType.move);
            _currentStatus.endPosition = command.targetPoint;
            _currentStatus.startPosition = _spiritTransform.position;
            _currentStatus.elapsTime = 0f;
            _currentStatus.moveTime = Vector3.Distance(_currentStatus.endPosition, _currentStatus.startPosition) / _moveSpeed;
            
            OnStatusCheng?.Invoke(this,connection);

            yield return new WaitForEndOfFrame();

            while (_currentStatus.elapsTime < _currentStatus.moveTime)
            {
                _spiritTransform.position = Vector3.Lerp(_currentStatus.startPosition, _currentStatus.endPosition, _currentStatus.elapsTime / _currentStatus.moveTime);
                _currentStatus.elapsTime += Time.deltaTime;
                yield return null;
            }

            _spiritTransform.position = _currentStatus.endPosition;

            // End move event
            _currentStatus.startPosition = _currentStatus.endPosition;
            _currentStatus.elapsTime = 0f;
            _currentStatus.moveTime = 0;
            SpiritStatus.Remove(SpiritStatusType.move);

            OnStatusCheng?.Invoke(this, connection);
        }
    }
}