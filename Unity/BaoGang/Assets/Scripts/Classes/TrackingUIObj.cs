
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;
using System.Collections.Generic;

public class TrackingUIObj : MonoBehaviour, ITrackingUI
{
	Text text;
	IChangableUI curUI;
	bool isUpdateValue = false;
	public string KeyID;
	public float size = .15f;
	public List<MeterObj> meters = new List<MeterObj>();
	public List<string> valves = new List<string>();

	// Use this for initialization
	void Start()
	{
		//text = transform.Find("Text").GetComponent<Text>();
		if (string.IsNullOrEmpty(KeyID))
		{
			Debug.LogError("KeyID can't be empty!");
		}
	}

	public void StartUpdateValue()
	{
		curUI = Instantiate(((GameObject)Resources.Load("Prefabs/InspectionTracker")).GetComponent<IChangableUI>(), transform);
		curUI.transform.localPosition = Vector3.zero;
		curUI.transform.localScale = Vector3.one * size;
		// 创建仪表
		curUI.CreateMeterUI(meters);
		// 创建阀门
		curUI.CreateValvesUI(valves);
		isUpdateValue = true;
	}

	public void StopUpdateValue()
	{
		if (curUI != null)
		{
			Destroy(curUI.gameObject);
		}
		isUpdateValue = false;
	}

	void Update()
	{
		if (isUpdateValue)
		{
			JSONNode node = InspectionMgr.Instance.GetNode(KeyID);
			for (int i = 0; i < meters.Count; i++)
			{
				meters[i].SetValues(node[meters[i].CurrentValueKey].AsFloat, node[meters[i].MaxValueKey].AsFloat);
			}
			List<bool> tmpIsOn = new List<bool>();
			foreach (string tmpValve in valves)
			{
				tmpIsOn.Add(node[tmpValve].AsInt != 0);
			}
			curUI.UpdateMetersUI(meters);
			curUI.UpdateValvesUI(valves, tmpIsOn);
		}

	}
}
