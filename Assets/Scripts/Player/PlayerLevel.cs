using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Player
{
        public class PlayerLevel : NetworkBehaviour {

        public int level = 1;
        public int experience = 0;

        public PlayerPerks perks;

        void Start () {
            perks = GetComponent<PlayerPerks>();
        }
        
        public void GetExperience(int amount) {
            experience += amount;
            FloatingTextManager.PlayerXp(amount, transform.position);
            if(experience >= level) {
                LevelUp();
            }
        }

        void LevelUp() {
            while(experience >= level) {
                level++;
                experience -= level;
                perks.GrantPerk();
            }
        }
    }
}
