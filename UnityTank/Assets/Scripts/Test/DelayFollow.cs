using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayFollow : MonoBehaviour
{
	public float speed = .1f;
	// Update is called once per frame
	void Update ()
	{
		UpdatePosition ();
	}

	void UpdatePosition ()
	{
		Vector3 p = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10));

		transform.position = Vector3.Lerp (transform.position, p, speed);
	}
}
