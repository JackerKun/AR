using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

/*
*Create By Keefor On 12/29/2017
*/

public class XJXiangModel
{
    /// <summary>
    /// 巡检项ID
    /// </summary>
    public string mid;

    /// <summary>
    /// 巡检项名称
    /// </summary>
    public string summary;

    /// <summary>
    /// 巡检项提示
    /// </summary>
    public string dialogContent;

    /// <summary>
    /// 巡检项当前情况
    /// </summary>
    public InspectionItem.SelectStatus selectStatus;

    public XJXiangModel(JSONNode node)
    {
        mid = node["checkContentID"].Value;
        summary = node["checkContent"].Value;
        dialogContent = node["checkDesc"].Value;
    }
}
