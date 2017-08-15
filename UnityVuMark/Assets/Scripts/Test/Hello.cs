using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Hello : MonoBehaviour
{

	Camera viewCamera;
	public Transform trackPoint;
	public Camera c;
	RectTransform _rect;

	// Use this for initialization
	void Start ()
	{
//		VuforiaARController.Instance.RegisterVuforiaStartedCallback (() => {
//			viewCamera = Camera.main;
//			Debug.LogError (transform + " >> " + viewCamera.name + " >> " + viewCamera.transform.parent);
//		});
		viewCamera = c;
		_rect = GetComponent<RectTransform> ();
	}

	void InitScene ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		UpdatePosition ();
	}

	void UpdatePosition ()
	{
		_rect.anchoredPosition = viewCamera.WorldToScreenPoint (trackPoint.position);
	}
}
