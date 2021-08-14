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
                Vars.sin.UNT.ServerListenPort = Convert.ToInt32(args[2]);

            Vars.sin.NTM.StartServer();
            Vars.sin.LMS.AddCallbacks();
        }
    }

    public void StartTemp()
    {
        Vars.sin.NTM.StartServer();
        Vars.sin.LMS.AddCallbacks();
    }
}
