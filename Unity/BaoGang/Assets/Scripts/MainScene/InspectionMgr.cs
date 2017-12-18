using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class InspectionMgr : MonoBehaviour
{
	List<InspectionItem> items = new List<InspectionItem>();
	InspectionUIMgr uiMgr;
	// Use this for initialization
	public void Start()
	{
		uiMgr = GetComponent<InspectionUIMgr>();
	}

	// 添加监听事件
	void InitListener()
	{

	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.X))
		{
			UpdateItems(null, 12);
		}
		else if (Input.GetKeyDown(KeyCode.Z))
		{
			UpdateItems(null, 36);
		}
	}

	// 更新巡检项按钮
	void UpdateItems(JSONNode node, int dd)
	{
		// 解析Node
		// 重置巡检项
		uiMgr.ResetItems();

		for (int i = 0; i < dd; i++)
		{
			// 添加新的巡检项
			uiMgr.InsertItem("巡检项" + i, "该巡检项是否正常？");
		}
		uiMgr.UpdateItemsLayout();
	}
}
