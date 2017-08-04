using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SceneMgr : MonoBehaviour
{
	Camera mainCamera;
	bool scanCamera = true;
	float initFov;
	Camera[] cameras;
	Vector3 canvasPivot;
	public Device DeviceName;
	Dictionary<Device, Vector3> targetOffset = new Dictionary<Device, Vector3> ();

	public enum Device
	{
		EpsonBT200,
		Other
	}

	void Start ()
	{
		var vuforia = VuforiaARController.Instance;
		vuforia.RegisterVuforiaStartedCallback (OnVuforiaStarted);
		vuforia.RegisterVuforiaInitializedCallback (ChangeFOV);
		vuforia.RegisterOnPauseCallback (OnPaused);
		mainCamera = Camera.main;
		initFov = mainCamera.fieldOfView;


		InitTargetOffsetValue ();
		#if !UNITY_EDITOR
		AdjustDeviceOffset ();
		#endif 
	}

	public float x;

	void ChangeFOV ()
	{
		Debug.Log ("ChageView");
//		//单位矩阵
//		Matrix4x4 matrix = Matrix4x4.identity; //单位矩阵
//
//		matrix.m03 = x; //x平移量可以在Inspector面板拖动
//		matrix.m13 = 4; //y轴平移量
//		matrix.m23 = 5; //z轴平移量
//		VuforiaARController.Instance.ApplyCorrectedProjectionMatrix (matrix, true);
	}

	void InitTargetOffsetValue ()
	{
		targetOffset.Add (Device.EpsonBT200, new Vector3 (0.15f, 0.1f, -0.09f));
		targetOffset.Add (Device.Other, Vector3.zero);
	}

	void AdjustDeviceOffset ()
	{
		TrackableBehaviour[] trackers = Transform.FindObjectsOfType<TrackableBehaviour> ();

		foreach (TrackableBehaviour tracker in trackers) {
			tracker.transform.Find ("Root").transform.localPosition = targetOffset [DeviceName];
		}
	}

	private void OnVuforiaStarted ()
	{
		CameraDevice.Instance.SetFocusMode (
			CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
		cameras = GameObject.Find ("ARCamera").GetComponentsInChildren<Camera> ();
	}

	private void OnPaused (bool paused)
	{
		if (!paused) { // resumed
			// Set again autofocus mode when app is resumed
			CameraDevice.Instance.SetFocusMode (
				CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
		}
	}
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}
}