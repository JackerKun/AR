using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using HopeRun;
using System;
using System.Collections.Generic;

public class LiquidUI : IChangableUI
{
	public Toggle valvePrefab;
	public CircleSlider MeterPrefab;
	//桶的ID
	public Text ID;
	string _drumId;
	Vector3 targetScale;
	List<Toggle> valves = new List<Toggle>();
	List<CircleSlider> meterSliders = new List<CircleSlider>();
	Transform valveRoot;
	Transform meterRoot;

	void Awake()
	{
		valveRoot = transform.Find("ValvesRoot");
		meterRoot = transform.Find("MetersRoot");
	}

	public void InitParam(string drumID)
	{
		this._drumId = drumID;
		ID.text = "ID: " + drumID;
	}

	public void InitUI()
	{
		targetScale = transform.localScale;
		transform.localScale *= .8f;
		transform.DOScaleX(targetScale.x, .3f).SetEase(Ease.OutBounce).SetDelay(.05f);
		transform.DOScaleY(targetScale.y, .4f).SetEase(Ease.OutBounce);
	}

	public override void CreateValvesUI(List<string> valveNames)
	{
		foreach (var v in valves)
		{
			Destroy(v.gameObject);
		}
		valves = new List<Toggle>();
		for (int i = 0; i < valveNames.Count; i++)
		{
			Toggle tmpTg = Instantiate(valvePrefab, valveRoot);
			tmpTg.name = valveNames[i];
			valves.Add(tmpTg);
		}
	}

	public override void CreateMeterUI(List<MeterObj> meterObjs)
	{
		foreach (var v in meterSliders)
		{
			Destroy(v.gameObject);
		}
		meterSliders = new List<CircleSlider>();
		for (int i = 0; i < meterObjs.Count; i++)
		{
			CircleSlider tmpSli = Instantiate(MeterPrefab, meterRoot);
			tmpSli.name = meterObjs[i].CurrentValueKey;
			meterObjs[i].SetSliderObj(tmpSli);
			meterSliders.Add(tmpSli);
		}
	}

	public void Deposit()
	{
		Debug.Log("Destroy:" + gameObject);
		Destroy(this.gameObject);
	}

	public override void UpdateValvesUI(List<string> valveNames, List<bool> isOn)
	{
		for (int i = 0; i < valves.Count; i++)
		{
			valves[i].isOn = isOn[i];
			Debug.Log(valves[i] + " >> " + isOn[i]);
		}
	}

	public override void UpdateMetersUI(List<MeterObj> meters)
	{
		for (int i = 0; i < meters.Count; i++)
		{
			meters[i].SetValues(meters[i].curValue, meters[i].maxValue);
		}
	}
}
