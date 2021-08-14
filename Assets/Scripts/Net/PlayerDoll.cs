using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class PlayerDoll : NetworkBehaviour
{
    [SerializeField] public int PCID;
    [SerializeField] public Vector2 velocity;
    [Header("Lag Compensation")]
    [SerializeField] public ulong addFulc;
    [SerializeField] public ulong addMccm;
    [Header("Status")]
    [SerializeField] public ulong fulc;
    [SerializeField] public ulong mccm;

    [ClientRpc]
    public void InitClientRpc(int PCID)
    {
        this.PCID = PCID;
    }

    [ClientRpc]
    public void UpdateClientRpc(ulong fulc, Vector2 pos, Vector2 vel)
    {
        if (this.fulc + mccm > fulc) return; // lag compensation
        transform.position = pos;
        velocity = vel;
        this.fulc = fulc;
        this.mccm = 0;
    }

    public void ClientUpdate(Vector2 velocity)
    {
        this.velocity = velocity;
        if (velocity != Vector2.zero) { fulc += addFulc; mccm = addMccm; }
    }

    void FixedUpdate()
    {
        transform.Translate(velocity);
        if (Vars.sin.NTM.IsServer) fulc++;
    }
}
