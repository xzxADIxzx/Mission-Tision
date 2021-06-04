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
    [Header("Scripts")]
    [SerializeField] private LobbyManagerServer LMS;
    [SerializeField] private UNetTransport uNetTransport;

    public void AddItem(Player player, int i)
    {
        Item newItem = Instantiate(itemPref, slot[i].transform).GetComponent<Item>();
        newItem.name.text = player.name;
        newItem.desc.text = player.desc;
        newItem.head.color = player.color;
        item.Add(newItem);
    }

    [ClientRpc]
    public void UpdateClientRpc(Player[] team1, Player[] team2, string mode, string map)
    {
        Debug.Log("Client: UpdateClientRpc");
        foreach (Item i in item.ToArray()) Destroy(i.gameObject);
        item.Clear();
        IP.text = uNetTransport.ConnectAddress;
        port.text = uNetTransport.ConnectPort.ToString();
        for (int i = 0; i < 5; i++)
        {
            slot[i].color = Color.red;
            if (mode != "Bots Attack") slot[i + 5].color = Color.blue;
            else slot[i + 5].color = Color.green;
        }
        for (int i = 0; i < team1.Length; i++) AddItem(team1[i], i);
        for (int i = 0; i < team2.Length; i++) AddItem(team2[i], i + 5);
    }

    public void ChangeTeam()
    {
        Debug.Log("Client: ChangeTeam");
        LMS.ChangeTeamServerRpc(NetworkManager.LocalClientId);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
