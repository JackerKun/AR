using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkOrderObj
{
	static WorkOrderObj instance;
	public static WorkOrderObj Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new WorkOrderObj();
			}
			return instance;
		}
	}
	public string jobNumber = "";

	//提交订单
	public void CommitWorkOrder(List<InspectionItem> items)
	{
		Debug.LogError("提交订单");

	}
}
