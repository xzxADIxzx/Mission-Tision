using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class PlayerContoller : NetworkBehaviour
{
    [SerializeField] public int PCID;
    [SerializeField] public MatchManagerServer MMS;

    [ClientRpc]
    public void InitClientRpc(int PCID)
    {
        Debug.Log("Client: InitClientRpc");
        this.PCID = PCID;
        MMS = FindObjectOfType<MatchManagerServer>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W)) MMS.MovePCServerRpc(PCID, new Vector3(0, .5f, 0));
        if (Input.GetKey(KeyCode.S)) MMS.MovePCServerRpc(PCID, new Vector3(0, -.5f, 0));
        if (Input.GetKey(KeyCode.D)) MMS.MovePCServerRpc(PCID, new Vector3(.5f, 0, 0));
        if (Input.GetKey(KeyCode.A)) MMS.MovePCServerRpc(PCID, new Vector3(-.5f, 0, 0));
    }
}
