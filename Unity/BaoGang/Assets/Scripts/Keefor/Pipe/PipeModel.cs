using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

/*
*Create By Keefor On 1/2/2018
*/

public class PipeDateModel  {
    /// <summary>
    /// 管道ID
    /// </summary>
    public string deviceID;
    /// <summary>
    /// 管道名称
    /// </summary>
    public string name;
    /// <summary>
    /// 管道流量
    /// </summary>
    public string liuliang;

    public PipeDateModel(JSONNode node)
    {
        deviceID = node["deviceID"];
        name = node["name"];
        liuliang = node["liuliang"];
    }

    public override string ToString()
    {
        return "id:" + deviceID + "  name:" + name + "  liuliang:" + liuliang;
    }
}

