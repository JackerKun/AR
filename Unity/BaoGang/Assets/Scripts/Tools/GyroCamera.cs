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
	//水平线的角度
	public float horizonAngle = 0;
	public Vector3 gyroAngle = Vector3.zero;
	public Vector3 forward = Vector3.zero;

	void Awake()
	{
		Init();
	}

	public void Init()
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
		camParent.eulerAngles = new Vector3(90, 0, 0);
		quatMult = new Quaternion(0, 0, 1, 0);

	}

	void Update()
	{
		quatMap = new Quaternion(gyro.attitude.x, gyro.attitude.y, gyro.attitude.z, gyro.attitude.w);
		Quaternion qt = quatMap * quatMult;

		transform.localRotation = qt;

		horizonAngle = Vector3.Angle(transform.up, Vector3.up);
		forward = Vector3.Cross(transform.right, Vector3.up);
		CalGyroAngle();
	}

	void CalGyroAngle()
	{
		float xAngle = Vector3.Angle(Vector3.up, transform.forward);
		float zAngle = 180f - Vector3.Angle(Vector3.up, transform.right);
		gyroAngle = new Vector3(xAngle, 0, zAngle);
	}
}