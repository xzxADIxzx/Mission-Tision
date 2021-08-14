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
    public bool isLoad;

    public Player(ulong id, string name, string desc, Color color)
    {
        this.id = id;
        this.name = name;
        this.desc = desc;
        this.color = color;
        this.isLobby = false;
        this.isLoad = false;
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
    [SerializeField] private List<ulong> ready;
    [Header("Match")]
    [SerializeField] private string mode;
    [SerializeField] private string map;
    [SerializeField] private List<string> modes;
    [SerializeField] private GameObject MMPrefab;
    [Header("Status")]
    [SerializeField] public bool isReady;
    [SerializeField] public bool isStart;
    [SerializeField] public bool isLoad;
    [SerializeField] public bool isClientLoad;
    [Header("Scripts")]
    [SerializeField] private LobbyManagerClient LMC;
    [SerializeField] private MatchManagerServer MMS;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private NetworkManager netMan;

    public IEnumerator StartMatch()
    {
        Debug.Log("Server: Start");
        LMC.StartClientRpc(3);
        yield return new WaitForSeconds(1);
        LMC.StartClientRpc(2);
        yield return new WaitForSeconds(1);
        LMC.StartClientRpc(1);
        yield return new WaitForSeconds(1);
        LMC.StartClientRpc(0, this.map, mode);
        Map map = GlobalManager.sin.MAP.GetMap(this.map);
        sceneLoader.Load(map.scene, delegate { isLoad = true; StartCoroutine(WaitForLoad()); });
        isStart = true;
    }

    public IEnumerator WaitForLoad()
    {
        for (int i = 0; i < 10; i++)
        {
            if (isLoad && isClientLoad) break;
            yield return new WaitForSeconds(3);
        }
        if (!isLoad)
        {
            // Your Server is too slow...
            foreach (Player p in team1) GlobalManager.sin.NTM.DisconnectClient(p.id);
            foreach (Player p in team2) GlobalManager.sin.NTM.DisconnectClient(p.id);
        }

        // Some Code for Init Match...
    }

    public ClientRpcParams SendTo(ulong id)
    {
        return new ClientRpcParams()
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { id}
            }
        };
    }

    public Player GetPlayer(ulong id)
    {
        foreach (Player p in team1) if (p.id == id) return p;
        foreach (Player p in team2) if (p.id == id) return p;
        return new Player();
    }

    public void Approve(byte[] data, ulong id, NetworkManager.ConnectionApprovedDelegate callback)
    {
        bool approve = team1.Count + team2.Count < 10 && !isReady;
        Debug.Log("Server: Approve " + approve);
        newPlayer = LMConvert.ToPlayer(data, id);
        if (NetworkManager.ConnectedClientsList.Count == 0)
        {
            newPlayer.isLobby = true;
            lobbyID = id;
        }
        callback(false, null, approve, Vector3.zero, Quaternion.identity);
    }

    public void OnConnected(ulong id)
    {
        Debug.Log("Server: OnConnected " + id);
        if (team1.Count <= team2.Count) team1.Add(newPlayer);
        else team2.Add(newPlayer);
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map, ready.ToArray());
        LMC.ActivateLobbyButtonsClientRpc(SendTo(lobbyID));
    }

    public void OnDisconnected(ulong id)
    {
        Debug.Log("Server: OnDesconnected " + id);
        Player player = new Player();
        foreach (Player p in team1) if (p.id == id) player = p;
        foreach (Player p in team2) if (p.id == id) player = p;
        team1.Remove(player);
        team2.Remove(player);
        ready.Remove(id);
        if(team1.Count + team1.Count == 0)
        {
            StopAllCoroutines();
            isReady = false;
        }
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
        if (isStart) return; // Что бы во время матча он не попытался обновить интерфейс у клиента
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map, ready.ToArray());
        LMC.ActivateLobbyButtonsClientRpc(SendTo(lobbyID));
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
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map, ready.ToArray());
        LMC.ActivateLobbyButtonsClientRpc(SendTo(lobbyID));
    }

    [ServerRpc(RequireOwnership = false)]
    public void NextModeServerRpc(ulong id)
    {
        Debug.Log("Server: NextModeServerRpc");
        if (id != lobbyID) return;
        int index = modes.IndexOf(mode) + 1;
        if (index > modes.Count - 1) index = 0;
        mode = modes[index];
        if (!GlobalManager.sin.MAP.GetMap(map).availableMode.Contains(mode)) map = GlobalManager.sin.MAP.GetMaps(mode)[0].name;
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map, ready.ToArray());
        LMC.ActivateLobbyButtonsClientRpc(SendTo(lobbyID));
    }

    [ServerRpc(RequireOwnership = false)]
    public void PrevModeServerRpc(ulong id)
    {
        Debug.Log("Server: PrevModeServerRpc");
        if (id != lobbyID) return;
        int index = modes.IndexOf(mode) - 1;
        if (index < 0) index = modes.Count - 1;
        mode = modes[index];
        if (!GlobalManager.sin.MAP.GetMap(map).availableMode.Contains(mode)) map = GlobalManager.sin.MAP.GetMaps(mode)[0].name;
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map, ready.ToArray());
        LMC.ActivateLobbyButtonsClientRpc(SendTo(lobbyID));
    }

    [ServerRpc(RequireOwnership = false)]
    public void NextMapServerRpc(ulong id)
    {
        Debug.Log("Server: NextMapServerRpc");
        if (id != lobbyID) return;
        List<Map> maps = GlobalManager.sin.MAP.GetMaps(mode);
        int index = maps.IndexOf(GlobalManager.sin.MAP.GetMap(map)) + 1;
        if (index > maps.Count - 1) index = 0;
        map = maps[index].name;
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map, ready.ToArray());
        LMC.ActivateLobbyButtonsClientRpc(SendTo(lobbyID));
    }

    [ServerRpc(RequireOwnership = false)]
    public void PrevMapServerRpc(ulong id)
    {
        Debug.Log("Server: PrevMapServerRpc");
        if (id != lobbyID) return;
        List<Map> maps = GlobalManager.sin.MAP.GetMaps(mode);
        int index = maps.IndexOf(GlobalManager.sin.MAP.GetMap(map)) - 1;
        if (index < 0) index = maps.Count - 1;
        map = maps[index].name;
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map, ready.ToArray());
        LMC.ActivateLobbyButtonsClientRpc(SendTo(lobbyID));
    }

    [ServerRpc(RequireOwnership = false)]
    public void KickServerRpc(ulong id, ulong clientId)
    {
        Debug.Log("Server: KickServerRpc");
        if (id != lobbyID) return;
        NetworkManager.DisconnectClient(clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReadyServerRpc(ulong id)
    {
        Debug.Log("Server: ReadyServerRpc");
        if (ready.Contains(id)) ready.Remove(id);
        else ready.Add(id);
        if (ready.Count == team1.Count + team2.Count) { StartCoroutine(StartMatch()); isReady = true; }
        else if (isReady == true) { StopAllCoroutines(); isReady = false; LMC.StartClientRpc(4); }
        LMC.UpdateClientRpc(team1.ToArray(), team2.ToArray(), mode, map, ready.ToArray());
        LMC.ActivateLobbyButtonsClientRpc(SendTo(lobbyID));
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendServerRpc(ulong id, string msg)
    {
        Debug.Log("Server: SendServerRpc");
        LMC.SendClientRpc(GetPlayer(id).name, msg);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoadedServerRpc(ulong id)
    {
        Debug.Log("Server: LoadedServerRpc");
        Player player = new Player();
        foreach (Player p in team1) { if (p.id == id) { player = p; } }
        List<Player> team = player.id == id ? team1 : team2;
        foreach (Player p in team2) { if (p.id == id) { player = p; } }
        int pos = team.IndexOf(player);
        player.isLoad = true;
        team.RemoveAt(pos);
        team.Insert(pos, player);

        bool[] t1 = team1.ConvertAll(new Converter<Player, bool>(delegate(Player p) { return p.isLoad; })).ToArray();
        bool[] t2 = team2.ConvertAll(new Converter<Player, bool>(delegate(Player p) { return p.isLoad; })).ToArray();
        GlobalManager.sin.LMC.UpdatePreviewClientRpc(t1, t2);

        bool load = true;
        foreach (bool l in t1) { if (!l) load = false; break; }
        foreach (bool l in t2) { if (!l) load = false; break; }
        isClientLoad = load;
    }
}
