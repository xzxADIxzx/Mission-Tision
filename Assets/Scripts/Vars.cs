﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;

public class Vars : MonoBehaviour
{
    [Header("Singleton")]
    [SerializeField] public static Vars sin;
    [Header("Managers")]
    [SerializeField] public NetworkManager NTM;
    [SerializeField] public UNetTransport UNT;
    [SerializeField] public LobbyManagerClient LMC;
    [SerializeField] public LobbyManagerServer LMS;
    [SerializeField] public MatchManagerClient MMC;
    [SerializeField] public MatchManagerServer MMS;
    [SerializeField] public SceneLoader SLD;
    [SerializeField] public ServerStartup SSU;
    [SerializeField] public Maps MAP;
    [Header("Objects")]
    [SerializeField] public GameObject NMO; // Network Manager
    [SerializeField] public GameObject LMO; // Lobby Manager
    [SerializeField] public GameObject MMO; // Match Manager
    [SerializeField] public GameObject SLO; // Scene Loader
    [SerializeField] public GameObject SSO; // Server Startup
    [SerializeField] public GameObject MPO; // Map

    void Start()
    {
        sin = this;

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(NMO);
        DontDestroyOnLoad(LMO);
        DontDestroyOnLoad(MMO);
        DontDestroyOnLoad(SLO);
        DontDestroyOnLoad(SSO);
        DontDestroyOnLoad(MPO);

        //Temp lobby
        SLD.StartCoroutine(SLD.Loading("lobby", delegate { LMO.SetActive(true); SSU.CheckArgs(); }));
    }
}
