using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public async void PickMag()
    {
        if(isBusy)
        {
            isBusy = true;
            GameObject mag = current.joint.GetChild(0).gameObject;
            Animator anim = rightHand.GetChild(0).GetComponent<Animator>();
            if(mag != null)
                mag.transform.SetParent(rightHand.GetChild(0));
            anim.SetTrigger("PickMag");
            await Task.Delay(800);
            if(mag != null)
                Destroy(mag);
            mag = Instantiate(current.magazine, rightHand.GetChild(0));
            await Task.Delay(800);
            mag.transform.SetParent(current.joint);
            mag.transform.Reset();
            isBusy = false;
        }
    }

    public void DropMag()
    {
        if(isBusy)
        {
            GameObject mag = current.joint.GetChild(0).gameObject;
            mag.GetComponent<Rigidbody2D>().simulated = true;
            mag.transform.SetParent(TrashObj.t);
        }
    }
}