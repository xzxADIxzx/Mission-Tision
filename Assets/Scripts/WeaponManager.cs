using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public enum com {blue, red, bots}
    [Header("Weapons")]
    [SerializeField] com command;
    [SerializeField] Weapon first;
    [SerializeField] Weapon second;
    [SerializeField] GameObject thirt; //if(GetComponent<Granade>()) Boom(); else Heal();
    [Header("Joints")]
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform body;
    [SerializeField] Transform rightLeg;
    [Header("Status")]
    [SerializeField] bool isBusy;
    [SerializeField] Weapon current;

    public void SwitchWeapon(bool isFirst, bool isSecond, bool isThirt){}

    public void GiveWeapon(GameObject weapon, bool isFirst)
    {
        if(isBusy)
            return;
        isBusy = true;
        if(isFirst)
        {
            first = Instantiate(weapon, body).GetComponent<Weapon>();
            first.transform.Reset(new Vector3(-1, 0, 0), new Vector3(0, 0, 90));
        }
        else
        {
            second = Instantiate(weapon, body).GetComponent<Weapon>();
            second.transform.Reset(new Vector3(-1, 0, 0), new Vector3(0, 0, 90));
        }
        GameObject mag = Instantiate(current.magazine, rightHand.GetChild(0));
        if(isFirst)
            mag.transform.SetParent(current.joint);
        else
            mag.transform.SetParent(current.joint);
        mag.transform.Reset();
        SwitchWeapon(isFirst, !isFirst, false);
        isBusy = false;
    }

    public void PickWeapon(){} //первое это DropWeapon потом замена на новый обьект

    public void DropWeapon()
    {
        if(isBusy)
            return;
        current.GetComponent<Rigidbody2D>().simulated = true;
        current.transform.SetParent(TrashObj.t);
    }

    public async void PickMag()
    {
        if(isBusy)
            return;
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

    public void DropMag()
    {
        if(isBusy)
            return;
        GameObject mag = current.joint.GetChild(0).gameObject;
        mag.GetComponent<Rigidbody2D>().simulated = true;
        mag.transform.SetParent(TrashObj.t);
    }
}