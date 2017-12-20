using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class InspectionMgr : MonoBehaviour
{
	List<InspectionItem> items = new List<InspectionItem>();
	string lastNode;
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

	// 添加监听事件
	void InitListener()
	{

	}

	// 更新巡检项按钮
	public void UpdateItems(JSONNode nodeRoot)
	{
		if (string.IsNullOrEmpty(nodeRoot.ToString()) || lastNode == nodeRoot.ToString())
		{
			return;
		}
		int count = nodeRoot.Count;
		// 重置巡检项
		uiMgr.ResetItems();
		foreach (JSONNode node in nodeRoot.Children)
		{
			// 添加新的巡检项
			uiMgr.InsertItem(node["checkContent"].Value, node["checkDesc"].Value);
		}
		uiMgr.UpdateItemsLayout();
		lastNode = nodeRoot.ToString();
	}
}
