using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*Create By Keefor On 1/2/2018
*/

public class PipeUIMsg : MonoBehaviour
{

    //public PipePop[] popList;

    // Use this for initialization
    IEnumerator Start()
    {
        //popList = new PipePop[2];
        //for (int i = 1; i <= 2; i++)
        //{
        //    popList[i - 1] = GameObject.Find(i.ToString()).GetComponent<PipePop>();
        //}
        yield return new WaitForSeconds(0.5f);
        TDMouseInput.inst.SetVisiable(false);
        //WebManager.Instance.GetServer<PipeServices>().StartRequest(UpdataUI);
    }

    void OnDestroy()
    {
        //WebManager.Instance.GetServer<PipeServices>().StopRequest(UpdataUI);
    }

    void UpdataUI(List<PipeDateModel> obj)
    {
        //List<PipeDateModel> data = obj;
        //Debug.Log(data[1]);
        //for (int i = 0; i < data.Count; i++)
        //{
        //    if (popList.Length > i)
        //        popList[i].SetContext(data[i].name + data[i].liuliang);
        //}
    }
}
