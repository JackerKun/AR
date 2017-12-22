using System;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Events;
using AR.Configs;
using AR.Model;
using HopeRun.Model;
using UnityEngine;
using HopeRun;
using SimpleJSON;

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
	        get {
                if (_socket != null) return _socket;
	            var options = new SocketOptions
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


		TargetDeviceRequest requestObj;

		private string deviceID = SystemInfo.deviceUniqueIdentifier;


		#region PUBLIC_METHODS


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


		public void Reconnect()
		{
			mySocket.Manager.Close();
		    _socket = null;
			Debug.LogError("Reconnect");
		}
		public void Register(string request, string response, SocketIOCallback callback)
		{
			requestObj = new TargetDeviceRequest(true, deviceID, "1");
			mySocket.Emit(request, JsonUtility.ToJson(requestObj));
			mySocket.On(response, callback);
			Debug.Log(request + " --- " + response + " register socket..");
           
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
        
		//订阅指定事件，请求目标物体动态数据（液位高度等实时变化的数据）
		public void Subscribe(string eventName, SocketIOCallback callback)
		{
			//如果初次连接socket
			requestObj = new TargetDeviceRequest(true, deviceID, GlobalManager.CURRENT_TANKID);
			Debug.Log(requestObj);
			mySocket.Emit(EventConfig.TANK, JsonUtility.ToJson(requestObj));
			mySocket.On(eventName, callback);
		}

		//订阅指定事件，取消目标物体动态数据请求
		public void DisSubscribe(string eventName)
		{
			//如果初次连接socket
			requestObj = new TargetDeviceRequest(false, deviceID, GlobalManager.CURRENT_TANKID);
			Debug.Log(requestObj);
			mySocket.Emit(EventConfig.TANK, JsonUtility.ToJson(requestObj));
			Debug.Log("Socket is Off");
		}

	    #endregion




        #region 新添加的东西
        /// <summary>
        /// 请求连接服务器
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="callback">回调方法</param>
        public void ConnectServer(string sceneName, KSuccessCallback callback)
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
            mySocket.On(EventConfig.AR_ONLINE, (socket, packet, arg) =>
            {
                JSONNode jRoot = JSON.Parse(packet.Payload)[1];

                if (jRoot["status"] == "success")
                {
                    Debug.Log(packet.Payload);
                    //TODO; 数据的转换
                    JSONNode data = jRoot["data"];
                    callback(data, jRoot);
                }
                else
                {
                    UIManager.ShowErrorMessage(jRoot["message"]);
                }
            });
            Debug.Log("register socket..");
        }
        /// <summary>
        /// 添加监听回调
        /// </summary>
        /// <param name="response">回调事件名</param>
        /// <param name="callback">回调方法</param>
        public void AddListener(string response, KSuccessCallback callback)
        {
            mySocket.On(response, (socket, packet, arg) =>
            {
                JSONNode jRoot = JSON.Parse(packet.Payload)[1];

                if (jRoot["status"] != "error")
                {
                    Debug.Log(packet.Payload);
                    //TODO; 数据的转换
                    JSONNode data = jRoot["data"];
                    callback(data,jRoot);
                }
                else
                {
                    UIManager.ShowErrorMessage(jRoot["message"]);
                }
            });
        }

	    public void Request(string request, string data)
	    {
	        mySocket.Emit(request, data);
	    }

	    /// <summary>
        /// 获取动态数据
        /// </summary>
        /// <param name="request">请求命令</param>
        /// <param name="eventName">回调事件名</param>
        /// <param name="callback">回调方法</param>
        public void StartGetData(string request, string eventName, KSuccessCallback callback)
        {
            requestObj = new TargetDeviceRequest(true, deviceID, GlobalManager.CURRENT_TANKID);
            mySocket.Emit(request, JsonUtility.ToJson(requestObj));
            mySocket.On(eventName, (socket, packet, arg) =>
            {
                Debug.Log("Connect...");
                JSONNode jRoot = JSON.Parse(packet.Payload)[1];

                if (jRoot["status"] == "success")
                {
                    Debug.Log(packet.Payload);
                    //TODO; 数据的转换
                    JSONNode data = jRoot["data"];
                    callback(data);
                }
                else
                {
                    UIManager.ShowErrorMessage(jRoot["message"]);
                }
            });

	    }
        /// <summary>
        /// 取消获取动态数据
        /// </summary>
        /// <param name="request">请求命令</param>
        public void CancleGetData(string request)
        {
            requestObj = new TargetDeviceRequest(false, deviceID, GlobalManager.CURRENT_TANKID);
            mySocket.Emit(request, JsonUtility.ToJson(requestObj));
        }
        #endregion
    }


    public delegate void KSuccessCallback(params JSONNode[] node);

    public delegate void KErrorCallback();
}

