using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AR.Common;
using AR.Configs;
using SimpleJSON;
using HopeRun.Message;
using HopeRun;

public class WebManager
{
	public static bool IsConnect = true;
	static SocketService _socketService;

	public static SocketService socketInstance
	{
		get
		{
			if (_socketService == null)
			{
				_socketService = new SocketService();
			}
			return _socketService;
		}
	}

	public static void Init(string sceneName)
	{
		#region 服务器注册
		SocketService socketService = socketInstance;
		IsConnect = false;
		//通用的注册服务
		PublicRegistListener(socketService);
		socketService.InitScene(sceneName,
			(socket, packet, args) =>
			{
				Debug.LogError("Init Scene.." + packet.Payload);
				GlobalManager.CURRENT_SCENE_SERVICE = sceneName;
				DealState(packet.Payload, true);
			});
		//监听流程
		socketService.ServerInfo(EventConfig.AR_WORKFLOW,
			(socket, packet, args) =>
			{
				Debug.Log(packet.Payload);
				DealState(packet.Payload);
			});
		socketService.AddListener(EventConfig.PHOTO,
			(socket, packet, args) =>
			{
				Debug.LogError("Callback PHOTO --> " + JSON.Parse(packet.Payload));
				UIManager.ShowStayMessage(MessageLibrary.GetMessage(JSON.Parse(packet.Payload)[1]["status"]));
				MainSceneMgr.MainMgr.LoadScene("TakePhoto");
			});
		#endregion
	}

	#region 公用监听

	static void PublicRegistListener(SocketService socketService)
	{
		//心跳连接
		socketService.Register(
			EventConfig.REQUEST_HEART,
			EventConfig.RESPONSE_HEART,
			(socket, packet, args) =>
			{
				Debug.Log("yes!");
				IsConnect = true;
			});
		//监听错误信息
		socketService.ServerInfo(EventConfig.WARN_MESSAGE,
			(socket, packet, args) =>
			{
				Debug.Log(packet.Payload);
				UIManager.ShowErrorMessage(JSON.Parse(packet.Payload)[1]["message"]);
			});
		socketService.AddListener(
			EventConfig.AR_DISCONNECT,
			(socket, packet, args) =>
			{
				Debug.Log(packet.Payload);
				UIManager.ShowErrorMessage(JSON.Parse(packet.Payload)[1]["message"]);
			});
	}

	#endregion

	static void DealState(string payload, bool isOnline = false)
	{
		Debug.Log(JSON.Parse(payload));
		JSONNode jn = JSON.Parse(payload)[1];
		if (jn["status"] == "error")
		{
			UIManager.ShowErrorMessage(jn["message"]);
		}
		else
		{
			if (GlobalManager.CURRENT_SCENE_SERVICE == "tank")
			{
				SceneMsgDealer.DealTankMsg(jn, isOnline);
			}
			else if (GlobalManager.CURRENT_SCENE_SERVICE == "inspection")
			{
				SceneMsgDealer.DealInspectionMsg(jn, isOnline);
			}
		}
	}

	public static void WARN_MESSAGE()
	{
		SocketService socketService = WebManager.socketInstance;
		socketService.ServerInfo(EventConfig.WARN_MESSAGE,
			(socket, packet, args) =>
			{
				Debug.Log(packet.Payload);
				UIManager.ShowErrorMessage(JSON.Parse(packet.Payload)[1]["message"]);
			});
		socketService.AddListener(
			EventConfig.AR_DISCONNECT,
			(socket, packet, args) =>
			{
				Debug.Log(packet.Payload);
				UIManager.ShowErrorMessage(JSON.Parse(packet.Payload)[1]["message"]);
			});
	}
}