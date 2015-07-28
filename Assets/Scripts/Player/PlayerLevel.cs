using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Player
{
        public class PlayerLevel : NetworkBehaviour {

        public int level = 1;
        public int experience = 0;

        public Slider slider;

        public PlayerPerks perks;
        Text text;

        void Start () {
            perks = GetComponent<PlayerPerks>();
            var lvlText = GameObject.Find("LevelText");
            text = lvlText.GetComponent<Text>();
        }
        
        // Update is called once per frame
        void Update () {
            if (isServer) return;

            text.text = "Level: " + level;
            slider.maxValue = level;
            slider.value = experience;
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
