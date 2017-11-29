/// <summary>
/// ar4ds
/// at first we need create two object in the scene;
/// 1. parent object (empty gameobject)
/// 2. child object (camera object)
/// 3. drag the script onto child object
/// </summary>

using UnityEngine;
using System.Collections;

public class GyroCamera : MonoBehaviour
{

	Gyroscope gyro;
	Quaternion quatMult;
	Quaternion quatMap;

	void Start ()
	{
		Init ();
	}

	public void Init ()
	{
		Transform camParent = transform.parent;
		// find the current parent of the camera's transform
		// instantiate a new transform
		// match the transform to the camera position
		camParent.position = transform.position;
		// make the new transform the parent of the camera transform
		transform.parent = camParent;

		gyro = Input.gyro;

		gyro.enabled = true;
		camParent.eulerAngles = new Vector3 (90, 0, 0);
		quatMult = new Quaternion (0, 0, 1, 0);

	}

	void Update ()
	{

		quatMap = new Quaternion (gyro.attitude.x, gyro.attitude.y, gyro.attitude.z, gyro.attitude.w);
		Quaternion qt = quatMap * quatMult;

		transform.localRotation = qt;
	}
}