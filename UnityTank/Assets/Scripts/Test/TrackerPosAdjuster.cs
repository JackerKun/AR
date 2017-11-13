using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerPosAdjuster : MonoBehaviour
{
	public Transform markRoot;
	public bool ShowLog = true;
	float offsetStep = .01f;
	Rect posTxt, xp, xn, yp, yn, zp, zn, reset, save;
	Vector3 initPosition;

	void Start ()
	{
		float h = Screen.height / 7f;
		float btnH = h * .8f;
		float btnW = h;
		float xOffset = h * .2f;
		float yOffset = h * .1f;

		posTxt = new Rect (xOffset, h, Screen.width, Screen.height);

		xn = new Rect (xOffset, 2 * h + yOffset, btnW, btnH);
		xp = new Rect (xn.x + xn.width + xOffset, xn.y, xn.width, xn.height);

		yn = new Rect (xOffset, 3f * h + yOffset, btnW, btnH);
		yp = new Rect (yn.x + yn.width + xOffset, yn.y, yn.width, yn.height);

		zn = new Rect (xOffset, 4f * h + yOffset, btnW, btnH);
		zp = new Rect (zn.x + zn.width + xOffset, zn.y, zn.width, zn.height);

		reset = new Rect (xOffset, 5f * h, btnW, btnH);
		save = new Rect (reset.x + reset.width + xOffset, reset.y, reset.width, reset.height);

		initPosition = new Vector3 (PlayerPrefs.GetFloat ("OffsetX"), PlayerPrefs.GetFloat ("OffsetY"), PlayerPrefs.GetFloat ("OffsetZ"));
		markRoot.localPosition = initPosition;
	}

	string logInfo;

	string GetInfos ()
	{
		logInfo = "";
		Camera[] cameras = Transform.FindObjectsOfType<Camera> ();
		foreach (Camera c in cameras) {
			logInfo += c.name + c.transform.localPosition + "\n";
		}
		return logInfo;
	}

	void OnGUI ()
	{
		if (!ShowLog)
			return;
		GetInfos ();
		Vector3 trackerLocalPos = markRoot.localPosition;
		GUI.color = Color.yellow;
		GUI.Label (posTxt, "Tracker Object Offset( " + trackerLocalPos.x + ", " + trackerLocalPos.y + ", " + trackerLocalPos.z + " )"
		+ "\n Screen( " + Screen.width + " , " + Screen.height + " )\n" + logInfo);
		if (GUI.Button (xn, "左")) {
			markRoot.localPosition += Vector3.left * offsetStep;
		} else if (GUI.Button (xp, "右")) {
			markRoot.localPosition += Vector3.right * offsetStep;
		} else if (GUI.Button (yn, "上")) {
			markRoot.localPosition += Vector3.forward * offsetStep;
		} else if (GUI.Button (yp, "下")) {
			markRoot.localPosition += Vector3.back * offsetStep;
		} else if (GUI.Button (zn, "前")) {
			markRoot.localPosition += Vector3.down * offsetStep;
		} else if (GUI.Button (zp, "后")) {
			markRoot.localPosition += Vector3.up * offsetStep;
		} else if (GUI.Button (reset, "Reset")) {
			markRoot.localPosition = initPosition;
		} else if (GUI.Button (save, "Save")) {
			PlayerPrefs.SetFloat ("OffsetX", markRoot.localPosition.x);
			PlayerPrefs.SetFloat ("OffsetY", markRoot.localPosition.y);
			PlayerPrefs.SetFloat ("OffsetZ", markRoot.localPosition.z);
		}
	}
}