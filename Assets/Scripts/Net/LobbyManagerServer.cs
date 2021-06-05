using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Serialization;

[Serializable]
public struct Player : INetworkSerializable
{
    public ulong id;
    public string name;
    public string desc;
    public Color color;
    public bool isLobby;

    public Player(ulong id, string name, string desc, Color color)
    {
        this.id = id;
        this.name = name;
        this.desc = desc;
        this.color = color;
        this.isLobby = false;
    }

    public void NetworkSerialize(NetworkSerializer serializer)
    {
        serializer.Serialize(ref id);
        serializer.Serialize(ref name);
        serializer.Serialize(ref desc);
        serializer.Serialize(ref color);
        serializer.Serialize(ref isLobby);
    }
}

public class LobbyManagerServer : NetworkBehaviour
{
    [Header("Lobby")]
    [SerializeField] private List<Player> team1;
    [SerializeField] private List<Player> team2;
    [SerializeField] private Player newPlayer;
    [SerializeField] private ulong lobbyID;
    [Header("Match")]
    [SerializeField] private string mode;
    [SerializeField] private string map;
    [SerializeField] private List<string> modes;
    [Header("Scripts")]
    [SerializeField] private Maps maps;
    [SerializeField] private LobbyManagerClient LMC;
    [SerializeField] private NetworkManager netMan;

    public ClientRpcParams SendToLobby()
    {
        return new ClientRpcParams()
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { lobbyID }
            }
        };
    }

    public void Approve(byte[] data, ulong id, NetworkManager.ConnectionApprovedDelegate callback)
    {
        Debug.Log("Server: Approve " + (team1.Count + team2.Count < 10));
        newPlayer = LMConvert.ToPlayer(data, id);
        if (NetworkManager.ConnectedClientsList.Count == 0)
        {
            newPlayer.isLobby = true;
            lobbyID = id;
        }
        callback(false, null, team1.Count + team2.Count < 10, Vector3.zero, Quaternion.identity);
    }

    public void OnConnected(ulong id)
    {
        Debug.Log("Server: OnConnected " + id);
        if (team1.Count <= team2.Count) team1.Add(newPlayer);
        else team2.Add(newPlayer);
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map);
        LMC.ActivateLobbyButtonsClientRpc(SendToLobby());
    }

    public void OnDisconnected(ulong id)
    {
        Debug.Log("Server: OnDesconnected " + id);
        Player player = new Player();
        foreach (Player p in team1) if (p.id == id) player = p;
        foreach (Player p in team2) if (p.id == id) player = p;
        team1.Remove(player);
        team2.Remove(player);
        if(team1.Count > 0) // Ужас
        {
            player = team1[0];
            player.isLobby = true;
            lobbyID = player.id;
            team1[0] = player;
        }
        else if(team2.Count > 0)
        {
            player = team2[0];
            player.isLobby = true;
            lobbyID = player.id;
            team2[0] = player;
        }
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map);
        LMC.ActivateLobbyButtonsClientRpc(SendToLobby());
    }

    public void AddCallbacks()
    {
        Debug.Log("Server: AddCallbaks");
        netMan.ConnectionApprovalCallback += Approve;
        netMan.OnClientConnectedCallback += OnConnected;
        netMan.OnClientDisconnectCallback += OnDisconnected;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeTeamServerRpc(ulong id)
    {
        Debug.Log("Server: ChangeTeamServerRpc");
        bool team;
        Player player = new Player();
        foreach (Player p in team1) if (p.id == id) player = p;
        team = player.id == id;
        foreach (Player p in team2) if (p.id == id) player = p;
        if (team && team2.Count < 5)
        {
            team1.Remove(player);
            team2.Add(player);
        }
        if (!team && team1.Count < 5)
        {
            team2.Remove(player);
            team1.Add(player);
        }
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map);
        LMC.ActivateLobbyButtonsClientRpc(SendToLobby());
    }

    [ServerRpc(RequireOwnership = false)]
    public void NextModeServerRpc(ulong id)
    {
        Debug.Log("Server: NextModeServerRpc");
        if (id != lobbyID) return;
        int index = modes.IndexOf(mode) + 1;
        if (index > modes.Count - 1) index = 0;
        mode = modes[index];
        if (!maps.GetMap(map).availableMode.Contains(mode)) map = maps.GetMaps(mode)[0].name;
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map);
        LMC.ActivateLobbyButtonsClientRpc(SendToLobby());
    }

    [ServerRpc(RequireOwnership = false)]
    public void PrevModeServerRpc(ulong id)
    {
        Debug.Log("Server: PrevModeServerRpc");
        if (id != lobbyID) return;
        int index = modes.IndexOf(mode) - 1;
        if (index < 0) index = modes.Count - 1;
        mode = modes[index];
        if (!maps.GetMap(map).availableMode.Contains(mode)) map = maps.GetMaps(mode)[0].name;
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map);
        LMC.ActivateLobbyButtonsClientRpc(SendToLobby());
    }

    [ServerRpc(RequireOwnership = false)]
    public void NextMapServerRpc(ulong id)
    {
        Debug.Log("Server: NextMapServerRpc");
        if (id != lobbyID) return;
        List<Map> maps = this.maps.GetMaps(mode);
        int index = maps.IndexOf(this.maps.GetMap(map)) + 1;
        if (index > maps.Count - 1) index = 0;
        map = maps[index].name;
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map);
        LMC.ActivateLobbyButtonsClientRpc(SendToLobby());
    }

    [ServerRpc(RequireOwnership = false)]
    public void PrevMapServerRpc(ulong id)
    {
        Debug.Log("Server: PrevMapServerRpc");
        if (id != lobbyID) return;
        List<Map> maps = this.maps.GetMaps(mode);
        int index = maps.IndexOf(this.maps.GetMap(map)) - 1;
        if (index < 0) index = maps.Count - 1;
        map = maps[index].name;
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map);
        LMC.ActivateLobbyButtonsClientRpc(SendToLobby());
    }

    [ServerRpc(RequireOwnership = false)]
    public void KickServerRpc(ulong id, ulong clientId)
    {
        Debug.Log("Server: KickServerRpc");
        NetworkManager.DisconnectClient(clientId);
    }
}
