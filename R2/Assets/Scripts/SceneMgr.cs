using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SceneMgr : MonoBehaviour
{
	Camera mainCamera;
	// Use this for initialization
	void Start ()
	{
		//https://library.vuforia.com/content/vuforia-library/en/articles/Solution/Unity-Configuration-Upgrade-Script-for-Vuforia-6-2-plus.html
//		new VuforiaConfiguration()
		mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
//		mainCamera.projectionMatrix = Matrix4x4.identity;
//		Debug.Log (mainCamera.projectionMatrix);
//		Matrix4x4 m = new Matrix4x4 ();
//		m.SetTRS (Vector3.zero, Quaternion.identity, new Vector3 (5000f, 5000f, 5000f));
	}
}
