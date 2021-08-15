using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class MatchManagerServer : NetworkBehaviour
{
    [Header("Match")]
    [SerializeField] private List<Player> team1;
    [SerializeField] private List<Player> team2;
    [SerializeField] private List<PlayerDoll> team1Doll;
    [SerializeField] private List<PlayerDoll> team2Doll;
    [SerializeField] private Vector3[] spawnPoint;
    [SerializeField] private string map;
    [SerializeField] private string mode;
    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;

    public void Init(List<Player> team1, List<Player> team2, string map, string mode)
    {
        Debug.Log("Server: Init");
        this.team1 = team1;
        this.team2 = team2;
        this.map = map;
        this.mode = mode;
        this.spawnPoint = Vars.sin.MAP.GetMap(map).spanwPoint;

        // Create Players
        for (int i = 0; i < team1.Count; i++)
        {
            team1Doll.Add(Instantiate(playerPrefab, spawnPoint[i], Quaternion.identity).GetComponent<PlayerDoll>());
            team1Doll[i].PCID = i;
        }
        for (int i = 0; i < team2.Count; i++)
        {
            team2Doll.Add(Instantiate(playerPrefab, spawnPoint[i + 5], Quaternion.identity).GetComponent<PlayerDoll>());
            team2Doll[i].PCID = i + 5;
        }
        foreach (PlayerDoll pc in team1Doll) pc.GetComponent<NetworkObject>().Spawn();
        foreach (PlayerDoll pc in team2Doll) pc.GetComponent<NetworkObject>().Spawn();
        for (int i = 0; i < team1.Count; i++) team1Doll[i].InitClientRpc(i);
        for (int i = 0; i < team2.Count; i++) team2Doll[i].InitClientRpc(i + 5);
    }

    private PlayerDoll GetPD(int PCID)
    {
        foreach (PlayerDoll pc in team1Doll) if (pc.PCID == PCID) return pc;
        foreach (PlayerDoll pc in team2Doll) if (pc.PCID == PCID) return pc;
        return null;
    }

    private int GetPCID(ulong id)
    {
        int pcid = 0;
        Player p = Vars.sin.LMS.GetPlayer(id);
        pcid = team1.Contains(p) ? team1.IndexOf(p) : team2.IndexOf(p) + 5;
        return pcid;
    }

    void FixedUpdate()
    {
        foreach (PlayerDoll pc in team1Doll) pc.UpdateClientRpc(pc.fulc, pc.fulc % 300 == 0, pc.transform.position, pc.velocity, pc.mouse);
        foreach (PlayerDoll pc in team2Doll) pc.UpdateClientRpc(pc.fulc, pc.fulc % 300 == 0, pc.transform.position, pc.velocity, pc.mouse);
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateDollServerRpc(ulong id, Vector2 velocity, Vector2 mouse)
    {
        PlayerDoll pd = GetPD(GetPCID(id));
        pd.velocity = velocity;
        pd.mouse = mouse;
    }

    [ServerRpc(RequireOwnership = false)]
    public void GetDollObjServerRpc(ulong id)
    {
        Vars.sin.MMC.SetDollObjClientRpc(GetPCID(id), Vars.sin.LMS.SendTo(id));
    }
}
