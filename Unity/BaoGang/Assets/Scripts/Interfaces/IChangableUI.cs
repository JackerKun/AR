using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IChangableUI : MonoBehaviour
{
	public abstract void CreateValvesUI(List<string> valveNames);
	public abstract void UpdateValvesUI(List<string> valveNames, List<bool> isOn);

	public abstract void CreateMeterUI(List<MeterObj> meterObjs);

	public abstract void UpdateMetersUI(List<MeterObj> meters);
}



