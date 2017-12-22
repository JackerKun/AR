using UnityEngine;
using AR.Common;
using AR.Configs;
using AR.Model;
using BestHTTP.SocketIO;
using SimpleJSON;
using HopeRun;

public class InspectionSocketService
{
	private SocketService socketService;
	private Socket socket;

	private System.Action<Tank> myCallback;

	static InspectionSocketService _instance;

	public static InspectionSocketService Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new InspectionSocketService();
			}
			return _instance;
		}
	}

	public void RegistServices()
	{
		socketService.InitScene("inspection",
			(socket, packet, args) =>
			{
				Debug.Log("Init Scene.." + packet.Payload);
				DealState(packet.Payload, true);
			});
		//监听流程
		socketService.AddListener(EventConfig.AR_CHECKPOINT,
			(socket, packet, args) =>
			{
				DealState(packet.Payload);
			});
	}

	void DealState(string payload, bool isOnline = false)
	{
		JSONNode jn = JSON.Parse(payload)[1];
		if (jn["status"] == "error")
		{
			UIManager.ShowErrorMessage(jn["message"]);
		}
		else
		{
			SceneMsgDealer.DealInspectionMsg(jn);
		}
	}

	public InspectionSocketService()
	{
		socketService = WebManager.Instance.socket;
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

