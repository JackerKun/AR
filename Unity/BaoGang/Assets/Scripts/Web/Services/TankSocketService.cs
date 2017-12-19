using UnityEngine;
using System.Collections;
using AR.Common;
using AR.Configs;
using AR.Model;
using BestHTTP.SocketIO;
using SimpleJSON;
using HopeRun;
using HopeRun.Message;

public class TankSocketService
{
	private SocketService socketService;
	private Socket socket;

	private System.Action<Tank> myCallback;

	// private delegate void mCallback (Socket socket, Packet packet, params object[] args);
	// Use this for initialization

	static TankSocketService _instance;

	public static TankSocketService Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new TankSocketService();
			}
			return _instance;
		}
	}

	public void RegistServices()
	{
		socketService.InitScene("tank",
			(socket, packet, args) =>
			{
				Debug.LogError("Init Scene.." + packet.Payload);
				DealState(packet.Payload, true);
			});
		//监听流程
        socketService.AddListener(EventConfig.AR_WORKFLOW,
			(socket, packet, args) =>
			{
				Debug.Log(packet.Payload);
				DealState(packet.Payload);
			});
		socketService.AddListener(EventConfig.PHOTO,
			(socket, packet, args) =>
			{
				Debug.LogError("Callback PHOTO --> " + JSON.Parse(packet.Payload));
				UIManager.ShowStayMessage(MessageLibrary.GetMessage(JSON.Parse(packet.Payload)[1]
["status"]));
				MainSceneMgr.MainMgr.LoadScene("TakePhoto");
			});
	}

	void DealState(string payload, bool isOnline = false)
	{
		Debug.Log(JSON.Parse(payload));
		JSONNode jn = JSON.Parse(payload)[1];
		if (jn["status"] == "error")
		{
			UIManager.ShowErrorMessage(jn["message"]);
		}
		else
		{
			SceneMsgDealer.DealTankMsg(jn, isOnline);
		}
	}

	public TankSocketService()
	{
		socketService = WebManager.socketInstance;
	}

	//当检索到识别物时触发
	public void onScaning(System.Action<Tank> callback)
	{
		myCallback = callback;
		Debug.Log("onScaning:" + EventConfig.RESPONSE_TANK);
		socketService.Subscribe(EventConfig.RESPONSE_TANK, OnResponseTank);
	}

	//当离开识别物时
	public void onLostScaning(string targetID)
	{
		socketService.DisSubscribe(EventConfig.RESPONSE_TANK);
		socketService.Cancel(EventConfig.RESPONSE_TANK);
	}

	//连接成功后回调
	private void OnResponseTank(Socket socket, Packet packet, params object[] args)
	{
		Debug.Log("Connect...");
		JSONNode jRoot = JSON.Parse(packet.Payload)[1];

		if (jRoot["status"] == "success")
		{
			Debug.Log(packet.Payload);
			//TODO; 数据的转换
			JSONNode data = jRoot["data"];
			Tank bucket = new Tank(data["liquidHeight"].AsFloat, data["limitLevel"].AsFloat, data["highestLevel"].AsFloat, data["valveStatus"].AsBool, data["blowerStatus"].AsBool);
			GlobalManager.IS_WORKFLOW = (data["sceneName"] == "workflow");
			myCallback(bucket);
		}
		else
		{
			UIManager.ShowErrorMessage(jRoot["message"]);
		}
	}
}

