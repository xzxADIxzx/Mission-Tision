using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Transports.UNET;

public class LobbyManagerClient : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private Text IP;
    [SerializeField] private Text port;
    [SerializeField] private Image[] slot;
    [SerializeField] private List<Item> item = new List<Item>();
    [SerializeField] private GameObject itemPref;
    [SerializeField] private Image map;
    [SerializeField] private Text mapName;
    [SerializeField] private Text mode;
    [SerializeField] private List<Button> match;
    [Header("Scripts")]
    [SerializeField] private Maps maps;
    [SerializeField] private LobbyManagerServer LMS;
    [SerializeField] private UNetTransport uNetTransport;

    public void AddItem(Player player, int i)
    {
        Item newItem = Instantiate(itemPref, slot[i].transform).GetComponent<Item>();
        newItem.name.text = player.name;
        newItem.desc.text = player.desc;
        newItem.head.color = player.color;
        newItem.kick.onClick.AddListener(delegate { LMS.KickServerRpc(NetworkManager.LocalClientId, player.id); });
        if (player.isLobby) newItem.lobby.active = true;
        item.Add(newItem);
    }

    [ClientRpc]
    public void UpdateClientRpc(Player[] team1, Player[] team2, string mode, string map, ClientRpcParams CRP = default)
    {
        Debug.Log("Client: UpdateClientRpc");
        foreach (Item i in item.ToArray()) Destroy(i.gameObject);
        item.Clear();
        IP.text = uNetTransport.ConnectAddress;
        port.text = uNetTransport.ConnectPort.ToString();
        for (int i = 0; i < 5; i++)
        {
            slot[i].color = Color.red;
            if (mode != "Bots attack") slot[i + 5].color = Color.blue;
            else slot[i + 5].color = Color.green;
        }
        for (int i = 0; i < team1.Length; i++) AddItem(team1[i], i);
        for (int i = 0; i < team2.Length; i++) AddItem(team2[i], i + 5);
        this.mode.text = mode;
        this.map.sprite = maps.GetMap(map).image;
        this.mapName.text = maps.GetMap(map).name;
    }

    [ClientRpc]
    public void ActivateLobbyButtonsClientRpc(ClientRpcParams CRP = default)
    {
        foreach (Item i in item) i.kick.interactable = true;
        foreach (Button b in match) b.interactable = true;
    }

    public void ChangeTeam()
    {
        Debug.Log("Client: ChangeTeam");
        LMS.ChangeTeamServerRpc(NetworkManager.LocalClientId);
    }

    public void NextMode()
    {
        Debug.Log("Client: NextMode");
        LMS.NextModeServerRpc(NetworkManager.LocalClientId);
    }

    public void PrevMode()
    {
        Debug.Log("Client: NextMode");
        LMS.PrevModeServerRpc(NetworkManager.LocalClientId);
    }

    public void NextMap()
    {
        Debug.Log("Client: NextMap");
        LMS.NextMapServerRpc(NetworkManager.LocalClientId);
    }

    public void PrevMap()
    {
        Debug.Log("Client: PrevMap");
        LMS.PrevMapServerRpc(NetworkManager.LocalClientId);
    }
}
