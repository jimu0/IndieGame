using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NanguaBoss : MonoBehaviour
{
    public GameObject touObj1;
    public GameObject touObj2;
    public GameObject touObj3;
    public GameObject touObj4;
    public GameObject touObj5;

    public Transform touPos1;
    public Transform touPos2;
    public Transform touPos3;
    public Transform touPos4;
    public Transform touPos5;

    private void Update()
    {
        touObj1.transform.SetPositionAndRotation(touPos1.position,Quaternion.Euler(-90,0,180));
        touObj2.transform.SetPositionAndRotation(touPos2.position,Quaternion.Euler(-90,0,180));
        touObj3.transform.SetPositionAndRotation(touPos3.position,Quaternion.Euler(-90,0,180));
        touObj4.transform.SetPositionAndRotation(touPos4.position,Quaternion.Euler(-90,0,180));
        touObj5.transform.SetPositionAndRotation(touPos5.position,Quaternion.Euler(-90,0,180));

    }
}
