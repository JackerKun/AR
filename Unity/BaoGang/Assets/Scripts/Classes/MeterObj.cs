using UnityEngine;
using System;

[Serializable]
public class MeterObj
{
	public string CurrentValueKey, MaxValueKey;
	[HideInInspector]
	public float curValue, maxValue;
	CircleSlider curSliderObj;

	public void SetSliderObj(CircleSlider sliderObj)
	{
		curSliderObj = sliderObj;
	}
	// 设置仪表最大和当前数值
	public void SetValues(float curV, float maxV)
	{
		curSliderObj.Value = curV;
		curSliderObj.Max = maxV;
		this.curValue = curV;
		this.maxValue = maxV;
	}
}
