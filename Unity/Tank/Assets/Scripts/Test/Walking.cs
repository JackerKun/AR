using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
	bool canMove = false;
	Vector3 lastTranPos;
	Vector3 mPos;
	float movingSpeed = .01f;
	public Texture targetTexture;
	// Update is called once per frame

	void Start ()
	{
		mPos = Input.mousePosition;
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			mPos = Input.mousePosition;
			lastTranPos = transform.position;
			canMove = true;
		} else if (Input.GetMouseButtonUp (0)) {
			canMove = false;
		}
		if (canMove) {
			Vector3 offsetV = Input.mousePosition - mPos;
//			transform.position = lastTranPos + new Vector3 (offsetV.x, 0, offsetV.y) * movingSpeed;
			transform.position = lastTranPos + offsetV * movingSpeed;
		}
	}

	void OnGUI ()
	{
		float targH = 100f;
		GUI.DrawTexture (new Rect ((Screen.width >> 2) - targH * .5f, (Screen.height >> 1) - targH * .5f, targH, targH * 2f), targetTexture);
		GUI.DrawTexture (new Rect ((Screen.width >> 2) * 3 - targH * .5f, (Screen.height >> 1) - targH * .5f, targH, targH * 2f), targetTexture);
		GUILayout.Label ((transform.position * 100f).ToString ());
	}
}
