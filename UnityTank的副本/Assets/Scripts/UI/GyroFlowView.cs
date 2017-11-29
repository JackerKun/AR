using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroFlowView : MonoBehaviour
{
	static ScrollRect scrollView;
	static GameObject viewGO;
	Gyroscope myGyro;
	float curHorizontalV = .5f;
	// Use this for initialization
	void Awake ()
	{
		myGyro = Input.gyro;
		myGyro.enabled = true;
		scrollView = GameObject.Find ("Canvas/Scroll View").GetComponent<ScrollRect> ();
		viewGO = scrollView.gameObject;
		scrollView.content.sizeDelta = new Vector2 (Screen.width, 0);
	}

	void Update ()
	{
		curHorizontalV = Mathf.Clamp01 (curHorizontalV - myGyro.rotationRate.y * .01f);
		scrollView.horizontalNormalizedPosition = curHorizontalV;
	}

	public static void SetFlowPanelActive (bool active)
	{
		if (viewGO) {
			viewGO.SetActive (active);
		}
	}
}
