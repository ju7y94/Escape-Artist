using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RelayUI : MonoBehaviour
{
    [SerializeField] private Button createRelayButton;
    [SerializeField] TMP_Text joinLobbyCode;
    void Awake()
    {
        createRelayButton.onClick.AddListener(()=> {
            FindObjectOfType<VirtualLobbyManager>().GetComponent<VirtualLobbyManager>().StartGame();
        });
    }

    void Update()
    {
        joinLobbyCode.text = "Lobby code: " + FindObjectOfType<VirtualLobbyManager>().GetComponent<VirtualLobbyManager>().GetLobbyCode();
    }
}
