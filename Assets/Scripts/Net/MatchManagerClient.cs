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
    [SerializeField] private Vector2 mouse;
    [Header("Status")]
    [SerializeField] private bool isStarted;

    [ClientRpc]
    public void InitClientRpc()
    {
        Debug.Log("Client: InitClientRpc");
        isStarted = true;

        // Camera Follow
        Vars.sin.MMS.GetDollObjServerRpc(Vars.sin.NTM.LocalClientId);
    }

    [ClientRpc]
    public void SetDollObjClientRpc(int PCID, ClientRpcParams CRP = default)
    {
        Debug.Log("Client: SetDollObjClientRpc");
        foreach (PlayerDoll pd in FindObjectsOfType<PlayerDoll>()) if (pd.PCID == PCID) player = pd;
        Camera.main.GetComponent<CameraFollow>().Follow(player.transform);
    }

    void Update()
    {
        if (!isStarted) return;
        Vector2 vel = new Vector2();
        if (Input.GetKey(KeyCode.W)) vel.y = .1f;
        if (Input.GetKey(KeyCode.S)) vel.y = -.1f;
        if (Input.GetKey(KeyCode.D)) vel.x = .1f;
        if (Input.GetKey(KeyCode.A)) vel.x = -.1f;
        velocity = vel;
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        if (!isStarted) return;
        Vars.sin.MMS.UpdateDollServerRpc(Vars.sin.NTM.LocalClientId, velocity, mouse);
        if (player != null)
            player.ClientUpdate(velocity, mouse); // lag compensation
    }
}
