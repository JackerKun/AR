﻿using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using HopeRun;

public class TankObj : Container
{
	public CircleSlider LiquidLevel;
	public Toggle ValveToggle;
	//桶的ID
	public Text ID, _gaodu;
	string _drumId;
	ValveState curValveState;
	Vector3 targetScale;

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

	/// <summary>
	/// 显示液位高度
	/// </summary>
	/// <param name="cur">当前液位高度</param>
	/// <param name="whole">总液位高度</param>
	public void UpdateHeight(float curH, float totalAmount)
	{
		//curH /= 100;
		//LiquidLevel.value = curH;
		//LiquidValue.text = string.Format("{0:00.00}%", curH);
		//float r = Mathf.Clamp01((curH - 1f / 3f) * 3f);
		//float g = Mathf.Max(0, (1f - 3f * curH));
		//float b = 1f - Mathf.Max(0, (curH - 2f / 3f) * 3f);
		//LiquidImg.color = new Color(r, g, b);
		//CurTankColor = LiquidImg.color;
		//currentAmount = curH;
		_gaodu.text = curH.ToString(CultureInfo.InvariantCulture);
		LiquidLevel.Max = totalAmount;
		LiquidLevel.Value = curH;

		currentAmount = curH;
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

	public override void Deposit()
	{
		Debug.Log("Destroy:" + gameObject);
		Destroy(this.gameObject);
	}
}
