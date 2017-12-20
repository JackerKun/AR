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
		GlobalManager.CURRENT_SCENE_SERVICE = sceneName;
		if (sceneName == "tank")
		{
			TankSocketService.Instance.RegistServices();
		}
		else if (sceneName == "inspection")
		{
			InspectionSocketService.Instance.RegistServices();
		}
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
		socketService.AddListener(EventConfig.WARN_MESSAGE,
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

	public static void WARN_MESSAGE()
	{
		SocketService socketService = WebManager.socketInstance;
		socketService.AddListener(EventConfig.WARN_MESSAGE,
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