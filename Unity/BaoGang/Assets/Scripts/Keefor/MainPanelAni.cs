using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*Create By Keefor On 12/20/2017
*/

public class MainPanelAni : MonoBehaviour
{

    // Use this for initialization
    //    private Transform[] obj;
    private Transform dummy;
    private Transform target;

    void Start()
    {
        target = new GameObject("target").transform;
        target.SetParent(Camera.main.transform);
        target.localPosition = Vector3.forward * 400;

        dummy = this.transform;
        //        obj = new Transform[8];
        //        for (int i = 1; i <= 4; i++)
        //        {
        //            obj[i] = transform.Find("sl" + i);
        //        }
        //        for (int i = 1; i <= 4; i++)
        //        {
        //            obj[i + 4] = transform.Find("sr" + i);
        //        }
    }

    // Update is called once per frame
    void Update()
    {
        var pos = dummy.position - target.position;
        dummy.forward = pos.normalized; 
    }

}
