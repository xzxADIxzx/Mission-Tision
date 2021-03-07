using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum firsec {first, second};
    public enum modes {single, semiAutomatic, auto}
    [Header("Main")]
    [SerializeField] string name;
    [SerializeField] string desc;
    [SerializeField] firsec type;
    [Header("Modes")]
    [SerializeField] bool single;
    [SerializeField] bool semiAutomatic;
    [SerializeField] bool auto;
    [Header("Delays")]
    [SerializeField] int singleDelay;
    [SerializeField] int semiAutomaticDelay;
    [SerializeField] int autoDelay;
    [Header("Bullet")]
    [SerializeField] float speed;
    [SerializeField] float scatter;
    [SerializeField] float recoil; 
    [SerializeField] Transform muzle;
    [SerializeField] int HMBbSiSAMpC;
    // How Many Bullets be Shooted in Semi Automatic Mode per Click
    [Header("Magazine")]
    [SerializeField] public Transform joint;
    [SerializeField] GameObject magazine;
    [SerializeField] int maxAmmo;
    [SerializeField] int[] magazines;
    [Header("Status")]
    [SerializeField] modes mode;
    [SerializeField] bool isAiming;
    [SerializeField] bool isRealoding;
    [SerializeField] int currentMagazine;
    [Space(10)]
    public WeaponManager weaponManager;

    public async void Reload()
    {
        isRealoding = true;
        if(magazines[currentMagazine] == 0)
            weaponManager.DropMag();
        int maxMag = -1;
        for(int i = 0; i < magazines.Length; i++)
            if(magazines[i] > maxMag && magazines[i] != 0)
                maxMag = i;
        if(maxMag == -1)
            Debug.Log("Magazines is Empty");
        else
        {
            currentMagazine = maxMag;
            weaponManager.PickMag();
            await Task.Delay(1000);
        }
        isRealoding = false;
    }

    public void FillMag()
    {
        for(int i = 0; i < magazines.Length; i++)
            magazines[i] = maxAmmo;
    }

    public void MoveMode()
    {
        switch(mode)
        {
            case modes.single:
                if(semiAutomatic)
                    mode = modes.semiAutomatic;
                else if(auto)
                    mode = modes.auto;
                break;
            case modes.semiAutomatic:
                if(auto)
                    mode = modes.auto;
                else if(single)
                    mode = modes.single;
                break;
            case modes.auto:
                if(single)
                    mode = modes.single;
                else if(semiAutomatic)
                    mode = modes.semiAutomatic;
                break;
        }
    }

    void Start()
    {
        FillMag();
    }
}