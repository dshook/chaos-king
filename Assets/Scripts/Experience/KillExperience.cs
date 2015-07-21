using UnityEngine;
using System.Collections;
using Player;

public class KillExperience : MonoBehaviour {

    public int experience = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GiveExperience(PlayerLevel player) {
        player.GetExperience(experience);
    }
}
