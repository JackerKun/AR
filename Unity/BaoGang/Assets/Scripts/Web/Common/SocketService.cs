using System;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Events;
using AR.Configs;
using HopeRun.Model;
using UnityEngine;
using HopeRun;

namespace AR.Common
{

	public class SocketService
	{

		//Set Namespace For Current
		public string currentChannel;

		public SocketManager socketManagerRef;

		private Socket _socket;

		public Socket mySocket
		{
			get
			{
				if (_socket != null) return _socket;
				options = new SocketOptions
				{
					AutoConnect = true,
					Reconnection = true,
					ReconnectionDelay = TimeSpan.FromMilliseconds(100)
				};
				socketManagerRef = new SocketManager(GlobalManager.IP, options);
				_socket = socketManagerRef.Socket;
				return _socket;
			}
		}

		SocketOptions options;

		bool isConnectSucceed = false;
		TargetDeviceRequest requestObj;

		private string deviceID = SystemInfo.deviceUniqueIdentifier;


		#region PUBLIC_METHODS

		public SocketService()
		{
		}

		public SocketService(SocketOptions options)
		{
			this.options = options;
		}

		public void InitScene2(string sceneID, SocketIOCallback callback)
		{
			TryConnect();
			//连接失败
			if (isConnectSucceed)
			{
				LocalDeviceRequest requestObj = new LocalDeviceRequest(sceneID);
				mySocket.Emit(EventConfig.ONLINE, JsonUtility.ToJson(requestObj));
				mySocket.On(EventConfig.AR_ONLINE, callback);
				Debug.Log("register socket..");
			}
		}

		public void Dispose()
		{
			Debug.LogError("dispose listener..");
			if (_socket != null)
			{
				mySocket.Off(EventConfig.AR_ONLINE, (e0, e1, e2) =>
				{
					Debug.Log("dispose listener..");
				});
			}
		}

		void TryConnect()
		{
			//如果初次连接socket
			mySocket.Manager.Handshake.OnError += (e1, e2) =>
			{
				//				Reconnect ();
			};
			mySocket.Manager.Handshake.OnReceived += (A) =>
			{
				isConnectSucceed = true;
				Debug.Log("connect succeed!");
			};
			mySocket.Manager.Open();
			Debug.Log(mySocket.IsOpen);
		}

		public void Reconnect()
		{
			mySocket.Manager.Close();
			_socket = null;
			Debug.LogError("Reconnect");
			//			MySocket.Manager.Open ();
		}

		public void InitScene(string sceneName, SocketIOCallback callback)
		{

			mySocket.Manager.Handshake.OnError += (e1, e2) =>
			{
				Debug.LogError("HandshakeError" + e2);
			};
			mySocket.Manager.Handshake.OnReceived += (A) =>
			{
				UIManager.ShowMessage("已成功连接服务");
				Debug.Log("Handshake OnReceived");
			};
			LocalDeviceRequest requestDevice = new LocalDeviceRequest(sceneName);

			mySocket.Emit(EventConfig.ONLINE, JsonUtility.ToJson(requestDevice));
			mySocket.On(EventConfig.AR_ONLINE, callback);
			Debug.Log("register socket..");
		}

		//加入监听
		public void AddListener(string response, SocketIOCallback callback)
		{
			mySocket.On(response, callback);
			Debug.Log("add listener.." + response);
		}

		public void Register(string request, string response, SocketIOCallback callback)
		{
			requestObj = new TargetDeviceRequest(true, deviceID, "1");
			mySocket.Emit(request, JsonUtility.ToJson(requestObj));
			mySocket.On(response, callback);
			Debug.Log(request + " --- " + response + " register socket..");

		}


		//订阅指定事件
		public void Subscribe(string eventName, SocketIOCallback callback)
		{
			//如果初次连接socket
			requestObj = new TargetDeviceRequest(true, deviceID, GlobalManager.CURRENT_TANKID);
			Debug.Log(requestObj);
			mySocket.Emit(EventConfig.TANK, JsonUtility.ToJson(requestObj));
			mySocket.On(eventName, callback);
		}

		//订阅指定事件
		public void DisSubscribe(string eventName)
		{
			//如果初次连接socket
			requestObj = new TargetDeviceRequest(false, deviceID, GlobalManager.CURRENT_TANKID);
			Debug.Log(requestObj);
			mySocket.Emit(EventConfig.TANK, JsonUtility.ToJson(requestObj));
			Debug.Log("Socket is Off");
		}

		//取消订阅指定事件
		public void Cancel(string eventName)
		{
			//				Debug.LogError (requestObj.isOpen);
			if (JsonUtility.ToJson(requestObj) != "")
			{
				requestObj.isOpen = false;
			}
			//			requestObj.isOpen = false;
			Debug.Log(requestObj);
			//			MySocket.Emit (eventName, JsonUtility.ToJson (requestObj));
			//			MySocket.Off (eventName);
			//			MySocket.Disconnect ();
		}

		#endregion


	}
}

