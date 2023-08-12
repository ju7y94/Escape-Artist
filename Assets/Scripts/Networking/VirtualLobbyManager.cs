using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class VirtualLobbyManager : MonoBehaviour
{

    public static VirtualLobbyManager Instance { get; private set; }

    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartBTimer;
    private float lobbyUpdateTimer;
    private string playerName;
    [SerializeField] TMP_InputField playerNameField;
    private float refreshLobbyListTimer = 5f;
    private float lobbyPollTimer;
    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_START_GAME = "0";
    string joinCode;
    [SerializeField] GameObject relayPanel;
    [SerializeField] GameObject startGameButton, backgroundImage;


    public async void Authenticate(string playerName) {
        this.playerName = playerName;
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(playerName);

        await UnityServices.InitializeAsync(initializationOptions);

        AuthenticationService.Instance.SignedIn += () => {
            // do nothing
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);
            Debug.Log("You have signed in as " + "' " + playerName + " '");

            RefreshLobbyList();
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Awake()
    {
        Instance = this;
    }

        private void Update() 
    {
        HandleLobbyHBeat();
        HandleLobbyPolling();
    }
    
    public async void CreateLobby(string lobbyName, bool isPrivate) {
        Player player = GetPlayer();

        CreateLobbyOptions options = new CreateLobbyOptions {
            Player = player,
            IsPrivate = isPrivate,
            Data = new Dictionary<string, DataObject> {
                {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0")}
            }
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, 2, options);

        joinedLobby = lobby;

        Debug.Log("Created Lobby " + lobby.Name + ". Lobby code: " + lobby.LobbyCode);
        joinCode = joinedLobby.LobbyCode;
    }

    public string GetLobbyCode()
    {
        return joinCode;
    }

    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions {
                Count = 25,
                Filters = new List<QueryFilter>{new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                new QueryFilter(QueryFilter.FieldOptions.S1, "Difficulty-Hard", QueryFilter.OpOptions.EQ)},
                Order = new List<QueryOrder>{
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results)
            {
            Debug.Log("Room " + lobby.Name + " has a limit of " + lobby.MaxPlayers + " players.");
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinLobbyByCode(string lobbyCode) {
        try
        {
            Player player = GetPlayer();

            Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, new JoinLobbyByCodeOptions {
            Player = player
        });

        joinedLobby = lobby;
        Debug.Log("Joined lobby with code: " + lobbyCode);

        startGameButton.SetActive(false);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
        
    }

    public async void JoinFirstLobbyAvailable()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(); //Client joins first session in query
            Lobby lobby = await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
            joinedLobby = lobby;
            Debug.Log("Joined first available lobby: " + joinedLobby.Name);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
        
    }

    public async void QuickJoinLobby()
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            joinedLobby = lobby;

            startGameButton.SetActive(false);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
        Debug.Log("Joined a random lobby: " + joinedLobby.Name);
    }

    public async void RefreshLobbyList() {
        try {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;

            // Filter for open lobbies only
            options.Filters = new List<QueryFilter> {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0")
            };

            // Order by newest lobbies first
            options.Order = new List<QueryOrder> {
                new QueryOrder(
                    asc: false,
                    field: QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbyListQueryResponse = await Lobbies.Instance.QueryLobbiesAsync();

        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }
    private void HandleRefreshLobbyList() {
        if (UnityServices.State == ServicesInitializationState.Initialized && AuthenticationService.Instance.IsSignedIn) {
            refreshLobbyListTimer -= Time.deltaTime;
            if (refreshLobbyListTimer < 0f) {
                float refreshLobbyListTimerMax = 5f;
                refreshLobbyListTimer = refreshLobbyListTimerMax;

                RefreshLobbyList();
            }
        }
    }

    private async void HandleLobbyPolling() {
        if (joinedLobby != null) {
            lobbyPollTimer -= Time.deltaTime;
            if (lobbyPollTimer < 0f) {
                float lobbyPollTimerMax = 1.1f;
                lobbyPollTimer = lobbyPollTimerMax;

                joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);


                if (!IsPlayerInLobby()) {
                    // Player was kicked out of this lobby
                    Debug.Log("Kicked from Lobby!");

                    joinedLobby = null;
                }

                if(joinedLobby.Data[KEY_START_GAME].Value != "0")
                {
                    if(!IsLobbyHost())
                    {
                        FindObjectOfType<RelayManager>().JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);
                        relayPanel.SetActive(false);
                    }
                    backgroundImage.SetActive(false);
                }
            }
        }
    }

    private bool IsPlayerInLobby() {
        if (joinedLobby != null && joinedLobby.Players != null) {
            foreach (Player player in joinedLobby.Players) {
                if (player.Id == AuthenticationService.Instance.PlayerId) {
                    // This player is in this lobby
                    return true;
                }
            }
        }
        return false;
    }

    public void PrintPlayers()
    {
        PrintPlayers(joinedLobby);
    }
    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Lobby name: " + lobby.Name);
        foreach (Player player in lobby.Players)
        {
            Debug.Log("Player: " + player.Data["PlayerName"].Value);
        }
    }

    private Player GetPlayer()
    {
        return new Player 
        {
            Data = new Dictionary<string, PlayerDataObject> {
            {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
            }
        };
    }
    public Lobby GetJoinedLobby() {
        return joinedLobby;
    }

    public bool IsLobbyHost() {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

        private async void HandleLobbyHBeat()
    {
        if(hostLobby != null)
        {
            heartBTimer -= Time.deltaTime;
            if(heartBTimer < 0f)
            {
                float heartBTimerMax = 15;
                heartBTimer = heartBTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    private async void HandleLobbyPollForUpdates() {
        if(joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if(lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
            }
        }
    }

    public async void UpdatePlayerName(string playerName) {
        this.playerName = playerName;

        if (joinedLobby != null) {
            try {
                UpdatePlayerOptions options = new UpdatePlayerOptions();

                options.Data = new Dictionary<string, PlayerDataObject>() {
                    {
                        KEY_PLAYER_NAME, new PlayerDataObject(
                            visibility: PlayerDataObject.VisibilityOptions.Public,
                            value: playerName)
                    }
                };

                string playerId = AuthenticationService.Instance.PlayerId;

                Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, playerId, options);
                joinedLobby = lobby;

            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }
    }
    private async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void KickPlayer()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void MigrateHost()
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions{
            HostId = joinedLobby.Players[1].Id
            });
        joinedLobby = hostLobby;

        PrintPlayers(hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void StartGame()
    {
        if (IsLobbyHost())
        {
            try
            {
                Debug.Log("Starting game");
                string relayCode = await FindObjectOfType<RelayManager>().GetComponent<RelayManager>().CreateRelay();

                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions {
                    Data = new Dictionary<string, DataObject> {
                        {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode)}
                    }
                });
                joinedLobby = lobby;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
}
