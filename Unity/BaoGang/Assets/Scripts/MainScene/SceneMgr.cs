using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Vuforia;

public class SceneMgr : MonoBehaviour
{
	Camera mainCamera;
	bool scanCamera = true;
	Vector3 canvasPivot;
	public Device DeviceName;
	Dictionary<Device, Vector3> targetOffset = new Dictionary<Device, Vector3>();
	public SCENE CurrentScene = SCENE.NONE;

	public enum SCENE
	{
		NONE,
		TANK,
		Inspection,
		EXPERT
	}

	;

	public enum Device
	{
		EpsonBT200,
		Other
	}

	void Start()
	{
		var vuforia = VuforiaARController.Instance;
		vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
		vuforia.RegisterVuforiaInitializedCallback(ChangeFOV);
		vuforia.RegisterOnPauseCallback(OnPaused);
		mainCamera = Camera.main;
		switch (CurrentScene)
		{
			case SCENE.EXPERT:
				{
					ExpertMgr.Init();
				}
				break;
		}
		//		InitTargetOffsetValue ();
		//		#if !UNITY_EDITOR
		//		AdjustDeviceOffset ();
		//		#endif 
		SceneManager.sceneUnloaded += (arg0) =>
		{
			VuforiaManager.Instance.Deinit();
			Debug.Log("sceneUnloaded");
		};
		InitWebClient();
	}

	//初始化网络监听
	void InitWebClient()
	{
		switch (CurrentScene)
		{
			case SCENE.TANK:
				{
				}
				break;
		}
	}

	void ChangeFOV()
	{
		Debug.Log("ChangeView");
		//		//单位矩阵
		//		Matrix4x4 matrix = Matrix4x4.identity; //单位矩阵
		//
		//		matrix.m03 = x; //x平移量可以在Inspector面板拖动
		//		matrix.m13 = 4; //y轴平移量
		//		matrix.m23 = 5; //z轴平移量
		//		VuforiaARController.Instance.ApplyCorrectedProjectionMatrix (matrix, true);
	}

	void InitTargetOffsetValue()
	{
		targetOffset.Add(Device.EpsonBT200, new Vector3(0.15f, 0.1f, -0.09f));
		targetOffset.Add(Device.Other, Vector3.zero);
	}

	void AdjustDeviceOffset()
	{
		TrackableBehaviour[] trackers = FindObjectsOfType<TrackableBehaviour>();

		foreach (TrackableBehaviour tracker in trackers)
		{
			tracker.transform.Find("Root").transform.localPosition = targetOffset[DeviceName];
		}
	}

	private void OnVuforiaStarted()
	{
		CameraDevice.Instance.SetFocusMode(
			CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}

	private void OnPaused(bool paused)
	{
		if (!paused)
		{ // resumed
		  // Set again autofocus mode when app is resumed
			CameraDevice.Instance.SetFocusMode(
				CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
		}
	}
}