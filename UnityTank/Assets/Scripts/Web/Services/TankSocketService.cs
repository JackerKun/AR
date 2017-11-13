using UnityEngine;
using System.Collections;
using AR.Common;
using AR.Configs;
using AR.Model;
using BestHTTP;
using BestHTTP.SocketIO;
using SimpleJSON;
using HopeRun;

public class TankSocketService
{
	private SocketService socketService;
	private Socket socket;

	private System.Action<Tank> myCallback;

	// private delegate void mCallback (Socket socket, Packet packet, params object[] args);
	// Use this for initialization

	static TankSocketService _instance;

	public static TankSocketService Instance {
		get { 
			if (_instance == null) {
				_instance = new TankSocketService ();
			}
			return _instance;
		}
	}

	public TankSocketService ()
	{
		socketService = WebManager.socketInstance;
	}

	//当检索到识别物时触发
	public void onScaning (System.Action<Tank> callback)
	{
		myCallback = callback;
		Debug.Log ("onScaning:" + EventConfig.RESPONSE_TANK);
		socketService.Subscribe (EventConfig.RESPONSE_TANK, OnResponseTank);
	}

	//当离开识别物时
	public void onLostScaning (string targetID)
	{
		socketService.DisSubscribe (EventConfig.RESPONSE_TANK);
		socketService.Cancel (EventConfig.RESPONSE_TANK, targetID);
	}

	//连接成功后回调
	private void OnResponseTank (Socket socket, Packet packet, params object[] args)
	{
		Debug.Log ("Connect...");
		JSONNode jRoot = JSON.Parse (packet.Payload) [1];

		if (jRoot ["status"] == "success") {
			Debug.Log (packet.Payload);
			//TODO; 数据的转换
			JSONNode data = jRoot ["data"];
			Tank bucket = new Tank (data ["liquidHeight"].AsFloat, data ["limitLevel"].AsFloat, data ["highestLevel"].AsFloat, data ["valveStatus"].AsBool, data ["blowerStatus"].AsBool);
			myCallback (bucket);
		} else {
			UIManager.ShowErrorMessage (jRoot ["message"]);
		}
	}
}

