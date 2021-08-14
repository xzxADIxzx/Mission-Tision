using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class MatchManagerClient : NetworkBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerDoll player;
    [SerializeField] private Vector2 velocity;
    [Header("Status")]
    [SerializeField] private bool isStarted;

    [ClientRpc]
    public void InitClientRpc()
    {
        isStarted = true;

        // Camera Follow
        Vars.sin.MMS.GetDollObjServerRpc(Vars.sin.NTM.LocalClientId);
    }

    [ClientRpc]
    public void SetDollObjClientRpc(int PCID, ClientRpcParams CRP = default)
    {
        foreach (PlayerDoll pd in FindObjectsOfType<PlayerDoll>()) if (pd.PCID == PCID) player = pd;
        Camera.main.GetComponent<CameraFollow>().Follow(player.transform);
    }

    void Update()
    {
        if (!isStarted) return;
        Vector2 vel = new Vector2();
        if (Input.GetKey(KeyCode.W)) vel.y = .2f;
        if (Input.GetKey(KeyCode.S)) vel.y = -.2f;
        if (Input.GetKey(KeyCode.D)) vel.x = .2f;
        if (Input.GetKey(KeyCode.A)) vel.x = -.2f;
        velocity = vel;
    }

    void FixedUpdate()
    {
        if (!isStarted) return;
        Vars.sin.MMS.MoveDollServerRpc(Vars.sin.NTM.LocalClientId, velocity);
        if (player != null) player.ClientUpdate(velocity); // lag compensation
    }
}
