using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFOV : MonoBehaviour
{
	public Text number;
	Camera curCame;
	RaycastHit hit;
	public Texture mousePoint;
	Rect mouseRect;
	int mouseWH = 15;

	void Start ()
	{
		curCame = Camera.main;
		mouseRect = new Rect ((Screen.width - mouseWH) >> 1, (Screen.height - mouseWH) >> 1, mouseWH, mouseWH);
	}

	void Update ()
	{
		if (Input.GetAxis ("Vertical") != 0) {
			curCame.fieldOfView += Input.GetAxis ("Vertical") * 10;
			number.text = "FOV = " + curCame.fieldOfView;
		}
	}

	void OnGUI ()
	{
		GUI.color = Color.red;
		GUI.DrawTexture (mouseRect, mousePoint);
	}

}
