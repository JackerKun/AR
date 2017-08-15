using UnityEngine;
using System.Collections;
using AR.Common;
using AR.Configs;
using AR.Model;
using BestHTTP;
using BestHTTP.SocketIO;
using SimpleJSON;

public class TankSceneService
{
	private SocketService socketService;
	private Socket socket;

	private System.Action<Tank> myCallback;

	// private delegate void mCallback (Socket socket, Packet packet, params object[] args);
	// Use this for initialization
	public TankSceneService ()
	{
		socketService = new SocketService ();
	}

	//当检索到识别物时触发
	public void onScaning (string targetID, System.Action<Tank> callback)
	{
		myCallback = callback;
		socketService.Subscribe (EventConfig.RESPONSE_BUCKET, targetID, OnResponseBucket);
	}

	//当离开识别物时
	public void onLostScaning (string targetID)
	{
		socketService.Cancel (EventConfig.RESPONSE_BUCKET, targetID);
	}

	//连接成功后回调
	private void OnResponseBucket (Socket socket, Packet packet, params object[] args)
	{
		Debug.Log ("Connect...");
		JSONNode jRoot = JSON.Parse (packet.Payload);
		//TODO; 数据的转换
		JSONNode data = jRoot [1];
		Tank bucket = new Tank (data ["isOpen"].AsBool, data ["IsCurrentTarget"].AsBool, data ["IsTrigger"].AsBool, data ["liquidLevel"].AsFloat);
		myCallback (bucket);
	}
}

