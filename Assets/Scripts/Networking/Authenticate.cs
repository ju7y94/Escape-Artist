using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Authenticate : MonoBehaviour
{
    [SerializeField] TMP_InputField playerName;
    [SerializeField] Button authenticateButton;
    VirtualLobbyManager lobbyManager;
    

    void Start()
    {
        lobbyManager = FindObjectOfType<VirtualLobbyManager>().GetComponent<VirtualLobbyManager>();
    }

    void Awake() {
        authenticateButton.onClick.AddListener(()=> {
            lobbyManager.Authenticate(playerName.text);
        });
    }
}
