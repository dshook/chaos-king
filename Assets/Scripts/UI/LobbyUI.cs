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

    public void HostGame()
    {
        netManager.StartGame();
    }

    public void JoinGame()
    {
        string ipAddress = serverIp.text;
        if (string.IsNullOrEmpty(ipAddress)){
            ipAddress = serverIp.placeholder.GetComponent<Text>().text;
        }
        if (!string.IsNullOrEmpty(ipAddress))
        {
            netManager.JoinGame(ipAddress);
        }
    }
}
