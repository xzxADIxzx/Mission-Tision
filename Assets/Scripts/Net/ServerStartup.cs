using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerStartup : MonoBehaviour
{
    public void CheckArgs()
    {
        if (Application.isEditor) return;
        var args = Environment.GetCommandLineArgs();
        if (args[1] == "-server")
        {
            if (args.Length > 2)
                GlobalManager.sin.UNT.ServerListenPort = Convert.ToInt32(args[2]);

            GlobalManager.sin.NTM.StartServer();
            GlobalManager.sin.LMS.AddCallbacks();
        }
    }

    public void StartTemp()
    {
        GlobalManager.sin.NTM.StartServer();
        GlobalManager.sin.LMS.AddCallbacks();
    }
}
