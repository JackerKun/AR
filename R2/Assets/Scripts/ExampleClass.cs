﻿// Set an off-center projection, where perspective's vanishing
// point is not necessarily in the center of the screen.
//
// left/right/top/bottom define near plane size, i.e.
// how offset are corners of camera's near plane.
// Tweak the values and you can see camera's frustum change.

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ExampleClass : MonoBehaviour
{
	public float left = -0.2F;
	public float right = 0.2F;
	public float top = 0.2F;
	public float bottom = -0.2F;
	Matrix4x4 originalProjection;
	Camera cam;

	void Start ()
	{
		cam = Camera.main;
		cam.transform.localPosition = new Vector3 (.1f, .2f, 0);
		originalProjection = cam.projectionMatrix;
	}

	//	void LateUpdate ()
	//	{
	//		Matrix4x4 p = originalProjection;
	//		p.m01 += Mathf.Sin (Time.time * 1.2F) * 0.1F;
	//		p.m10 += Mathf.Sin (Time.time * 1.5F) * 0.1F;
	//		cam.projectionMatrix = p;
	//	}

	void LateUpdate ()
	{
		Camera cam = Camera.main;
		Matrix4x4 m = PerspectiveOffCenter (left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane);
		cam.projectionMatrix = m;
	}

	public float scaleFactor = 1.0f;

	Matrix4x4 PerspectiveOffCenter (float left, float right, float bottom, float top, float near, float far)
	{
		float x = 2.0f * near / (right - left) * scaleFactor;
		float y = 2.0f * near / (top - bottom) * scaleFactor;
		float a = (right + left) / (right - left);
		float b = (top + bottom) / (top - bottom);
		float c = -(far + near) / (far - near);
		float d = -(2.0F * far * near) / (far - near);
		float e = -1.0F;
		Matrix4x4 m = new Matrix4x4 ();
		m [0, 0] = x;
		m [0, 1] = 0;
		m [0, 2] = a;
		m [0, 3] = 0;
		m [1, 0] = 0;
		m [1, 1] = y;
		m [1, 2] = b;
		m [1, 3] = 0;
		m [2, 0] = 0;
		m [2, 1] = 0;
		m [2, 2] = c;
		m [2, 3] = d;
		m [3, 0] = 0;
		m [3, 1] = 0;
		m [3, 2] = e;
		m [3, 3] = 0;
		/// x 0 a 0
		/// 0 y b 0
		/// 0 0 c d
		/// 0 0 e 0
		return m;
	}
}