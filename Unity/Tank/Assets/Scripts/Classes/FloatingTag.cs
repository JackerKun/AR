using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HopeRun;

public class FloatingTag : MonoBehaviour
{
	public Text Name;
	public Text Status;
	Transform trackPoint;
	RectTransform tagRect;
	Camera viewCamera;
	LineRenderer lr;

	Image bg;

	public void Init (Transform trackPoint)
	{
		bg = GetComponent<Image> ();
		transform.localScale = Vector3.one;
		viewCamera = Camera.main;
		this.trackPoint = trackPoint;
		tagRect = GetComponent<RectTransform> ();
		lr = gameObject.GetComponent<LineRenderer> ();
		transform.position = trackPoint.position;
	}

	void Update ()
	{
		if (!trackPoint || !viewCamera)
			return;
		UpdatePosition ();
	}

	void UpdatePosition ()
	{
//		tagRect.anchoredPosition = viewCamera.WorldToScreenPoint (trackPoint.position);
		transform.position = Vector3.Lerp (transform.position, trackPoint.position + new Vector3 (0, .6f, .1f), Time.deltaTime * 10f);
//		tagRect.position = trackPoint.position + new Vector3 (0, 2f, 5f);
		lr.SetPositions (new Vector3[]{ transform.position, trackPoint.position });
	}
	
	// Update is called once per frame
	void UpdateStatus (PipeStatus status)
	{
		if (status == PipeStatus.Normal) {
			
		} else if (status == PipeStatus.Abnormal) {
		}
		Status.text = status.ToString ();
	}

	public void Deposit ()
	{
		Destroy (gameObject);
	}
}
