using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*Create By Keefor On 12/25/2017
*/


public class GyroInput : MonoBehaviour
{
    private static GyroInput instance;
    public static GyroInput inst
    {
        get { return instance ?? (instance = Camera.main.gameObject.AddComponent<GyroInput>()); }
    }

    /// <summary>
    /// 竖直角度
    /// </summary>
    public float verticalAngle = 0;
    public Vector3 gyroAngle = Vector3.zero;
    public Vector3 forward = Vector3.zero;

    private event Action NodHandle;

    private float _nodtimer;
    private bool isnod;
    void Awake()
    {
        instance = this;
        Input.gyro.enabled = true;
    }

    void Update()
    {

        Quaternion gyroQuat = Input.gyro.attitude;
        Quaternion a = new Quaternion(0.5f, 0.5f, -0.5f, 0.5f);
        Quaternion b = new Quaternion(0f, 0f, 1f, 0f);
        gyroQuat = a * gyroQuat * b;

        transform.localRotation = gyroQuat;

        CheckNod();
    }

    private void CheckNod()
    {
        verticalAngle = Vector3.Angle(transform.up, Vector3.up);

        if (isnod)
        {
            //计时范围内抬头执行事件,超出时间则停止计时
            if (IsNodTimer())
            {
                if (verticalAngle < 20)
                {
                    //调用
                    if (NodHandle != null) NodHandle();
                    _nodtimer = Time.unscaledTime;
                    isnod = false;
                }
            }
            else
            {
                _nodtimer = Time.unscaledTime;
                isnod = false;
            }
        }
        else
        {
            if (verticalAngle < 20)
            {
                StartNodTimer();
            }
            else
            {//计时范围内到达低头值开始抬头计时
                if (IsNodTimer() && verticalAngle > 45)
                {
                    isnod = true;
                    StartNodTimer();
                }
            }
        }

    }

    public void StartNodTimer()
    {
        _nodtimer = 0.8f + Time.unscaledTime;
    }

    private bool IsNodTimer()
    {
        return _nodtimer > Time.unscaledTime;
    }
    /// <summary>
    /// 添加抬头事件
    /// </summary>
    /// <param name="func"></param>
    public void AddNodListener(Action func)
    {
        NodHandle += func;
    }
    /// <summary>
    /// 移除抬头事件
    /// </summary>
    /// <param name="func"></param>
    public void RemoveNodListener(Action func)
    {
        if (NodHandle != null)
            NodHandle -= func;
    }


    public static void CorrecteUIPostion(Transform ui)
    {
        Debug.Log("diantou");
        var cent = inst.transform.position;
        var forw = inst.transform.forward;
        var pos = (ui.position - cent).normalized;
        pos.y = forw.y = 0;
        var angle = Vector3.SignedAngle(pos, forw, Vector3.up);
        ui.RotateAround(cent, Vector3.up, angle);
    }
}
