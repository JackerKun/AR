using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustMatrix : MonoBehaviour
{
	public float matrixV = 1f;
	Camera currentCamera;
	Matrix4x4 initMatrix;
	// Use this for initialization
	void Start ()
	{
		currentCamera = GetComponent<Camera> ();
		initMatrix = currentCamera.projectionMatrix;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Matrix4x4 tran0 = new Matrix4x4 (
			                  new Vector4 (matrixV, 0, 0, 0),
			                  new Vector4 (0, matrixV, 0, 0),
			                  new Vector4 (0, 0, 1, 0),
			                  new Vector4 (0, 0, 0, 1)
		                  );
		Matrix4x4 tran1 = new Matrix4x4 (
			                  new Vector4 (1, 0, 0, 0),
			                  new Vector4 (0, 1, 0, 0),
			                  new Vector4 (0, 0, 1, 0),
			                  new Vector4 (matrixV, matrixV, matrixV, 1)
		                  );
		currentCamera.projectionMatrix = tran1 * initMatrix;
	}
}
