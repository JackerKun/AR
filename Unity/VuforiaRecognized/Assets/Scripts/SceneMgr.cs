using UnityEngine;
using UnityEngine.UI;

public class SceneMgr : MonoBehaviour
{
	public Text text;
	CameraSettings camSet;
	public bool autoFocus = false;
	void Start()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		camSet = FindObjectOfType<CameraSettings>();
		Switch(autoFocus);
	}
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		if (Input.GetMouseButtonDown(0))
		{
			autoFocus = !autoFocus;
			Switch(autoFocus);
		}
	}

	void Switch(bool focus)
	{
		camSet.SwitchAutofocus(focus);
		text.text = focus ? "ON" : "OFF";
	}


}
