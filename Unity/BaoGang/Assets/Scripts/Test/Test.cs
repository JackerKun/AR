using UnityEngine;

public class Test : MonoBehaviour
{
	public bool showBg;

	void Start()
	{
		showBg = FindObjectOfType<CustomARCamera>().ShowBg;
	}


	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			showBg = !showBg;
			GameObject.Find("ARCamera/BackgroundPlane").layer = showBg ? 31 : 10;
		}
	}
}