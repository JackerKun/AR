using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Test : MonoBehaviour
{
	public Texture pic;
	// Use this for initialization
	void Start ()
	{
		InitTexture ();
		GameObject plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
		plane.GetComponent<Renderer> ().material.mainTexture = pic;
	}

	void InitTexture ()
	{
		using (StreamReader sr = File.OpenText ("/Users/wangyinan/Downloads/a.txt")) {
			string base64Txt = sr.ReadToEnd ();
			Debug.Log (base64Txt);
			byte[] b = Convert.FromBase64String (base64Txt);
			Texture2D tx2d = new Texture2D (800, 800);
			tx2d.LoadImage (b);
			pic = tx2d;
		}
	}
}