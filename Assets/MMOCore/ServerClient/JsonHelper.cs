using UnityEngine;

namespace MMOCore
{
    public static class JsonHelper
    {
        public static string SerializeToJsonMassege<T>(T message)
        {
            MessageBase mb = message as MessageBase;
            if (mb == null)
            {
                return "-- error : not MessageBase --";
            }

            string json = mb.MessageKey() + JsonUtility.ToJson(message);
            return json;
        }

        public static MessageBase DeSerializeMassege(string message)
        {
            MessageBase messageBase = null;
            string key = message.Substring(0,5);
            string json = message.Substring(5);

            switch (key)
            {
                case "M000:":
                    messageBase = JsonUtility.FromJson<MessagePingPong>(json);
                    Debug.Log("DeSerializeToJsonMassege : " + key + " : MessagePingPong");
                    break;

                case "C000:":
                    messageBase = JsonUtility.FromJson<CommandBase>(json);
                    Debug.Log("DeSerializeToJsonMassege : " + key + " : CommandBase");
                    break;

                case "S000:":
                    messageBase = JsonUtility.FromJson<MessageSpriritStatus>(json);
                    Debug.Log("DeSerializeToJsonMassege : " + key + " : MessageSpriritStatus");
                    break;
            }

            return messageBase;
        }

    }
}