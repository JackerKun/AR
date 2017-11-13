using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class Client : MonoBehaviour
{
	/// <summary>
	/// 服务器ip地址
	/// </summary>
	//	string ip = "127.0.0.1";
	//	string ip = "192.168.110.147";
	//	string ip = "192.168.1.14";
	string ip = "192.168.43.243";

	/// <summary>
	/// 服务器端口
	/// </summary>
	string port = "8080";
	/// <summary>
	/// 要发送的信息
	/// </summary>
	Vector3 sendPos;
	float sendFOV;
	/// <summary>
	/// 显示信息
	/// </summary>
	public string info = "Start..";
	/// <summary>
	/// 网络客户端
	/// </summary>
	private NetworkClient myClient;
	/// <summary>
	/// 用户信息分类
	/// </summary>
	private const short userMsg = 64;

	public bool IsClient = true;

	void OnGUI ()
	{
		if (IsClient) {
			GUILayout.Label (info);
		} else {
			if (GUI.Button (new Rect (0, 0, Screen.width, Screen.height), "send")) {
				ServerSend ();
			}
		}
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Return)) {
			ServerSend ();
		}
	}

	void Start ()
	{
		if (IsClient) {
			myClient = new NetworkClient ();
			SetupClient ();
		} else {
			SetupServer ();
		}
	}

	/// <summary>
	/// 建立服务器
	/// </summary>
	public void SetupServer ()
	{
		if (!NetworkServer.active) {
			ShowMsg ("setup server");
			ServerRegisterHandler ();
			NetworkServer.Listen (int.Parse (port));

			if (NetworkServer.active) {
				ShowMsg ("Server setup ok.");
			}
		}
	}

	/// <summary>
	/// 建立客户端
	/// </summary>
	public void SetupClient ()
	{
		if (!myClient.isConnected) {
			ShowMsg ("setup client");
			ClientRegisterHandler ();
			myClient.Connect (ip, int.Parse (port));
		}
	}

	/// <summary>
	/// 停止客户端
	/// </summary>
	public void ShutdownClient ()
	{
		if (myClient.isConnected) {
			ClientUnregisterHandler ();
			myClient.Disconnect ();

			//NetworkClient.Shutdown()使用后，无法再次连接。
			//This should be done when a client is no longer going to be used.
			//myClient.Shutdown ();
		}
	}

	/// <summary>
	/// 停止服务器端
	/// </summary>
	public void ShutdownServer ()
	{
		if (NetworkServer.active) {
			ServerUnregisterHandler ();
			NetworkServer.DisconnectAll ();
			NetworkServer.Shutdown ();

			if (!NetworkServer.active) {
				ShowMsg ("shut down server");
			}
		}
	}

	/// <summary>
	/// 客户端连接到服务器事件
	/// </summary>
	/// <param name="netMsg">Net message.</param>
	private void OnClientConnected (NetworkMessage netMsg)
	{
		ShowMsg ("Client connected to server");
	}

	/// <summary>
	///客户端从服务器断开事件
	/// </summary>
	/// <param name="netMsg">Net message.</param>
	private void OnClientDisconnected (NetworkMessage netMsg)
	{
		ShowMsg ("Client disconnected from server");
	}

	/// <summary>
	/// 客户端错误事件
	/// </summary>
	/// <param name="netMsg">Net message.</param>
	private void OnClientError (NetworkMessage netMsg)
	{
		ClientUnregisterHandler ();
		ShowMsg ("Client error");
	}



	/// <summary>
	/// 服务器端有客户端连入事件
	/// </summary>
	/// <param name="netMsg">Net message.</param>
	private void OnServerConnected (NetworkMessage netMsg)
	{
		ShowMsg ("One client connected to server");
	}

	/// <summary>
	/// 服务器端有客户端断开事件
	/// </summary>
	/// <param name="netMsg">Net message.</param>
	private void OnServerDisconnected (NetworkMessage netMsg)
	{
		ShowMsg ("One client connected from server");
	}

	/// <summary>
	/// 服务器端错误事件
	/// </summary>
	/// <param name="netMsg">Net message.</param>
	private void OnServerError (NetworkMessage netMsg)
	{
		ServerUnregisterHandler ();
		ShowMsg ("Server error");
	}

	/// <summary>
	/// 显示信息
	/// </summary>
	/// <param name="Msg">Message.</param>
	private void ShowMsg (string Msg)
	{
		info = Msg + "\n\r" + info;
		//Debug.Log (Msg);
	}

	/// <summary>
	/// 客户端向服务器端发送信息
	/// </summary>
	public void ClientSend ()
	{
		if (myClient.isConnected) {
			UserMsg um = new UserMsg ();
			um.message = sendPos;
			um.sendFOV = sendFOV;
			if (myClient.Send (userMsg, um)) {
				ShowMsg ("Client send:" + sendPos);
			}
		}
	}

	/// <summary>
	/// 客户端接收到服务器端信息事件
	/// </summary>
	/// <param name="netMsg">Net message.</param>
	private void ClientGet (NetworkMessage netMsg)
	{
		UserMsg Msg = netMsg.ReadMessage<UserMsg> ();
		Camera.main.transform.localPosition = Msg.message;
		Camera.main.fieldOfView = Msg.sendFOV;
		ShowMsg ("Client get:" + Msg.message);
	}

	/// <summary>
	/// 服务器端向所有客户端发送信息
	/// </summary>
	public void ServerSend ()
	{
		if (NetworkServer.active) {
			UserMsg um = new UserMsg ();
			sendPos = GetComponent<Camera> ().transform.localPosition;
			sendFOV = GetComponent<Camera> ().fieldOfView;
			um.message = sendPos;
			um.sendFOV = sendFOV;
			if (NetworkServer.SendToAll (userMsg, um)) {
				ShowMsg ("Server send:" + sendPos);
			}
		}
	}

	/// <summary>
	/// 服务器端收到信息事件
	/// </summary>
	/// <param name="netMsg">Net message.</param>
	private void ServerGet (NetworkMessage netMsg)
	{
		UserMsg Msg = netMsg.ReadMessage<UserMsg> ();
		ShowMsg ("Server get:" + Msg.message);
	}

	/// <summary>
	/// 服务器端注册事件
	/// </summary>
	private void ServerRegisterHandler ()
	{
		NetworkServer.RegisterHandler (MsgType.Connect, OnServerConnected);
		NetworkServer.RegisterHandler (MsgType.Disconnect, OnServerDisconnected);
		NetworkServer.RegisterHandler (MsgType.Error, OnServerError);
		NetworkServer.RegisterHandler (userMsg, ServerGet);
	}

	/// <summary>
	/// 客户端注册事件
	/// </summary>
	private void ClientRegisterHandler ()
	{
		myClient.RegisterHandler (MsgType.Connect, OnClientConnected); 
		myClient.RegisterHandler (MsgType.Disconnect, OnClientDisconnected);
		myClient.RegisterHandler (MsgType.Error, OnClientError);
		myClient.RegisterHandler (userMsg, ClientGet);
	}

	/// <summary>
	/// 客户端注销事件
	/// </summary>
	private void ClientUnregisterHandler ()
	{
		myClient.UnregisterHandler (MsgType.Connect);
		myClient.UnregisterHandler (MsgType.Disconnect);
		myClient.UnregisterHandler (MsgType.Error);
		myClient.UnregisterHandler (userMsg);
	}

	/// <summary>
	/// 服务器端注销事件
	/// </summary>
	private void ServerUnregisterHandler ()
	{
		NetworkServer.UnregisterHandler (MsgType.Connect);
		NetworkServer.UnregisterHandler (MsgType.Disconnect);
		NetworkServer.UnregisterHandler (MsgType.Error);
		NetworkServer.UnregisterHandler (userMsg);
	}
}
