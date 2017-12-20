using System.Collections;
using System.Collections.Generic;
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
				MainSceneMgr.MainMgr.LoadScene("Tank");
			}
			else
			{
				MainSceneMgr.MainMgr.LoadScene("WorkFlow");
			}
		}
	}

	public static void DealInspectionMsg(JSONNode jn)
	{
		if (string.IsNullOrEmpty(jn.ToString()) || jn["data"].IsNull)
		{
			return;
		}
		InspectionMgr.Instance.UpdateItems(jn["data"]["checkContent"]);
		foreach (JSONNode node in jn["data"]["checkContent"].Children)
		{
			Debug.LogError(node.Value);
		}
	}
}
