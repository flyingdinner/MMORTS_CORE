using System;

namespace MMOCore
{
    [Serializable]
    public class MessagePingPong : MessageBase
    {
        public string gameTime;
        public float pingTime;

        public override string MessageKey()
        {
            return "M000:";
        }

        public MessagePingPong()
        {
            gameTime = "-- game time --";
            pingTime = 1f;
        }
    }
}