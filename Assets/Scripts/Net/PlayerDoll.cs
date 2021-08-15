using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class PlayerDoll : NetworkBehaviour
{
    [SerializeField] public int PCID;
    [SerializeField] public Vector2 velocity;
    [SerializeField] public Vector2 mouse;
    [SerializeField] private DollManager dm;
    [Header("Lag Compensation")]
    [SerializeField] public ulong addFulc;
    [SerializeField] public ulong addMccm;
    [Header("Status")]
    [SerializeField] public ulong fulc;
    [SerializeField] public ulong mccm;

    [ClientRpc]
    public void InitClientRpc(int PCID)
    {
        Debug.Log("Client: InitClientRpc " + PCID);
        this.PCID = PCID;
    }

    [ClientRpc]
    public void UpdateClientRpc(ulong fulc, bool down, Vector2 pos, Vector2 vel, Vector2 mos)
    {
        if (this.fulc + mccm > fulc && !down) return; // lag compensation
        transform.position = pos;
        velocity = vel;
        mouse = mos;

        this.fulc = fulc;
        this.mccm = 0;
    }

    public void ClientUpdate(Vector2 velocity, Vector2 mouse)
    {
        if (velocity != Vector2.zero) { fulc += addFulc; mccm = addMccm; }
        this.velocity = velocity;
        this.mouse = mouse;
    }

    public void LookAt(Vector2 mouse)
    {
        Vector3 direction = ((Vector3)mouse - transform.position - new Vector3(0, 0.6f, 0)).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        dm.SetLeftPalmPos(0.86f, angle);
        dm.SetRightPalmPos(0.56f, angle);

        direction = ((Vector3)mouse - transform.position - new Vector3(0, 1.65f, 0)).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        dm.SetHeadRot(angle);
    }

    void FixedUpdate()
    {
        transform.Translate(velocity);
        LookAt(mouse);
        if (Vars.sin.NTM.IsServer) fulc++;
    }
}
