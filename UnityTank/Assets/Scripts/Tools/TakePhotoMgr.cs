using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
using HopeRun;
using SimpleJSON;
using HopeRun;
using AR.Model;
using BestHTTP;
using BestHTTP.SocketIO;
using AR.Common;
using AR.Configs;

public class TakePhotoMgr : MonoBehaviour
{
	private SocketService socketService;
	public RawImage gTex;
	static string photoBase64;
	static WebCamTexture webcamTexture;
	float sourceRate, screenRate;
	Text countDownNum;

	void Start ()
	{
		webcamTexture = new WebCamTexture ();
		webcamTexture.Play ();
		gTex.texture = webcamTexture;
		countDownNum = GameObject.Find ("Canvas/CountDownNum").GetComponent<Text> ();
		countDownNum.text = "";
		StartTakePhoto ();
	}

	void UpdateCameraCanvasSize ()
	{
		screenRate = (float)Screen.width / Screen.height;
		sourceRate = (float)webcamTexture.width / webcamTexture.height;
		if (screenRate > sourceRate) {
			gTex.transform.localScale = new Vector3 (1, screenRate / sourceRate, 1);
		} else {
			gTex.transform.localScale = new Vector3 (sourceRate / screenRate, 1, 1);
		}
	}

	void FixedUpdate ()
	{
		UpdateCameraCanvasSize ();
	}

	public void StartTakePhoto ()
	{
		StartCoroutine (CountDownTakePhoto ());
	}

	IEnumerator CountDownTakePhoto ()
	{
//		yield return new WaitForSeconds (5);
		int num = 3;
		for (int i = num; i > 0; i--) {
			countDownNum.text = num.ToString ();
			--num;
			yield return new WaitForSeconds (1f);
		}
		countDownNum.text = "";
		TakePhoto ();
	}

	void TakePhoto ()
	{
		int width = webcamTexture.width;
		int height = webcamTexture.height;
		Texture2D texture2D = new Texture2D (width, height);
		texture2D.SetPixels (webcamTexture.GetPixels ());
		texture2D.Apply ();
//		File.WriteAllBytes (path, texture2D.EncodeToJPG ());
//		FileInfo info = new FileInfo (path);
//		info.Attributes = FileAttributes.Normal;
		GlobalManager.CURRENT_PHOTO_BASE64 = Convert.ToBase64String (texture2D.EncodeToJPG ());
//		Debug.Log (path);
		SendPictureToServer ();
	}

	void SendPictureToServer ()
	{
		socketService = WebManager.socketInstance;
		PhotoObj requestObj = new PhotoObj ();
		socketService.mySocket.Emit (EventConfig.AR_PHOTO, JsonUtility.ToJson (requestObj));
		//停止拍摄
		webcamTexture.Stop ();
		//返回场景
		MainSceneMgr.MainMgr.LoadScene (GlobalManager.LAST_LOADED_SCENE);
	}
}