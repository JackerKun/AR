// iOS gyroscope example
//
// Create a cube with camera vector names on the faces.
// Allow the iPhone to show named faces as it is oriented.

using UnityEngine;

public class ExampleScript : MonoBehaviour
{
	private Gyroscope gyro;

	void Start ()
	{
		// make camera solid colour and based at the origin
		Camera.main.backgroundColor = Color.black;
		Camera.main.transform.position = new Vector3 (0, 0, 0);
		Camera.main.clearFlags = CameraClearFlags.SolidColor;
		if (SystemInfo.supportsGyroscope) {
			gyro = Input.gyro;
			gyro.enabled = true;
		}

		// create the six quads forming the sides of a cube
		GameObject quad = GameObject.CreatePrimitive (PrimitiveType.Quad);

		createQuad (quad, Vector3.left, Vector3.down, "left", Color.cyan);
		createQuad (quad, Vector3.right, Vector3.up, "right", Color.magenta);
		createQuad (quad, Vector3.up, Vector3.left, "up", Color.blue);
		createQuad (quad, Vector3.down, Vector3.right, "down", Color.yellow);
		createQuad (quad, Vector3.forward, Vector3.zero, "forward", Color.white);
		createQuad (quad, Vector3.back, Vector3.right * 2, "back", Color.black);

		GameObject.Destroy (quad);
	}

	// make a quad for one side of the cube
	GameObject createQuad (GameObject quad, Vector3 pos, Vector3 rot, string name, Color col)
	{
		Quaternion quat = Quaternion.Euler (rot * 90f);
		GameObject GO = Instantiate (quad, pos, quat);
		GO.name = name;
		GO.GetComponent<Renderer> ().material.color = col;
		GO.transform.localScale += new Vector3 (0.25f, 0.25f, 0.25f);
		return GO;
	}

	protected void Update ()
	{
		GyroModifyCamera ();
	}

	protected void OnGUI ()
	{
		GUI.skin.label.fontSize = Screen.width / 40;

		GUILayout.Label ("Orientation: " + Screen.orientation);
//		GUILayout.Label ("input.gyro.attitude: " + Input.gyro.attitude);
		GUILayout.Label ("input.gyro.attitude: " + new Quaternion (Input.gyro.attitude.x * 180f, Input.gyro.attitude.y * 180f, Input.gyro.attitude.z * 180f, Input.gyro.attitude.w * 180f));
		GUILayout.Label ("iphone width/font: " + Screen.width + " : " + GUI.skin.label.fontSize);
	}

	/********************************************/

	// The Gyroscope is right-handed.  Unity is left handed.
	// Make the necessary change to the camera.
	void GyroModifyCamera ()
	{
		transform.rotation = GyroToUnity (Input.gyro.attitude);
	}

	public Quaternion roteQX, roteQY, roteQZ;

	private Quaternion GyroToUnity (Quaternion q)
	{
//		return new Quaternion (q.x, q.y, -q.z, -q.w);
		//q=((x,y,z)sin(θ/2), cos(θ/2)) 
		return new Quaternion (-q.y, -q.z, -q.x, q.w) * roteQX * roteQY * roteQZ;
//		return new Quaternion (-q.x, -q.z, -q.y, q.w) * roteQX * roteQY * roteQZ;
//		return q * roteQX * roteQY * roteQZ;
//		return new Quaternion (-q.y, q.x, q.z, -q.w);
	}
}