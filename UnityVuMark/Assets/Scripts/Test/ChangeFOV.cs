using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ChangeFOV : MonoBehaviour
{
	public Text fov;
	Camera curCame;
	RaycastHit hit;
	public Texture mousePoint;
	Rect mouseRect;
	int mouseWH = 15;
	VuforiaBehaviour vb;
	VideoBackgroundBehaviour vbgb;

	void Start ()
	{
		curCame = Camera.main;
		mouseRect = new Rect ((Screen.width - mouseWH) >> 1, (Screen.height - mouseWH) >> 1, mouseWH, mouseWH);
		vb = GetComponent<VuforiaBehaviour> ();
		vb.StartEvent += (() => {
			Debug.Log ("StartEvent -- " + GameObject.FindObjectsOfType<Camera> ().Length);
		});
//		DigitalEyewearARController.SerializableViewerParameters d = GetComponent<VuforiaBehaviour>().;
//		d.FieldOfView = new Vector4 ();
	}

	void Update ()
	{
		if (Input.GetAxis ("Vertical") != 0) {
			curCame.fieldOfView += Input.GetAxis ("Vertical") * 10;
			fov.text = "FOV = " + curCame.fieldOfView;
		}
	}

	void OnGUI ()
	{
		GUI.color = Color.red;
		GUI.DrawTexture (mouseRect, mousePoint);
	}

}
