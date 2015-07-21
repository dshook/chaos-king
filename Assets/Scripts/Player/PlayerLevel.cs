using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLevel : MonoBehaviour {

    public int level = 1;
    public int experience = 0;

    public Text text;
    public Slider slider;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        text.text = "Level: " + level;
        slider.maxValue = level;
        slider.value = experience;
    }

    public void GetExperience(int amount) {
        experience += amount;
        if(experience > level) {
            LevelUp();
        }
    }

    void LevelUp() {
        level++;
        experience -= level;
    }
}
