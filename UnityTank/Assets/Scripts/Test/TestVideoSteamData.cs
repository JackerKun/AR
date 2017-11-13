using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Vuforia;
using System.Text;

public class TestVideoSteamData : MonoBehaviour
{
	public Transform tran;
	// Use this for initialization
	void Start ()
	{
		Vuforia.VuforiaARController.Instance.RegisterVuforiaInitializedCallback (() => {
			Debug.Log ("can i capture?");
//			Debug.Log (Vuforia.CameraDevice.Instance.GetCameraImage (Image.PIXEL_FORMAT.RGB565).Pixels);
//			Debug.Log (Vuforia.CameraDevice.Instance.GetCameraImage (Image.PIXEL_FORMAT.GRAYSCALE).Pixels);
//			Debug.Log (Vuforia.CameraDevice.Instance.GetCameraImage (Image.PIXEL_FORMAT.RGB888).Pixels);
//			Debug.Log (Vuforia.CameraDevice.Instance.GetCameraImage (Image.PIXEL_FORMAT.RGBA8888).Pixels);
//			Debug.Log (Vuforia.CameraDevice.Instance.GetCameraImage (Image.PIXEL_FORMAT.UNKNOWN_FORMAT).Pixels);
//			Debug.Log (Vuforia.CameraDevice.Instance.GetCameraImage (Image.PIXEL_FORMAT.YUV).Pixels);
		});
		//this.Pixels[]
	}

	public GUITexture gTex;
	// Update is called once per frame
	public Vuforia.Image.PIXEL_FORMAT iFormat;

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			Vuforia.CameraDevice.Instance.SetFrameFormat (iFormat, true);
			Image captureImg = Vuforia.CameraDevice.Instance.GetCameraImage (iFormat);
			byte[] imageData = captureImg.Pixels;
			Texture2D txt2d = new Texture2D (captureImg.Width, captureImg.Height);
			captureImg.CopyToTexture (txt2d);
////			txt2d.LoadImage (imageData);
//			Color[] colors = GetPixels2 (imageData);
//			txt2d.SetPixels (colors);
//			Debug.Log (new Vector2 (captureImg.Width, captureImg.Height) + " >> " + imageData.Length + " >> " + colors.Length);
//			StringBuilder colorStr = new StringBuilder ();
//			foreach (Color c in colors) {
//				colorStr.Append (c + "\t");
//			}
//			Debug.Log (colorStr);
			gTex.texture = txt2d;
			tran.GetComponent<Renderer> ().material.mainTexture = txt2d;
		}
	}

	Color[] GetPixels2 (byte[] bts)
	{
		List<Color> colors = new List<Color> ();
		Color32 tmpC = Color.black;
		Debug.LogError (tmpC);
		for (int i = 0; i < bts.Length; i += 4) {
			//colors.Add (new Color32 (bts [i], bts [i + 1], bts [i + 2], bts [i + 3]));
			colors.Add (tmpC);
		}
		return colors.ToArray ();
	}

	Color32[] GetPixels (byte[] bts)
	{
		List<Color32> colors = new List<Color32> ();
		Color32 tmpC = Color.red;//new Color32 (255, 0, 0, 255);

		tmpC.r = Convert.ToByte (0);
		tmpC.g = Convert.ToByte (0);
		tmpC.b = Convert.ToByte (0);
		tmpC.a = Convert.ToByte (0);
		Debug.LogError (tmpC);
		for (int i = 0; i < bts.Length; i += 4) {
			//colors.Add (new Color32 (bts [i], bts [i + 1], bts [i + 2], bts [i + 3]));
			colors.Add (tmpC);
		}
		return colors.ToArray ();
	}

	void FixedUpdate ()
	{
		tran.Rotate (Vector3.up, Space.World);
	}
}
