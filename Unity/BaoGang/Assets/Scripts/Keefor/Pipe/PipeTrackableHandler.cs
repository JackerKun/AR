using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*Create By Keefor On 1/2/2018
*/

public class PipeTrackableHandler : BaseTrackableEventHandler
{


    public PipePop[] popList;
    private Dictionary<int, PipePop> dicpop;
    protected override void Init()
    {
        base.Init();
        popList = GetComponentsInChildren<PipePop>();
        dicpop = new Dictionary<int, PipePop>();
        for (int i = 0; i < popList.Length; i++)
        {
            dicpop.Add(int.Parse(popList[i].name),popList[i]);
        }
    }

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        EnableCollider();
        EnableRender();
        WebManager.Instance.GetServer<PipeServices>().StartRequest(UpdataUI);
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        DisableCollider();
        DisableRender();
        WebManager.Instance.GetServer<PipeServices>().StopRequest(UpdataUI);
    }

    void UpdataUI(List<PipeDateModel> obj)
    {
        List<PipeDateModel> data = obj;
        Debug.Log(trackName+data[1]);
        foreach (var pipePop in dicpop)
        {
            var index = pipePop.Key - 1;
            pipePop.Value.SetContext(data[index].name+"\n"+data[index].liuliang);
        }
    }
}
