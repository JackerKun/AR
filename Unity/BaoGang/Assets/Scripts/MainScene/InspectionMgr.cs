using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class InspectionMgr : MonoBehaviour
{
	public static List<InspectionItem> items = new List<InspectionItem>();
	// 当前工单号
	public static string curWorkOrderNumber;
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
		Debug.LogError(itemsDataNode);
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
		// 重置巡检项
		ResetItems();
		foreach (JSONNode node in nodeRoot["checkContent"].Children)
		{
			// 添加新的巡检项
			uiMgr.InsertItem(node["deviceID"].Value, node["checkContent"].Value, node["checkDesc"].Value);
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
	}

	// 更新巡检项
	public void Submit()
	{
		// 更新巡检项状态
		WorkOrderObj.Instance.CommitWorkOrder(items);
		Debug.LogError("提交工单");
	}
}
