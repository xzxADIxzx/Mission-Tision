using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class MatchManagerServer : NetworkBehaviour
{
    [Header("Match")]
    [SerializeField] private Player[] team1;
    [SerializeField] private Player[] team2;
    [SerializeField] private List<PlayerContoller> team1Controller;
    [SerializeField] private List<PlayerContoller> team2Controller;
    [SerializeField] private string mode;
    [SerializeField] private Vector3[] spawnPoint;
    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;

    public async void Init(Player[] team1, Player[] team2, string mode, Vector3[] spawnPoint)
    {
        await Task.Delay(1);
        Debug.Log("Server: Init");
        this.team1 = team1;
        this.team2 = team2;
        this.mode = mode;
        this.spawnPoint = spawnPoint;

        // Создаём игроков
        for (int i = 0; i < team1.Length; i++)
        {
            team1Controller.Add(Instantiate(playerPrefab, spawnPoint[i], Quaternion.identity).GetComponent<PlayerContoller>());
            team1Controller[i].PCID = i;
        }
        for (int i = 0; i < team2.Length; i++)
        {
            team2Controller.Add(Instantiate(playerPrefab, spawnPoint[i + 5], Quaternion.identity).GetComponent<PlayerContoller>());
            team2Controller[i].PCID = i + 5;
        }
        foreach (PlayerContoller pc in team1Controller) pc.GetComponent<NetworkObject>().Spawn();
        foreach (PlayerContoller pc in team2Controller) pc.GetComponent<NetworkObject>().Spawn();
        for (int i = 0; i < team1.Length; i++) team1Controller[i].InitClientRpc(i);
        for (int i = 0; i < team2.Length; i++) team2Controller[i].InitClientRpc(i + 5);
    }

    public PlayerContoller GetPC(int PCID)
    {
        foreach (PlayerContoller pc in team1Controller) if (pc.PCID == PCID) return pc;
        foreach (PlayerContoller pc in team2Controller) if (pc.PCID == PCID) return pc;
        return null;
    }

    [ServerRpc(RequireOwnership = false)]
    public async void MovePCServerRpc(int PCID, Vector3 move)
    {
        Debug.Log("Server: MovePCServerRpc");
        await Task.Delay(200);
        GetPC(PCID).transform.Translate(move);
    }

    void Start()
    {
        if (IsServer) GetComponent<NetworkObject>().Spawn();
    }
}
