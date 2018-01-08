using System;
using System.Collections;
using System.Collections.Generic;
using HopeRun;
using HopeRun.Model;
using SimpleJSON;
using UnityEngine;

/*
*Create By Keefor On 1/2/2018
*/

public class PipeServices : IRegistServer
{
    private Action<List<PipeDateModel>> callAction;

    public void AddSelfEvent()
    {
        Debug.Log("注册管道服务");
        WebManager.Instance.Connect("pipe", node => { Debug.Log("已连接服务器"); });
        WebManager.Instance.On(PipeConstKey.ResponsePipe, DealFunc);
    }

    public void StartRequest(Action<List<PipeDateModel>> callback)
    {

        callAction += callback;
        //WebManager.Instance.Emit(PipeConstKey.StartReceive, JsonUtility.ToJson(new TargetDeviceRequest(false, GlobalManager.DeviceID, "0003")));
        WebManager.Instance.CancleRequestData(PipeConstKey.StartReceive);
    }

    public void StopRequest(Action<List<PipeDateModel>> callback)
    {
        if (callAction != null)
            callAction -= callback;
        WebManager.Instance.CancleRequestData(PipeConstKey.StopReceive);
    }

    void DealFunc(JSONNode[] node)
    {
        List<PipeDateModel> pipedata = new List<PipeDateModel>();
        for (int i = 1; i < node[0].Count; i++)
        {
            pipedata.Add(new PipeDateModel(node[0][i]));
        }
        if (callAction != null)
            callAction(pipedata);
    }
}
