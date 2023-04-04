using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Unity.Netcode;
public class NetworkHandler : NetworkBehaviour
{
    private static NetworkHandler _instance;
    public static NetworkHandler Singleton
    {
        get
        {
            return _instance;
        }
    }

    public NetworkList<It4080.PlayerData> allPlayers;

    public void Awake()
    {

         allPlayers = new NetworkList<It4080.PlayerData>();

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += ServerOnClientConnected;
            allPlayers.Add(new It4080.PlayerData(NetworkManager.LocalClientId, Color.blue, true));
        }
    }

    public int FindPlayerIndex(ulong clientId)
    {
        var id = 0;
        var found = false;
        while (id < allPlayers.Count && !found)
        {
            if (allPlayers[id].clientId == clientId)
            {
                found = true;
            }
            else
            {
                id += 1;
            }
        }
        if (!found)
        {
            id = 0;
        }
        return id;
    }

    private void ServerOnClientConnected(ulong clientId)
    {
        AddPlayerToList(clientId);
    }

    public void AddPlayerToList(ulong clientId)
    {
        allPlayers.Add(new It4080.PlayerData(clientId, Color.blue, false));
    }


    public It4080.PlayerData GetPlayerFromList(ulong clientId)
    {
        int index = FindPlayerIndex(clientId);
        return allPlayers[index];
    }
}
