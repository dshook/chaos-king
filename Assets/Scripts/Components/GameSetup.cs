using UnityEngine;
using UnityEngine.Networking;
using UI;
using Util;

public class GameSetup : NetworkBehaviour
{
    public GameObject LevelUI;
    public GameObject AmmoUI;


    [ClientRpc]
    public void RpcSetupUI(GameObject player)
    {
        if (LevelUI == null)
        {
            Debug.LogError("Couldn't set up UI with null objects");
            return;
        }
        var levelUIComponent = LevelUI.GetComponent<LevelUI>();
        if (levelUIComponent.player == null)
        {
            levelUIComponent.player = player;
        }

        var ammoUIComponent = AmmoUI.GetComponent<AmmoUI>();
        if (ammoUIComponent.player == null)
        {
            ammoUIComponent.player = player;
        }
    }

}
