using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AR.Model;

public class WebTest : MonoBehaviour
{
	TankSceneService webBucket;

	void OnGUI ()
	{
		if (GUILayout.Button ("Start Scan")) {
			if (webBucket == null) {
				webBucket = new TankSceneService ();
			}
			webBucket.onScaning ("0000", scanCallback);
		} else if (GUILayout.Button ("Lost Scan")) {
			webBucket.onLostScaning ("0000");
		}
	}

	void scanCallback (Tank data)
	{
		Debug.Log ("scanCallback:" + data);
	}
}
