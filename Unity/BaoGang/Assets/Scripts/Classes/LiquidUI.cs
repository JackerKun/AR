using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using HopeRun;
using System;
using System.Collections.Generic;

public class LiquidUI : IChangableUI
{
	public CircleSlider circleLeft;
	public CircleSlider circleRight;
	public Toggle ValveToggle;
	public Toggle valvePrefab;
	//桶的ID
	public Text ID;
	string _drumId;
	ValveState curValveState;
	Vector3 targetScale;
	List<Toggle> valves = new List<Toggle>();
	Transform valveRoot;

	void Awake()
	{
		valveRoot = transform.Find("ValvesRoot");
	}

	public void InitParam(string drumID)
	{
		this._drumId = drumID;
		ID.text = "ID: " + drumID;
		curValveState = ValveToggle.isOn ? ValveState.ON : ValveState.OFF;
	}

	public void InitUI()
	{
		targetScale = transform.localScale;
		transform.localScale *= .8f;
		transform.DOScaleX(targetScale.x, .3f).SetEase(Ease.OutBounce).SetDelay(.05f);
		transform.DOScaleY(targetScale.y, .4f).SetEase(Ease.OutBounce);
	}

	public override void CreateValvesUI(List<string> valvesName)
	{
		foreach (var v in valves)
		{
			Destroy(v.gameObject);
		}
		valves = new List<Toggle>();
		for (int i = 0; i < valvesName.Count; i++)
		{
			Toggle tmpTg = Instantiate(valvePrefab, valveRoot);
			tmpTg.name = valvesName[i];
			valves.Add(tmpTg);
		}
	}

	/// <summary>
	/// 显示液位高度
	public override void UpdatePercentLeft(float curH, float totalAmount)
	{
		circleLeft.Max = totalAmount;
		circleLeft.Value = curH;
	}

	public override void UpdatePercentRight(float curH, float totalAmount)
	{
		circleRight.Max = totalAmount;
		circleRight.Value = curH;
	}

	public void ChangeValveState(ValveState state)
	{
		switch (state)
		{
			case ValveState.ON:
				{
					ValveToggle.isOn = true;
				}
				break;
			case ValveState.OFF:
				{
					ValveToggle.isOn = false;
				}
				break;
		}
	}

	public void Deposit()
	{
		Debug.Log("Destroy:" + gameObject);
		Destroy(this.gameObject);
	}

	public override void UpdateValvesUI(List<string> valveNames, List<bool> isOn)
	{
		Debug.LogError("Hello World!!!");
	}
}
