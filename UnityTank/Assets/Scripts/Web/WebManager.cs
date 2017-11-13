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

	public static SocketService socketInstance {
		get { 
			if (_socketService == null) {
				_socketService = new SocketService ();
			}
			return _socketService;
		}
	}


	public static void Init ()
	{
		#region 服务器注册
		SocketService socketService = WebManager.socketInstance;
		IsConnect = false;
		//心跳连接
		socketService.Register (
			EventConfig.REQUEST_HEART,
			EventConfig.RESPONSE_HEART, 
			(socket, packet, args) => {
				Debug.Log ("yes!");
				IsConnect = true;
			});
		//监听流程
		socketService.ServerInfo (EventConfig.AR_WORKFLOW, 
			(socket, packet, args) => {
				Debug.Log (packet.Payload);
				DealState (packet.Payload);
			});
		//监听错误信息
		socketService.ServerInfo (EventConfig.WARN_MESSAGE, 
			(socket, packet, args) => {
				Debug.Log (packet.Payload);
				UIManager.ShowErrorMessage (JSON.Parse (packet.Payload) [1] ["message"]);
			});
		socketService.AddListener (
			EventConfig.AR_DISCONNECT,
			(socket, packet, args) => {
				Debug.Log (packet.Payload);
				UIManager.ShowErrorMessage (JSON.Parse (packet.Payload) [1] ["message"]);
			});
		socketService.InitScene ("1",
			(socket, packet, args) => {
				Debug.Log (packet.Payload);
				DealState (packet.Payload, true);
				Debug.LogError ("Init Scene..");
			});
		socketService.AddListener (EventConfig.PHOTO,
			(socket, packet, args) => {
				Debug.LogError ("Callback PHOTO --> " + JSON.Parse (packet.Payload));
				UIManager.ShowStayMessage (MessageLibrary.GetMessage (JSON.Parse (packet.Payload) [1] ["status"]));
				MainSceneMgr.MainMgr.LoadScene ("TakePhoto");
			});
		#endregion
	}

	static void DealState (string payload, bool isOnline = false)
	{
		JSONNode jn = JSON.Parse (payload) [1];
		if (jn ["status"] == "errror") {
			UIManager.ShowErrorMessage (jn ["message"]);
		} else {
			string index;
			if (isOnline) {
				index = jn ["data"] ["job"] ["status"];
			} else {
				index = jn ["data"] ["status"];
			}
			string curStep = MessageLibrary.GetMessage (index);
			if (curStep != "11" || curStep != "13") {
				UIManager.ShowStayMessage ("");
			} else {
				UIManager.ShowStayMessage (curStep);
			}
			if (index == "10") {
				MainSceneMgr.MainMgr.LoadScene ("Tank");
			} else if (index == "14") {
				MainSceneMgr.MainMgr.LoadScene ("WorkFlow");
			}
		}
	}

	public static void WARN_MESSAGE ()
	{
		SocketService socketService = WebManager.socketInstance;
		socketService.ServerInfo (EventConfig.WARN_MESSAGE, 
			(socket, packet, args) => {
				Debug.Log (packet.Payload);
				UIManager.ShowErrorMessage (JSON.Parse (packet.Payload) [1] ["message"]);
			});
		socketService.AddListener (
			EventConfig.AR_DISCONNECT,
			(socket, packet, args) => {
				Debug.Log (packet.Payload);
				UIManager.ShowErrorMessage (JSON.Parse (packet.Payload) [1] ["message"]);
			});
	}
}