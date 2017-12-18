using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using HopeRun;

public class UIManager : MonoBehaviour
{
	public static Text MessageTxt;
	Camera mainCamera;
	bool scanCamera = true;
	float initFov;
	float canvasScale = .0001f;
	Camera[] cameras;
	Vector3 canvasPivot;
	float curT;

	public static MessageDialog dialog;

	void Start()
	{
		MessageTxt = GameObject.Find("MainCanvas/StayMessage").GetComponent<UnityEngine.UI.Text>();
		mainCamera = Camera.main;
		//		initFov = mainCamera.fieldOfView;
	}

	/// <summary>
	/// 显示液位高度
	/// </summary>
	/// <param name="cur">当前液位高度</param>
	/// <param name="whole">总液位高度</param>
	public static void UpdateLiquidHeight(float cur, float limitValue)
	{
		if (GlobalManager.CURRENT_TANK)
		{
			GlobalManager.CURRENT_TANK.UpdateHeight(cur, limitValue);
		}
	}

	public static void ShowStayMessage(string strMsg)
	{
		MessageTxt.text = strMsg;
	}

	//更改阀门状态
	public static void ChangeValveState(ValveState state)
	{
		if (GlobalManager.CURRENT_TANK)
		{
			GlobalManager.CURRENT_TANK.ChangeValveState(state);
		}
		else {
			Debug.Log("CURRENT_TANK IS NULL!");
		}
	}

	public static void ShowErrorMessage(string str)
	{
		if (dialog == null)
		{
			dialog = Instantiate<MessageDialog>(Resources.Load<MessageDialog>("Prefabs/ErrorMessage"), GameObject.Find("MainCanvas").transform);
			dialog.InitText(str);
		}
	}

	public static void ShowMessage(string str)
	{
		if (dialog == null)
		{
			dialog = Instantiate<MessageDialog>(Resources.Load<MessageDialog>("Prefabs/Message"), GameObject.Find("MainCanvas").transform);
			dialog.InitText(str);
		}
	}
}
