using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using HopeRun;

public class TankObj : Container
{
	public Slider LiquidLevel;
	public Text LiquidValue;
	public Image LiquidImg;
	public Toggle ValveToggle;
	//桶的ID
	public Text ID;
	string drumID;
	ValveState curValveState;
	Vector3 targetScale;

	public void InitParam (string drumID, float curH, float fullH)
	{
		this.drumID = drumID;
		ID.text = "ID: " + drumID;
		currentAmount = curH;
		totalAmount = fullH;
		curValveState = ValveToggle.isOn ? ValveState.ON : ValveState.OFF;
	}

	public void InitUI ()
	{
		targetScale = transform.localScale;
		transform.localScale *= .8f;
		transform.DOScaleX (targetScale.x, .3f).SetEase (Ease.OutBounce).SetDelay (.05f);
		transform.DOScaleY (targetScale.y, .4f).SetEase (Ease.OutBounce);
	}

	/// <summary>
	/// 显示液位高度
	/// </summary>
	/// <param name="cur">当前液位高度</param>
	/// <param name="whole">总液位高度</param>
	public void UpdateHeight (float curH)
	{
		LiquidLevel.value = curH / 100f;
		LiquidValue.text = string.Format ("{0:00.00}%", curH);
		float r = Mathf.Clamp01 ((curH - 1f / 3f) * 3f);
		float g = Mathf.Max (0, (1f - 3f * curH));
		float b = 1f - Mathf.Max (0, (curH - 2f / 3f) * 3f);
		LiquidImg.color = new Color (r, g, b);
		currentAmount = curH;

//		float curV = curH / totalAmount;
//		LiquidLevel.value = curV;
//		LiquidValue.text = string.Format ("{0:00.00升}", curH);
//		float r = Mathf.Clamp01 ((curV - 1f / 3f) * 3f);
//		float g = Mathf.Max (0, (1f - 3f * curV));
//		float b = 1f - Mathf.Max (0, (curV - 2f / 3f) * 3f);
//		LiquidImg.color = new Color (r, g, b);
//		currentAmount = curH;
	}

	public void ChangeValveState (ValveState state)
	{
		switch (state) {
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

	public override void Deposit ()
	{
		Debug.Log ("Destroy:" + gameObject);
		Destroy (this.gameObject);
	}
}
