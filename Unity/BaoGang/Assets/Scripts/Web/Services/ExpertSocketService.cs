using UnityEngine;
using System.Collections;
using AR.Common;
using AR.Configs;
using HopeRun.Model;
using AR.Model;
using BestHTTP;
using BestHTTP.SocketIO;
using SimpleJSON;
using HopeRun;

public class ExpertSocketService
{
	private SocketService socketService;
	private Socket socket;

	// private delegate void mCallback (Socket socket, Packet packet, params object[] args);
	// Use this for initialization
	public ExpertSocketService ()
	{
//		socketService = WebManager.Instance.socket;
//		//开启连接，监听专家的截图消息
//		socketService.Register (
//			EventConfig.REQUEST_CAPTURE_REGISTER,
//			EventConfig.RESPONSE_CAPTURE_START, 
//			(socket, packet, args) => {
//				GameObject.Find ("CaptureTexture").GetComponent<MeshRenderer> ().material.mainTexture = ExpertMgr.Instance.GetCapture ();
//
//				TargetDeviceRequest requestObj = new TargetDeviceRequest (true, GlobalManager.DeviceID, "");
//				socketService.mySocket.Emit (EventConfig.RESPONSE_CAPTURE_START, JsonUtility.ToJson (requestObj));
//				Debug.LogError ("REQUEST_VIDEO-->callback");
//			});
//		Debug.LogError (socketService.mySocket.IsOpen);
//		//截图并发送给服务器
//		socketService.Register (
//			EventConfig.REQUEST_CAPTURE, 
//			EventConfig.RESPONSE_CAPTURE, 
//			(socket, packet, args) => {
//				Debug.LogError ("REQUEST_CAPTURE_START-->callback");
//			});
		
	}
}

