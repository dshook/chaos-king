using UnityEngine;
using UnityEngine.UI;
using Player;

namespace UI
{
    public class LevelUI : MonoBehaviour
    {
        GameObject _player;
        public GameObject player {
            get { return _player; }
            set
            {
                _player = value;
                if (_player != null)
                {
                    playerLevel = _player.GetComponent<PlayerLevel>();
                }
                else
                {
                    playerLevel = null;
                }
            }
        }

        Text levelText;
        Slider xpSlider;
        PlayerLevel playerLevel;

        // Use this for initialization
        void Start()
        {
            levelText = GetComponentInChildren<Text>();
            xpSlider = GetComponentInChildren<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerLevel != null)
            {
                levelText.text = "Level: " + playerLevel.level;
                xpSlider.maxValue = playerLevel.level;
                xpSlider.value = playerLevel.experience;
            }
        }
    }
}
