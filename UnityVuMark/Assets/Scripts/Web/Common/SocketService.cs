using System;
using BestHTTP;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Events;
using SimpleJSON;
using System.Collections.Generic;
using AR.Configs;
using HopeRun.Model;
using UnityEngine;

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

		DeviceRequest request;
		//订阅指定事件
		public void Subscribe (String eventName, string targetID, SocketIOCallback callback)
		{
			//如果初次连接socket
			if (mySocket == null) {
				socketManagerRef = new SocketManager (new Uri (HopeRun.GlobalManager.apiURL), options);
				Debug.Log ("set connection options");
				socketManagerRef.Options.AutoConnect = true;
				mySocket = socketManagerRef.Socket;

//				socketManagerRef.Open ();
			}
			Debug.Log ("Socket is connected ? " + mySocket.IsOpen);
//			request = new DeviceRequest (true, deviceID, targetID);
			request = new DeviceRequest (true, "2", "1");
			mySocket.Emit (EventConfig.REQUEST_SOCKET, JsonUtility.ToJson (request));
			mySocket.On (eventName, callback);// + "_" + targetID
			Debug.Log ("Socket is connected ? " + mySocket.IsOpen);
		}

		//取消订阅指定事件
		public void Cancel (String eventName, string targetID)
		{
			request.isOpen = false;
			mySocket.Emit (EventConfig.REQUEST_SOCKET, JsonUtility.ToJson (request));
			mySocket.Off ();// + "_" + targetID
//			mySocket.Disconnect ();
		}

		#endregion

		#region PRIVATE_METHODS

		private void setDefaultSocketOptions ()
		{
			//Options
			options = new SocketOptions ();
//			options.ReconnectionAttempts = 3;
			options.AutoConnect = true;
			options.ReconnectionDelay = TimeSpan.FromMilliseconds (1000);
			Debug.LogError ("Set Socket Options Finished!");
		}

		#endregion

	}
}

