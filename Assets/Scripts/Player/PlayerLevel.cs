using UnityEngine;
using UnityEngine.Networking;
using Util;

namespace Player
{
    public class PlayerLevel : NetworkBehaviour
    {

        [SyncVar]
        public int level = 1;
        [SyncVar]
        public int experience = 0;

        public PlayerPerks perks;

        void Start()
        {
            perks = GetComponent<PlayerPerks>();
        }

        public void GetExperience(int amount)
        {
            if (!isServer) return;

            experience += amount;
            var xpMsg = new PlayerExperienceMessage()
            {
                amount = amount,
                position = transform.position
            };

            NetworkServer.SendToClient(connectionToClient.connectionId, MessageTypes.GrantExperience, xpMsg);

            if (experience >= level)
            {
                LevelUp();
            }
        }

        void LevelUp()
        {
            while (experience >= level)
            {
                level++;
                experience -= level;
                perks.GrantPerk();
            }
        }

        public class PlayerExperienceMessage : MessageBase
        {
            public int amount;
            public Vector3 position;
        }

        public static void OnPlayerExperience(NetworkMessage netMsg)
        {
            var msg = netMsg.ReadMessage<PlayerExperienceMessage>();

            //Seems like the server is receiving the message as well
            FloatingTextManager.PlayerXp(msg.amount, msg.position);
        }
    }
}
