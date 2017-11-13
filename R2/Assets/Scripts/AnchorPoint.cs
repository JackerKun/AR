using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
	//跟踪点物体位置的ui
	public Transform UITran;
	public Vector3 offset = new Vector3 ();
	public bool IsShowLine = false;
	Camera mainCamera;
	LineRenderer line;
	bool isTracking = false;

	void Start ()
	{
		mainCamera = Camera.main;
		line = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isTracking) {
			//更新屏幕坐标
			UpdatePose ();
		}
	}

	public void StartTracking ()
	{
		isTracking = true;
	}

	public void StopTracking ()
	{
		isTracking = false;
	}

	void UpdatePose ()
	{
		Debug.Log (transform.parent);
		UITran.position = mainCamera.WorldToScreenPoint (transform.position + offset);
		float angle = Vector3.Angle (transform.forward, Vector3.up);
		Vector3 pv = transform.forward - Vector3.Project (transform.forward, Vector3.up);

		UITran.up = Vector3.ProjectOnPlane (transform.forward, mainCamera.transform.forward);

//		UITran.eulerAngles = new Vector3 (0, 0, pv.x * -90f);

		Vector3 v = mainCamera.ScreenToWorldPoint (UITran.position);
		if (IsShowLine) {
			line.SetPosition (0, transform.position);
			line.SetPosition (1, v);
		}
	}
}
