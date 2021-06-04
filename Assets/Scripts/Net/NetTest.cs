using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Transports.UNET;

public class NetTest : MonoBehaviour
{
    [SerializeField] private LobbyManagerClient LMC;
    [SerializeField] private LobbyManagerServer LMS;
    [SerializeField] private NetworkManager NetMan;
    [SerializeField] private UNetTransport uNetTransport;
    [SerializeField] private Text pname;
    [SerializeField] private Text pdesc;

    public void StartServer()
    {
        Debug.Log("Starting Server");
        NetMan.StartServer();
        LMS.AddCallbacks();
        Debug.Log("Server started");
    }

    public void StartClient()
    {
        Debug.Log("Starting Client");
        NetMan.NetworkConfig.ConnectionData = LMConvert.ToByte(pname.text, pdesc.text, 0, .588f, 1);
        NetMan.StartClient();
        Debug.Log("Client started");
    }

    public void StopServer()
    {
        Debug.Log("Stoping Server");
        NetMan.StopServer();
        Debug.Log("Server stoped");
    }

    public void StopClient()
    {
        Debug.Log("Stoping Client");
        NetMan.StopClient();
        Debug.Log("Client stoped");
    }

    public void ChangeIP(string ip)
    {
        uNetTransport.ConnectAddress = ip;
    }

    public void ChangePort(string port)
    {
        uNetTransport.ConnectPort = Convert.ToInt32(port);
        uNetTransport.ServerListenPort = Convert.ToInt32(port);
    }
}
