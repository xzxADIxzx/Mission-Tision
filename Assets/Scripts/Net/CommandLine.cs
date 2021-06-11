using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;

public class CommandLine : MonoBehaviour
{
    [SerializeField] private LobbyManagerServer LMS;
    [SerializeField] private NetworkManager netMan;
    [SerializeField] private UNetTransport uNetTransport;

    void Start()
    {
        if (Application.isEditor) return;
        var args = Environment.GetCommandLineArgs();
        if (args.Length > 2) uNetTransport.ServerListenPort = Convert.ToInt32(args[2]);
        if (args[1] == "-server")
        {
            netMan.StartServer();
            LMS.AddCallbacks();
        }
    }
}
