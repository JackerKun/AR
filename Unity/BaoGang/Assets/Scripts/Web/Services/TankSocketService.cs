using System;
using UnityEngine;
using AR.Common;
using AR.Configs;
using AR.Model;
using BestHTTP.SocketIO;
using SimpleJSON;
using HopeRun;
using HopeRun.Message;
using HopeRun.Model;

public class TankSocketService : IRegistServer
{
	//private SocketService socketService;
	//private Socket socket;

	//private System.Action<Tank> myCallback;

	// private delegate void mCallback (Socket socket, Packet packet, params object[] args);
	// Use this for initialization

	private static TankSocketService _instance;
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

	public void AddSelfEvent()
	{
		Debug.LogError("Not Error");
		#region 旧的代码
		//        socketService.InitScene("tank",
		//            (socket, packet, args) =>
		//            {
		//                Debug.LogError("Init Scene.." + packet.Payload);
		//                DealState(packet.Payload, true);
		//            });
		//监听流程
		//        socketService.AddListener(EventConfig.AR_WORKFLOW,
		//            (socket, packet, args) =>
		//            {
		//                Debug.Log(packet.Payload);
		//                DealState(packet.Payload);
		//            });
		//        socketService.AddListener(EventConfig.PHOTO,
		//            (socket, packet, args) =>
		//            {
		//                Debug.LogError("Callback PHOTO --> " + packet.Payload);
		//                UIManager.ShowStayMessage(MessageLibrary.GetMessage(JSON.Parse(packet.Payload)[1]["status"]));
		//                MainSceneMgr.MainMgr.LoadScene("TakePhoto");
		//            });
		#endregion
		WebManager.Instance.Connect("tank", node =>
		{
			DealTankMsg(node[0], true);
        });
//        WebManager.Instance.On(EventConfig.AR_ONLINE, node =>
//        {
//            DealTankMsg(node[0], true);
//        });
		WebManager.Instance.On(EventConfig.AR_WORKFLOW, node =>
		{
			DealTankMsg(node[0]);
		});
		WebManager.Instance.On(EventConfig.PHOTO, node =>
		{
			UIManager.ShowStayMessage(MessageLibrary.GetMessage(node[1]["status"]));
			GlobalManager.LoadScene("TakePhoto");
		});
	}

//    public void FirstRequest()
//    {
//        LocalDeviceRequest requestDevice = new LocalDeviceRequest("tank");
//        WebManager.Instance.Emit(EventConfig.ONLINE, JsonUtility.ToJson(requestDevice));
//    }

    public void onScaning(Action<Tank> callback)
	{
		//接收实时数据
		WebManager.Instance.StartRequestData(EventConfig.TANK, EventConfig.RESPONSE_TANK, node =>
		 {
			 var tank = new Tank(node[0]);
			 GlobalManager.IS_WORKFLOW = (tank.sceneName.Equals("workflow"));
			 callback(tank);
		 });
	}
	public void onLostScaning()
	{
		//取消接收实时数据
		WebManager.Instance.Off(EventConfig.RESPONSE_TANK);
	}

	/// <summary>
	/// 处理加药流程
	/// </summary>
	/// <param name="jn"></param>
	/// <param name="isOnline"></param>
	public void DealTankMsg(JSONNode jn, bool isOnline = false)
	{
		string index = isOnline ? jn["job"]["status"] : jn["status"];
		if (string.IsNullOrEmpty(index)) return;

		string curStep = MessageLibrary.GetMessage(index);
		if (curStep == "10")
		{
			string tankARName = isOnline ? jn["job"]["prodTitle"] : jn["prodTitle"];
			curStep = MessageLibrary.GetMessage("ARTank_" + tankARName);
		}
		if (index == "11" || index == "13")
		{
			UIManager.ShowStayMessage("");
		}
		else
		{
			UIManager.ShowStayMessage(curStep);
		}

		GlobalManager.LoadScene(index == "10" ? "Tank" : "WorkFlow");
	}

	//
	//    void DealState(string payload, bool isOnline = false)
	//    {
	//        JSONNode jn = JSON.Parse(payload)[1];
	//        if (jn["status"] == "error")
	//        {
	//            UIManager.ShowErrorMessage(jn["message"]);
	//        }
	//        else
	//        {
	//            SceneMsgDealer.DealTankMsg(jn, isOnline);
	//        }
	//    }

	//    public TankSocketService()
	//    {
	//        socketService = WebManager.Instance.socket;
	//    }

	//当检索到识别物时触发
	//    //连接成功后回调
	//    private void OnResponseTank(Socket socket, Packet packet, params object[] args)
	//    {
	//        Debug.Log("Connect...");
	//        JSONNode jRoot = JSON.Parse(packet.Payload)[1];
	//
	//        if (jRoot["status"] == "success")
	//        {
	//            Debug.Log(packet.Payload);
	//            //TODO; 数据的转换
	//            JSONNode data = jRoot["data"];
	//            Tank bucket = new Tank(data);
	//            GlobalManager.IS_WORKFLOW = (bucket.sceneName.Equals("workflow"));
	//            myCallback(bucket);
	//        }
	//        else
	//        {
	//            UIManager.ShowErrorMessage(jRoot["message"]);
	//        }
	//    }
}

