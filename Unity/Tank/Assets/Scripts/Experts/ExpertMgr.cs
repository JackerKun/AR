using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpertMgr
{
	static ExpertMgr _instance;
	static ExpertSocketService service;
	static WebCamTexture webCamTex;
	static Renderer MyPlane;

	public static void Init ()
	{
		service = new ExpertSocketService ();
		webCamTex = new WebCamTexture ();
		webCamTex.Play ();
	}

	public Texture GetCapture ()
	{
		return webCamTex;
	}

	public static ExpertMgr Instance {
		get { 
			if (_instance == null) {
				_instance = new ExpertMgr ();
			}
			return _instance;
		}	
	}

	public void CaptureScreen ()
	{
		MyPlane.material.mainTexture = webCamTex;
		Debug.Log ("Screen Captured");
	}
}
