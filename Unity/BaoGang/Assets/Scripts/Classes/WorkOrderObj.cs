using System;
using HopeRun;
using System.Collections.Generic;
using UnityEngine;

public class WorkOrderObj
{
	public string jobNumber;
	public string deviceID;
	public NoName checkPoint;
	public List<NoName> checkContent = new List<NoName>();
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

	private WorkOrderObj() { }

	public WorkOrderObj(string jobNumber)
	{
		deviceID = GlobalManager.DeviceID;
		this.jobNumber = jobNumber;
		checkContent = new List<NoName>();
	}

	public void UpdateCheckPoint(string id, string status)
	{
		checkPoint = new NoName(id, status);
	}
	public void InsertCheckContent(string id, string status)
	{
		checkContent.Add(new NoName(id, status));
	}

	//提交订单
	public void CommitWorkOrder(List<InspectionItem> items)
	{
		Debug.LogError("提交订单");

	}

	[Serializable]
	public class NoName
	{
		public string id, cstatus;
		private NoName() { }
		public NoName(string id, string cstatus)
		{
			this.id = id;
			this.cstatus = cstatus;
		}
	}
}
