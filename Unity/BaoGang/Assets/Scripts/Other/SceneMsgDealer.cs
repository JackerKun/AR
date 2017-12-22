using System.Collections;
using System.Collections.Generic;
using HopeRun;
using UnityEngine;
using SimpleJSON;
using HopeRun.Message;

public class SceneMsgDealer
{
	public static void DealTankMsg(JSONNode jn, bool isOnline = false)
	{
		string index;
		string tankARName = "";
		if (isOnline)
		{
			index = jn["data"]["job"]["status"];
		}
		else
		{
			index = jn["data"]["status"];
		}
		if (!string.IsNullOrEmpty(index))
		{
			string curStep = MessageLibrary.GetMessage(index);
			if (curStep == "10")
			{
				if (isOnline)
				{
					tankARName = jn["data"]["job"]["prodTitle"];
				}
				else
				{
					tankARName = jn["data"]["prodTitle"];
				}
				curStep = MessageLibrary.GetMessage("ARTank_" + tankARName);
			}
			if (index == "11" || index == "13")
			{
				UIManager.ShowStayMessage("");
			}
			else
			{
				UIManager.ShowStayMessage(curStep);
			}
			if (index == "10")
			{
				GlobalManager.LoadScene("Tank");
			}
			else
			{
                GlobalManager.LoadScene("WorkFlow");
			}
		}
	}

	public static void DealInspectionMsg(JSONNode jn)
	{
		if (string.IsNullOrEmpty(jn.ToString()) || jn["data"].IsNull)
		{
			return;
		}
		Debug.LogError(jn["data"]);
		// 创建工单
		InspectionMgr.Instance.UpdateWorkOrder(jn["data"]);
		// 保存巡检项数据
		InspectionMgr.Instance.UpdateItemsData(jn["data"]["checkResultData"]);
	}
}
