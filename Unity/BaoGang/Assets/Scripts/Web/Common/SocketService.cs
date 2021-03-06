﻿using System;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Events;
using AR.Configs;
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
            get
            {
                if (_socket != null) return _socket;
                var options = new SocketOptions
                {
                    AutoConnect = false
//                    ReconnectionDelay = TimeSpan.FromMilliseconds(3000)
                };
                socketManagerRef = new SocketManager(GlobalManager.IP, options);
                _socket = socketManagerRef.Socket;
                return _socket;
            }
        }


        TargetDeviceRequest requestObj;


        public void CloseServer()
        {
            mySocket.Off();
            mySocket.Manager.Close();
            _socket = null;
            Debug.LogError("CloseServer");
        }

        //public void InitScene(string sceneName, SocketIOCallback callback)
        //{

        //    mySocket.Manager.Handshake.OnError += (e1, e2) =>
        //    {
        //        Debug.LogError("HandshakeError" + e2);
        //    };
        //    mySocket.Manager.Handshake.OnReceived += (A) =>
        //    {
        //        UIManager.ShowMessage("已成功连接服务");
        //        Debug.Log("Handshake OnReceived");
        //    };
        //    LocalDeviceRequest requestDevice = new LocalDeviceRequest(sceneName);

        //    mySocket.Emit(EventConfig.ONLINE, JsonUtility.ToJson(requestDevice));
        //    mySocket.On(EventConfig.AR_ONLINE, callback);
        //    Debug.Log("register socket..");
        //}

        ////加入监听
        //public void AddListener(string response, SocketIOCallback callback)
        //{
        //    mySocket.On(response, callback);
        //    Debug.Log("add listener.." + response);
        //}

        //订阅指定事件，请求目标物体动态数据（液位高度等实时变化的数据）
        ////订阅指定事件
        //public void Subscribe(string eventName, SocketIOCallback callback)
        //{
        //    //如果初次连接socket
        //    requestObj = new TargetDeviceRequest(true, deviceID, GlobalManager.CURRENT_TANKID);
        //    Debug.Log(requestObj);
        //    mySocket.Emit(EventConfig.TANK, JsonUtility.ToJson(requestObj));
        //    mySocket.On(eventName, callback);
        //}

        ////订阅指定事件，取消目标物体动态数据请求
        //public void DisSubscribe(string eventName)
        //{
        //    //如果初次连接socket
        //    requestObj = new TargetDeviceRequest(false, deviceID, GlobalManager.CURRENT_TANKID);
        //    Debug.Log(requestObj);
        //    mySocket.Emit(EventConfig.TANK, JsonUtility.ToJson(requestObj));
        //    Debug.Log("Socket is Off");
        //}
//        public void Register(string request, string response, SocketIOCallback callback)
//        {
//            requestObj = new TargetDeviceRequest(true, deviceID, "1");
//            Request(request, JsonUtility.ToJson(requestObj));
//            mySocket.On(response, callback);
//            Debug.Log(request + " --- " + response + " register socket..");
//
//        }


        #region 新添加的东西
        public void Open()
        {
            socketManagerRef.Open();
        }

        public void AddConnectListener(KCallback callback)
        {
            mySocket.On(SocketIOEventTypes.Connect, (a1, a2, a3) =>
            {
                Debug.Log("------------------->建立连接" + a2.Payload);
                if (callback != null) callback();
            });
        }

        public void AddDisconnectListener(KCallback callback)
        {
            mySocket.On(SocketIOEventTypes.Disconnect, (a1, a2, a3) =>
            {
                Debug.Log("------------------->断开连接" + a2.Payload);
                if (callback != null) callback();
            });
//            mySocket.On(SocketIOEventTypes.Error, (a1, a2, a3) =>
//            {
//                Debug.Log("------------------->出错了" + a2.Payload);
//                if (callback != null) callback();
//            });
//            mySocket.On(SocketIOEventTypes.Unknown, (a1, a2, a3) =>
//            {
//                Debug.Log("------------------->未知反馈" + a2.Payload);
//                if (callback != null) callback();
//            });
        }

        
        /// <summary>
        /// 添加监听回调
        /// </summary>
        /// <param name="response">回调事件名</param>
        /// <param name="callback">回调方法</param>
        public void AddListener(string response, KNodeCallback callback)
        {
            mySocket.On(response, (socket, packet, arg) =>
            {
                Debug.Log(packet.Payload);
                JSONNode jRoot = JSON.Parse(packet.Payload)[1];

                if (jRoot["status"] != "error")
                {
                    //TODO; 数据的转换
                    JSONNode data = jRoot["data"];
                    callback(data, jRoot);
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

        public void RemoveListener(string response)
        {
            mySocket.Off(response);
        }

        /// <summary>
        /// 获取动态数据
        /// </summary>
        /// <param name="request">请求命令</param>
        /// <param name="eventName">回调事件名</param>
        /// <param name="callback">回调方法</param>
        public void StartGetData(string request, string eventName, KNodeCallback callback)
        {
            requestObj = new TargetDeviceRequest(true, GlobalManager.DeviceID, GlobalManager.CURRENT_TANKID);
            Request(request, JsonUtility.ToJson(requestObj));
            AddListener(eventName, callback);

        }
        /// <summary>
        /// 取消获取动态数据
        /// </summary>
        /// <param name="request">请求命令</param>
        public void CancleGetData(string request)
        {
            requestObj = new TargetDeviceRequest(false, GlobalManager.DeviceID, GlobalManager.CURRENT_TANKID);
            Request(request, JsonUtility.ToJson(requestObj));
        }
        #endregion
    }


    public delegate void KNodeCallback(params JSONNode[] node);

    public delegate void KCallback();

    public delegate void KErrorCallback();


}

