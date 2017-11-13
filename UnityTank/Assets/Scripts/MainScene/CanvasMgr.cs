using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMgr : MonoBehaviour
{
	CanvasScaler canvasHandle;
	// Use this for initialization
	void Start ()
	{
		canvasHandle = GetComponent<CanvasScaler> ();
	}
	
	// Update is called once per frame
	void OnGUI ()
	{
		GUILayout.Label (canvasHandle.scaleFactor.ToString ());
		if (GUILayout.Button ("+")) {
			canvasHandle.scaleFactor += .01f;
		}
		if (GUILayout.Button ("-")) {
			canvasHandle.scaleFactor -= .01f;
		}
	}
}
