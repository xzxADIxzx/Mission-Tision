using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static void Reset(this Transform tran){
    	tran.localPosition = Vector3.zero;
        tran.localRotation = Quaternion.identity;
    }

    public static void Reset(this Transform tran, Vector3 move){
    	tran.localPosition = Vector3.zero + move;
        tran.localRotation = Quaternion.identity;
    }
}