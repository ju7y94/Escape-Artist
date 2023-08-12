using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] TMP_InputField lobbyInputField;
    [SerializeField] Button submitLobbyCodeButton;
    [SerializeField] Button quickJoinLobbyButton;
    [SerializeField] Button listLobbiesButton;
    VirtualLobbyManager lobbyManager;

    [SerializeField] TMP_InputField lobbyName;
    bool privateLobby;
    [SerializeField] Button createLobbyButton;

    void Start()
    {
        lobbyManager = FindObjectOfType<VirtualLobbyManager>().GetComponent<VirtualLobbyManager>();
    }

    private void Awake() {
        submitLobbyCodeButton.onClick.AddListener(()=> {
            lobbyManager.JoinLobbyByCode(lobbyInputField.text);
        });

        quickJoinLobbyButton.onClick.AddListener(()=> {
            lobbyManager.QuickJoinLobby();
        });

        createLobbyButton.onClick.AddListener(()=> {
            lobbyManager.CreateLobby(lobbyName.text, privateLobby);
        });

        listLobbiesButton.onClick.AddListener(()=> {
            lobbyManager.ListLobbies();
        });
    }

    public void PrivateToggle(bool toggle)
    {
        Debug.Log(toggle);
        privateLobby = toggle;
    }
}
