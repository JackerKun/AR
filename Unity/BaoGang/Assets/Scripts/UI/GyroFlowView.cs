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
	float curHorizontalH = .5f;
	// Use this for initialization
	void Awake ()
	{
		myGyro = Input.gyro;
		myGyro.enabled = true;
		scrollView = GameObject.Find ("Canvas/Scroll View").GetComponent<ScrollRect> ();
		viewGO = scrollView.gameObject;
		scrollView.content.sizeDelta = new Vector2 (Screen.width, Screen.height);
		SetFlowPanelActive (false);
	}

	void Update ()
	{
		curHorizontalV = Mathf.Clamp01 (curHorizontalV - myGyro.rotationRate.y * .05f);
		curHorizontalH = Mathf.Clamp01 (curHorizontalH + myGyro.rotationRate.x * .05f);
		scrollView.horizontalNormalizedPosition = curHorizontalV;
		scrollView.verticalNormalizedPosition = curHorizontalH;
	}

	public static void SetFlowPanelActive (bool active)
	{
		if (viewGO) {
			viewGO.SetActive (active);
		}
	}

	public static Text text {
		get { 
			return scrollView.transform.Find ("Viewport/Content/Text").GetComponent<Text> ();
		}
	}

	public static RawImage colorImg {
		get { 
			return scrollView.transform.Find ("Viewport/Content/RawImage").GetComponent<RawImage> ();
		}
	}
}
