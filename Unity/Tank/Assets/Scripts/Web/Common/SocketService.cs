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
		#region PUBLIC_VARS

		//Set Namespace For Current
		public string currentChannel;

		//SocketManager Reference
		public SocketManager socketManagerRef;

		//Socket Reference
		public Socket mySocket;

		SocketOptions options;

		bool isConnectSucceed = false;
		TargetDeviceRequest requestObj;

		#endregion

		#region PRIVATE_VARS

		private string deviceID = SystemInfo.deviceUniqueIdentifier;

		#endregion


		#region PUBLIC_METHODS

		public SocketService ()
		{
			setDefaultSocketOptions ();
		}

		public SocketService (SocketOptions options)
		{
			this.options = options;
		}

		public void InitScene2 (string sceneID, SocketIOCallback callback)
		{
			#region 新建socket
			TryConnect ();
			//连接失败
			if (isConnectSucceed) {
				LocalDeviceRequest requestObj = new LocalDeviceRequest (sceneID);
				mySocket.Emit (EventConfig.ONLINE, JsonUtility.ToJson (requestObj));
				mySocket.On (EventConfig.AR_ONLINE, callback);
				Debug.Log ("register socket..");
			}
			#endregion
		}

		public void Dispose ()
		{
			Debug.LogError ("dispose listener..");
			if (mySocket != null) {
				mySocket.Off (EventConfig.AR_ONLINE, (e0, e1, e2) => {
					Debug.Log ("dispose listener..");
				});
			}
		}

		void TryConnect ()
		{
			//如果初次连接socket
			if (mySocket == null) {
				socketManagerRef = new SocketManager (GlobalManager.IP, options);
				Debug.Log ("set connection options");
				socketManagerRef.Options.AutoConnect = true;
			}
			mySocket = socketManagerRef.Socket;
			mySocket.Manager.Handshake.OnError += (e1, e2) => {
				//				Reconnect ();
			};
			mySocket.Manager.Handshake.OnReceived += (A) => {
				isConnectSucceed = true;
				Debug.Log ("connect succeed!");
			};
			mySocket.Manager.Open ();
			Debug.Log (mySocket.IsOpen);
		}

		public static void Reconnect ()
		{
			//			Debug.LogError ("Hello World!");
			//			//			Debug.Log (mySocket.IsOpen);
			//			//			Debug.Log (mySocket.IsOpen);
			//			float t = Time.realtimeSinceStartup + 15f;
			//			while (Time.realtimeSinceStartup <= t) {
			//				//wait
			//			}
			//			Debug.LogError (t);
			////			Thread.Sleep (1000);
			////			if (!mySocket.IsOpen) {
			//////				mySocket.Manager.Open ();
			////				Reconnect ();
			////			}
			Debug.LogError ("Reconnect");
			//			mySocket.Manager.Open ();
		}

		public void InitScene (string sceneID, SocketIOCallback callback)
		{

			//			BestHTTP.SocketIO.Socket
			//如果初次连接socket
			if (mySocket == null) {
				socketManagerRef = new SocketManager (GlobalManager.IP, options);
				Debug.Log ("set connection options");
			}
			mySocket = socketManagerRef.Socket;
			mySocket.Manager.Handshake.OnError += (e1, e2) => {
				Debug.LogError ("HandshakeError" + e2);
			};
			mySocket.Manager.Handshake.OnReceived += (A) => {
				UIManager.ShowMessage ("已成功连接服务");
				Debug.Log ("Handshake OnReceived");
			};
			//			requestObj = new DeviceRequest (true, deviceID, "");

			LocalDeviceRequest requestObj = new LocalDeviceRequest (sceneID);
			mySocket.Emit (EventConfig.ONLINE, JsonUtility.ToJson (requestObj));
			mySocket.On (EventConfig.AR_ONLINE, callback);
			Debug.Log ("register socket..");
		}

		//加入监听
		public void AddListener (string response, SocketIOCallback callback)
		{
			//如果初次连接socket
			if (mySocket == null) {
				socketManagerRef = new SocketManager (GlobalManager.IP, options);
				Debug.Log ("set connection options");
				socketManagerRef.Options.AutoConnect = true;
			}
			mySocket = socketManagerRef.Socket;
			mySocket.On (response, callback);
			Debug.Log ("add listener.." + response);
		}

		public void Register (string request, string response, SocketIOCallback callback)
		{
			//如果初次连接socket
			if (mySocket == null) {
				socketManagerRef = new SocketManager (GlobalManager.IP, options);
				Debug.Log ("set connection options");
				socketManagerRef.Options.AutoConnect = true;
				mySocket = socketManagerRef.Socket;
			}
			//			requestObj = new DeviceRequest (true, deviceID, "");
			//			requestObj = new TargetDeviceRequest (true, deviceID, targetID);
			requestObj = new TargetDeviceRequest (true, deviceID, "1");
			mySocket.Emit (request, JsonUtility.ToJson (requestObj));
			mySocket.On (response, callback);
			Debug.Log (request + " --- " + response + " register socket..");
		}

		public void ServerInfo (string response, SocketIOCallback callback)
		{
			//如果初次连接socket
			if (mySocket == null) {
				socketManagerRef = new SocketManager (GlobalManager.IP, options);
				Debug.Log ("set connection options");
				socketManagerRef.Options.AutoConnect = true;
			}
			mySocket = socketManagerRef.Socket;
			mySocket.On (response, callback);
			Debug.Log ("register socket..");
		}

		//订阅指定事件
		public void Subscribe (String eventName, SocketIOCallback callback)
		{
			//如果初次连接socket
			if (mySocket == null) {
				socketManagerRef = new SocketManager (GlobalManager.IP, options);
				Debug.Log ("set connection options");
				socketManagerRef.Options.AutoConnect = true;
			}
			mySocket = socketManagerRef.Socket;
			requestObj = new TargetDeviceRequest (true, deviceID, GlobalManager.CURRENT_TANKID);
			Debug.Log (requestObj);
			mySocket.Emit (EventConfig.TANK, JsonUtility.ToJson (requestObj));
			mySocket.On (eventName, callback);
		}

		//订阅指定事件
		public void DisSubscribe (String eventName)
		{
			//如果初次连接socket
			if (mySocket == null) {
				socketManagerRef = new SocketManager (GlobalManager.IP, options);
				Debug.Log ("set connection options");
				socketManagerRef.Options.AutoConnect = true;
			}
			mySocket = socketManagerRef.Socket;
			requestObj = new TargetDeviceRequest (false, deviceID, GlobalManager.CURRENT_TANKID);
			Debug.Log (requestObj);
			mySocket.Emit (EventConfig.TANK, JsonUtility.ToJson (requestObj));
			Debug.Log ("Socket is Off");
		}

		//取消订阅指定事件
		public void Cancel (string eventName)
		{
			//				Debug.LogError (requestObj.isOpen);
			if (JsonUtility.ToJson (requestObj) != "") {
				requestObj.isOpen = false;
			}
			//			requestObj.isOpen = false;
			Debug.Log (requestObj);
			//			mySocket.Emit (eventName, JsonUtility.ToJson (requestObj));
			//			mySocket.Off (eventName);
			//			mySocket.Disconnect ();
		}

		#endregion

		#region PRIVATE_METHODS

		private void setDefaultSocketOptions ()
		{
			//Options
			options = new SocketOptions ();
			options.AutoConnect = true;
			options.Reconnection = true;
			options.ReconnectionDelay = TimeSpan.FromMilliseconds (100);
			Debug.Log ("Set Socket Options Finished!");
		}

		#endregion

	}
}

