using UnityEngine;
using UnityEngine.UI;

namespace Player
{
        public class PlayerLevel : MonoBehaviour {

        public int level = 1;
        public int experience = 0;

        public Text text;
        public Slider slider;

        public PlayerPerks perks;

        // Use this for initialization
        void Start () {
            perks = GetComponent<PlayerPerks>();
        }
        
        // Update is called once per frame
        void Update () {
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
