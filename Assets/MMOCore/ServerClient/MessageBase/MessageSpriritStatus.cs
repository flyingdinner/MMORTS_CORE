using MMOCore.ServerScripts;
using System;

namespace MMOCore
{
    [Serializable]
    public class MessageSpriritStatus : MessageBase
    {
        public override string MessageKey()
        {
            return "S000:";
        }

        public SpiritSE SpiritSE;
        public SpirtStatusSE Status;
    }
}