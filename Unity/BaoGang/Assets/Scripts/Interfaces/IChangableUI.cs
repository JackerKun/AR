using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IChangableUI : MonoBehaviour
{
	public abstract void UpdatePercentLeft(float curH, float totalAmount);
	public abstract void UpdatePercentRight(float curH, float totalAmount);
	public abstract void CreateValvesUI(List<string> valveNames);
	public abstract void UpdateValvesUI(List<string> valveNames, List<bool> isOn);
}



