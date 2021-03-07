using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] Weapon first;
    [SerializeField] Weapon second;
    [Header("Joints")]
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform body;
    [SerializeField] Transform rightLeg;
    [Header("Status")]
    [SerializeField] bool isBusy;
    [SerializeField] Weapon current;

    public void PickMag()
    {

    }

    public void DropMag()
    {
        GameObject mag = current.joint.GetChild(0).gameObject;
        mag.GetComponent<Rigidbody2D>().simulated = true;
        mag.transform.SetParent(Trash.t);
    }
}