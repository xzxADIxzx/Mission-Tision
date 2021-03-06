using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum firsec {first, second};
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
    [SerializeField] GameObject muzle;
    [SerializeField] int HMBbSiSAMpC;
    // How Many Bullets be Shooted in Semi Automatic Mode per Click 
}