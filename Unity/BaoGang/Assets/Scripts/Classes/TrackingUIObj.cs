
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections.Generic;

public class TrackingUIObj : MonoBehaviour, ITrackingUI
{
	Text text;
	IChangableUI curUI;
	bool isUpdateValue = false;
	public string KeyID;
	public float size = .15f;
	public string NodeValueKey_L;
	public string NodeMaxValue_L;
	public string NodeValueKey_R;
	public string NodeMaxValue_R;
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
			Debug.LogError(KeyID + " >> " + node.ToString());
			string tmpStr = "";
			foreach (JSONNode v in node.Children)
			{
				Debug.Log(v.ToString());
				curUI.UpdatePercentLeft(node[NodeValueKey_L].AsFloat, node[NodeMaxValue_L].AsFloat);
				curUI.UpdatePercentRight(node[NodeValueKey_R].AsFloat, node[NodeMaxValue_R].AsFloat);
				List<bool> tmpIsOn = new List<bool>();
				foreach (string tmpValve in valves)
				{
					tmpIsOn.Add(node[tmpValve].AsBool);
				}
				curUI.UpdateValvesUI(valves, tmpIsOn);
				tmpStr += "\n" + v;
			}
			//text.text = tmpStr;
		}

	}
}
