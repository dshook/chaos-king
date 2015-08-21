using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{

    public InputField serverIp;
    public CustomNetManager netManager;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void JoinGame()
    {
        if (!string.IsNullOrEmpty(serverIp.text))
        {
            netManager.JoinGame(serverIp.text);
        }
    }
}
