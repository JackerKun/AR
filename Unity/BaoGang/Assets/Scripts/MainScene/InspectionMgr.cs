using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class InspectionMgr : MonoBehaviour
{
	public static List<InspectionItem> items = new List<InspectionItem>();
	public static float LastMsgT = 0;
	// 当前工单号
	public static string curWorkOrderNumber;
	public static string orderId;
	// 巡检项数据信息
	JSONNode itemsDataNode;
	static InspectionMgr instance;
	public static InspectionMgr Instance
	{
		get
		{
			return instance;
		}
	}
	InspectionUIMgr uiMgr;
	// Use this for initialization
	public void Awake()
	{
		instance = this;
		uiMgr = GetComponent<InspectionUIMgr>();
	}

	public JSONNode GetNode(string key)
	{
		Debug.Log(itemsDataNode);
		if (itemsDataNode.IsNull)
			return null;
		return itemsDataNode[key];
	}

	// 添加监听事件
	void InitListener()
	{

	}

	// 更新工单表
	public void UpdateWorkOrder(JSONNode nodeRoot)
	{
		if (nodeRoot.IsNull || curWorkOrderNumber == nodeRoot["jobNumber"])
		{
			return;
		}
		curWorkOrderNumber = nodeRoot["jobNumber"];
		orderId = nodeRoot["checkPoint"]["checkPointID"];
		// 重置巡检项
		ResetItems();
		foreach (JSONNode node in nodeRoot["checkContent"].Children)
		{
			// 添加新的巡检项
			uiMgr.InsertItem(node["checkContentID"].Value, node["checkContent"].Value, node["checkDesc"].Value);
		}
		uiMgr.UpdateItemsLayout();
	}

	// 清除巡检项
	void ResetItems()
	{
		for (int i = 0; i < items.Count; i++)
		{
			Destroy(items[i].gameObject);
		}
		items = new List<InspectionItem>();
	}

	// 更新数据
	internal void UpdateItemsData(JSONNode jSONNode)
	{
		if (jSONNode.IsNull || jSONNode.Equals(itemsDataNode))
		{
			return;
		}
		itemsDataNode = jSONNode;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Submit();
		}
		CheckDataCommunicate();
	}

	// 检测数据通信
	void CheckDataCommunicate()
	{
		if (Time.time - LastMsgT < 10f)
		{
			Debug.Log("at check point");
		}
		else
		{
			Debug.Log("not in position");
		}
	}

	// 更新巡检项
	public void Submit()
	{
		WorkOrderObj order = new WorkOrderObj(curWorkOrderNumber);
		// 工单是否正常
		InspectionItem.SelectStatus curOrderStatus = InspectionItem.SelectStatus.Null;
		// 一个没填选择null, 有一个错选择no
		bool isOrderLoop = true;
		foreach (InspectionItem item in items)
		{
			if (isOrderLoop)
			{
				if (item.curSltStatus != InspectionItem.SelectStatus.Null)
				{
					if (item.curSltStatus == InspectionItem.SelectStatus.No)
					{
						curOrderStatus = item.curSltStatus;
						isOrderLoop = false;
					}
					curOrderStatus = item.curSltStatus;
				}
			}
			order.InsertCheckContent(item.id, ((int)item.curSltStatus).ToString());
		}
		order.UpdateCheckPoint(orderId, ((int)curOrderStatus).ToString());
		JSONNode _orderNode = JSONNode.Parse(JsonUtility.ToJson(order));
		InspectionSocketService.Instance.SubmitWorkOrder(_orderNode);
		HideAllUI();
		InspectionUIMgr.Instance.SetMouse2D(true);
		Debug.Log("提交工单");
	}

	void HideAllUI()
	{
		if (InspectionUIMgr.curUIMode == InspectionUIMgr.UIMode.ItemList)
		{
			uiMgr.HideItems();
		}
		else if (InspectionUIMgr.curUIMode == InspectionUIMgr.UIMode.DialogBox)
		{
			uiMgr.HideOptionDialog();
			uiMgr.HideItems();
		}
		InspectionInteractive.CanChangeUI = true;
	}
}
