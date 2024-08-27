using MMOCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MMOCore
{
    [Serializable]
    public class CommandBase : MessageBase
    {
        public string commandID;
        public string spiritID;
        public SpiritCommandType command = SpiritCommandType.move;
        public Vector3 targetPoint;
        public float commandTime;

        public override string MessageKey()
        {

            return "C000:" ;
        }
    }
}