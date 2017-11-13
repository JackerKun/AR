using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CameraCorrector : MonoBehaviour
{

	Matrix4x4 originalProjection;
	Camera cam;
	// Use this for initialization
	void Start ()
	{
		cam = Camera.main;
//		RotationalDeviceTracker.MODEL_CORRECTION_MODE.HEAD
	}

	void Update ()
	{
		Matrix4x4 p = originalProjection;
		p.m01 += Mathf.Sin (Time.time * 1.2F) * 0.1F;
		p.m10 += Mathf.Sin (Time.time * 1.5F) * 0.1F;
		cam.projectionMatrix = p;
	}
}
