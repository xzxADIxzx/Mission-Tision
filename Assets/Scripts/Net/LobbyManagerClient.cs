using System;
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
    [SerializeField] private GameObject ui;
    [SerializeField] private Text IP;
    [SerializeField] private Text port;
    [SerializeField] private Image[] slot;
    [SerializeField] private List<Item> item;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Image map;
    [SerializeField] private Text mapName;
    [SerializeField] private Text mode;
    [SerializeField] private List<Button> match;
    [SerializeField] private GameObject conBG;
    [SerializeField] private GameObject[] conImg;
    [SerializeField] private GameObject strBG;
    [SerializeField] private Text strTxt;
    [SerializeField] private Transform repeate;
    [SerializeField] private int msgAmount;
    [SerializeField] private float size;
    [Header("Scripts")]
    [SerializeField] private LobbyManagerServer LMS;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private NetworkManager netMan;
    [SerializeField] private UNetTransport uNetTransport;
    [Header("Map Preview")]
    [SerializeField] private GameObject MP;
    [SerializeField] private Image MPImg;
    [SerializeField] private Text MPName;
    [SerializeField] private Text MPDesc;
    [SerializeField] private Image[] point;
    [Header("Temp")]
    [SerializeField] private Text pname;
    [SerializeField] private Text pdesc;

    public void ChangeConnectionAlpha(bool active)
    {
        if (active)
        {
            Alpha.On(conBG, 2, 0, 128, false, true);
            foreach (GameObject obj in conImg) Alpha.On(obj, 1, 0, 255, false, true);
        }
        else
        {
            Alpha.Off(conBG, 2, 128, 0, false, true, false);
            foreach (GameObject obj in conImg) Alpha.Off(obj, 1, 255, 0, false, true, false);
        }
    }

    public void AddItem(Player player, ulong[] ready, int i)
    {
        Item newItem = Instantiate(itemPrefab, slot[i].transform).GetComponent<Item>();
        newItem.name.text = player.name;
        newItem.desc.text = player.desc;
        newItem.head.color = player.color;
        newItem.kick.onClick.AddListener(delegate { LMS.KickServerRpc(NetworkManager.LocalClientId, player.id); });
        List<ulong> readyList = new List<ulong>();
        foreach (ulong id in ready) readyList.Add(id);
        if (readyList.Contains(player.id)) newItem.desc.text = "Ready!";
        if (player.isLobby) newItem.lobby.SetActive(true);
        item.Add(newItem);
    }

    [ClientRpc]
    public void UpdateClientRpc(Player[] team1, Player[] team2, string mode, string map, ulong[] ready, ClientRpcParams CRP = default)
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
        for (int i = 0; i < team1.Length; i++) AddItem(team1[i], ready, i);
        for (int i = 0; i < team2.Length; i++) AddItem(team2[i], ready, i + 5);
        foreach (Button b in match) b.interactable = false;
        this.mode.text = mode;
        this.map.sprite = GlobalManager.sin.MAP.GetMap(map).image;
        this.mapName.text = GlobalManager.sin.MAP.GetMap(map).name;
    }

    [ClientRpc]
    public void ActivateLobbyButtonsClientRpc(ClientRpcParams CRP = default)
    {
        Debug.Log("Client: ActivateLobbyButtonsClientRpc");
        foreach (Item i in item) i.kick.interactable = true;
        foreach (Button b in match) b.interactable = true;
    }

    [ClientRpc]
    public void StartClientRpc(int sec, string map = null, string mode = null, ClientRpcParams CRP = default)
    {
        Debug.Log("Client: StartClientRpc");
        if (sec == 4)
        {
            Alpha.Off(strBG, 1, 128, 0, false, true, false);
            Alpha.Off(strTxt.gameObject, 1, 255, 0, false, true, false);
            return;
        }
        if (sec == 3)
        {
            Alpha.On(strBG, 1, 0, 128, false, true, true);
            Alpha.On(strTxt.gameObject, 1, 0, 255, false, true, true);
        }
        if (sec == 0)
        {
            Action callback = delegate 
            {
                ui.SetActive(false);
                MP.SetActive(true);

                Map m = GlobalManager.sin.MAP.GetMap(map);
                MPName.text = m.name;
                MPImg.sprite = m.image;
                MPDesc.text = GlobalManager.sin.MAP.GetMode(mode).desc;

                GlobalManager.sin.LMS.LoadedServerRpc(GlobalManager.sin.NTM.LocalClientId);
            };
            sceneLoader.Load(GlobalManager.sin.MAP.GetMap(map).scene, callback);
        }
        strTxt.text = sec.ToString();
    }

    [ClientRpc]
    public void UpdatePreviewClientRpc(bool[] team1, bool[] team2, ClientRpcParams CRP = default)
    {
        Debug.Log("Client: UpdatePreviewClientRpc");
        Color red = new Color(1, 0, 0);
        Color green = new Color(0, 1, 0);
        for (int i = 0; i < team1.Length; i++) point[i].color = team1[i] ? green : red;
        for (int i = 0; i < team2.Length; i++) point[i + 5].color = team2[i] ? green : red;
    }

    [ClientRpc]
    public void SendClientRpc(string name, string msg)
    {
        Debug.Log("Client: SendClientRpc");
        msgAmount++;
        Transform instantiate = Instantiate(repeate.GetChild(0), repeate);
        instantiate.Translate(0, -12f * msgAmount * size, 0);
        instantiate.GetComponent<Text>().text = name + ": " + msg;
        repeate.Translate(0, 12f * size, 0);
        if (repeate.childCount > 5)  Alpha.Off(repeate.GetChild(repeate.childCount - 5).gameObject, 1, 255, 0, true);
    }
    
    public void OnDisconnect(ulong id = 0)
    {
        Debug.Log("Client: OnDisconnect");
        if (LMS.isStart) return;
        ChangeConnectionAlpha(true);

        if(strBG.GetComponent<Image>().color.a > 0)
        {
            Alpha.Off(strBG, 1, 128, 0, false, true, false);
            Alpha.Off(strTxt.gameObject, 1, 255, 0, false, true, false);
        }
    }

    public void Connect()
    {
        Debug.Log("Client: Connect");
        netMan.NetworkConfig.ConnectionData = LMConvert.ToByte(pname.text, pdesc.text, 0, .588f, 1);
        netMan.StartClient();
        ChangeConnectionAlpha(false);
    }

    public void Disconnect()
    {
        Debug.Log("Client: Disconnect");
        netMan.StopClient();
        OnDisconnect();
    }

    public void ChangeIP(string ip)
    {
        Debug.Log("Client: ChangeIP");
        if (ip.Length > 0) uNetTransport.ConnectAddress = ip;
    }

    public void ChangePort(string port)
    {
        Debug.Log("Client: ChangePort");
        if (port.Length > 0) uNetTransport.ConnectPort = Convert.ToInt32(port);
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

    public void Ready()
    {
        Debug.Log("Client: Ready");
        LMS.ReadyServerRpc(NetworkManager.LocalClientId);
    }

    public void Send(string msg)
    {
        Debug.Log("Client: Send");
        if (msg.Length > 0) LMS.SendServerRpc(NetworkManager.LocalClientId, msg);
    }

    void Start()
    {
        netMan.OnClientDisconnectCallback += OnDisconnect;
        size = GlobalManager.sin.LMO.transform.GetChild(0).localScale.y;
    }
}
