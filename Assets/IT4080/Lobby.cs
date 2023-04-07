using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class Lobby : NetworkBehaviour
{
    public It4080.ConnectedPlayers connectedPlayers;

    private Button btnStart;

    private void Start()
    {
        btnStart = GameObject.Find("start").GetComponent<Button>();

        btnStart.onClick.AddListener(btnStartOnClick);

        btnStart.gameObject.SetActive(IsHost);
    }


    public override void OnNetworkSpawn()
    {
        InitialClear();
        if (IsClient)
        {

            NetworkHandler.Singleton.allPlayers.OnListChanged += ClientOnAllPlayersChanged;

            AddPlayersUsingPlayerData(NetworkHandler.Singleton.allPlayers);

            if (IsHost)
            {
                NetworkManager.OnClientDisconnectCallback += ClientOnDisconnect;
            }

        }
       

    }

    public void InitialClear()
    {
        connectedPlayers.Clear();

    }

    public void AddPlayersUsingPlayerData(NetworkList<It4080.PlayerData> players)
    {
        connectedPlayers.Clear();
        
        foreach (It4080.PlayerData player in players)
        {
            var card = AddPlayerCard(player.clientId);

            card.SetReady(player.isReady);

            string status = "Not Ready";

            if (player.isReady)
            {
                status = "Ready";
            }
            card.SetStatus(status);

            // Connect to the ReadyToggled event on the PlayerCard if we are
            // the player represented by the card we are creating.
            if (player.clientId == NetworkManager.Singleton.LocalClientId)
            {
                card.ReadyToggled += ClientOnReadyToggled;
            }

            // If we are the host then we can connect to the KickPlayer event
            // on the PlayerCard.  This allows us to then kick the player when
            // the button is pressed.
            if (IsHost)
            {
                card.KickPlayer += HostKickPlayer;
            }
        }
    }


    private void HostKickPlayer(ulong clientId)
    {
        Debug.Log($"Get outta here {clientId}");
    }

    private It4080.PlayerCard AddPlayerCard(ulong clientId)
    {
        It4080.PlayerCard card = connectedPlayers.AddPlayer("temp", clientId);

        string ready = "";

        string type = "";

        card.ShowKick(IsServer);

        if (clientId == NetworkManager.LocalClientId)
        {
            ready = "(you)";
            card.ShowReady(true);
        }
        else
        {
            ready = "";
            card.ShowReady(false);
        }

        if (clientId == NetworkManager.ServerClientId)
        {
            type = "Host";
            card.ShowReady(false);
            card.ShowKick(false);
        }
        else
        {
            type = "Player";
        }
        card.SetPlayerName($"{type} {clientId} {ready}");

        return card;
    }

    private void ClientOnDisconnect(ulong clientId)
    {
        Debug.Log($"Client {clientId} has disconnected");
    }

    private void ClientOnAllPlayersChanged(NetworkListEvent<It4080.PlayerData> change)
    {
        AddPlayersUsingPlayerData(NetworkHandler.Singleton.allPlayers);
    }

    private void ClientOnReadyToggled(bool isReady)
    {
        Debug.Log($"Ready = {isReady}");
        EnableStartIfAllReady();
    }

    private void btnStartOnClick()
    {
        StartGame();
    }

    private void StartGame()
    {
        NetworkManager.SceneManager.LoadScene("Arena1",
            UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private void EnableStartIfAllReady()
    {
        int ready = 0;
        foreach (It4080.PlayerData players in NetworkHandler.Singleton.allPlayers)
        {
            if (players.isReady)
            {
                ready++;
            }
        }

        // This should be .Count -1 since the Host does not have a ready button.
        btnStart.enabled = ready == NetworkHandler.Singleton.allPlayers.Count;
        if (btnStart.enabled)
        {
            btnStart.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
        } else
        {
            btnStart.GetComponentInChildren<TextMeshProUGUI>().text = "Players not yet ready";
        }
    }
}
