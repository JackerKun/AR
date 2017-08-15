using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using HopeRun;

public class UIManager : MonoBehaviour
{
	Camera mainCamera;
	bool scanCamera = true;
	float initFov;
	float canvasScale = .0001f;
	Camera[] cameras;
	Vector3 canvasPivot;
	float curT;

	void Start ()
	{
		var vuforia = VuforiaARController.Instance;
		//vuforia.RegisterVuforiaStartedCallback (InitCanvas);
//		vuforia.RegisterVuforiaStartedCallback (GetInfos);
		mainCamera = Camera.main;
		initFov = mainCamera.fieldOfView;
	}

	/// <summary>
	/// 显示液位高度
	/// </summary>
	/// <param name="cur">当前液位高度</param>
	/// <param name="whole">总液位高度</param>
	public static void UpdateLiquidHeight (float cur)
	{
		if (GlobalManager.CURRENT_DRUM) {
			GlobalManager.CURRENT_DRUM.UpdateHeight (cur);
		}
	}

	//更改阀门状态
	public static void ChangeValveState (ValveState state)
	{
		if (GlobalManager.CURRENT_DRUM) {
			GlobalManager.CURRENT_DRUM.ChangeValveState (state);
		} else {
			Debug.Log ("CURRENT_DRUM IS NULL!");
		}
	}

	public static void ShowDialog (string str)
	{
		MessageDialog dialog = Instantiate<MessageDialog> (Resources.Load <MessageDialog> ("Prefabs/Dialog"), GameObject.Find ("Canvas").transform);
		dialog.InitText (str);
	}
}
