using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/*
*Create By Keefor On 12/25/2017
*/

/// <summary>
/// 陀螺仪输入
/// 控制附着物体旋转
/// 添加点头事件，校正目标物体位置，获取与目标物体夹角
/// </summary>
public partial class GyroInput : MonoBehaviour
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
                    CallNod();
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

    private void CallNod()
    {
        Debug.Log("diantou");
        if (NodHandle != null) NodHandle();
    }
    /// <summary>
    /// 校正位置，使传入物体显示在视口正前方，高度不变
    /// </summary>
    /// <param name="target"></param>
    public static void CorrecteUIPostion(Transform target)
    {
        target.RotateAround(inst.transform.position, Vector3.up, GetViewAngle(target));
    }
    /// <summary>
    /// 获取传入物体与视口水平夹角
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static float GetViewAngle(Transform target)
    {
        var cent = inst.transform.position;
        var forw = inst.transform.forward;
        var pos = (target.position - cent).normalized;
        pos.y = forw.y = 0;
        return Vector3.SignedAngle(pos, forw, Vector3.up);
    }

}
