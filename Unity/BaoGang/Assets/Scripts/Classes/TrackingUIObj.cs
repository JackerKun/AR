using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class TrackingUIObj : MonoBehaviour, ITrackingUI
{
	Text text;
	bool isUpdateValue = false;
	public string KeyID;

	// Use this for initialization
	void Start()
	{
		text = transform.Find("Text").GetComponent<Text>();
		if (string.IsNullOrEmpty(KeyID))
		{
			Debug.LogError("KeyID can't be empty!");
		}
	}

	public void StartUpdateValue()
	{
		isUpdateValue = true;
	}

	public void StopUpdateValue()
	{
		isUpdateValue = false;
	}

	void Update()
	{
		if (isUpdateValue)
		{
			JSONNode node = InspectionMgr.Instance.GetNode(KeyID);
			Debug.LogError(node.ToString());
			foreach (var v in node.Children)
			{
				Debug.LogError(v.ToString());
			}
		}

	}
}
