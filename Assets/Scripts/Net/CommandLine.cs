using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandLine : MonoBehaviour
{
    [SerializeField] private NetTest netTst;

    void Start()
    {
        if (Application.isEditor) return;

        var args = System.Environment.GetCommandLineArgs();
        if (args[1] == "-server") netTst.StartServer();
    }
}
