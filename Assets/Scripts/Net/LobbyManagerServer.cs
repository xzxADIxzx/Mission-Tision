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

    public Player(ulong id, string name, string desc, Color color)
    {
        this.id = id;
        this.name = name;
        this.desc = desc;
        this.color = color;
    }

    public void NetworkSerialize(NetworkSerializer serializer)
    {
        serializer.Serialize(ref id);
        serializer.Serialize(ref name);
        serializer.Serialize(ref desc);
        serializer.Serialize(ref color);
    }
}

public class LobbyManagerServer : NetworkBehaviour
{
    [Header("Lobby")]
    [SerializeField] private List<Player> team1;
    [SerializeField] private List<Player> team2;
    [SerializeField] private Player newPlayer;
    [Header("Match")]
    [SerializeField] private string mode;
    [SerializeField] private string map;
    [Header("Scripts")]
    [SerializeField] private LobbyManagerClient LMC;
    [SerializeField] private NetworkManager netMan;

    public void Approve(byte[] data, ulong id, NetworkManager.ConnectionApprovedDelegate callback)
    {
        Debug.Log("Server: Approve " + (team1.Count + team2.Count< 10));
        newPlayer = LMConvert.ToPlayer(data, id);
        callback(false, null, team1.Count + team2.Count < 10, Vector3.zero, Quaternion.identity);
    }

    public void OnConnected(ulong id)
    {
        Debug.Log("Server: OnConnected " + id);
        if (team1.Count <= team2.Count) team1.Add(newPlayer);
        else team2.Add(newPlayer);
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map);
    }

    public void OnDisconnected(ulong id)
    {
        Player player = new Player();
        foreach (Player p in team1) if (p.id == id) player = p;
        foreach (Player p in team2) if (p.id == id) player = p;
        team1.Remove(player);
        team2.Remove(player);
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map);
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
    }
}
