using UnityEngine;
using System;
using BestHTTP;
using BestHTTP.SocketIO;
using SimpleJSON;
using System.Collections.Generic;
using BestHTTP.JSON;
using BestHTTP.SocketIO.Events;
using AR.Configs;

public class MySocketManager : MonoBehaviour
{
	#region Fields

	/// <summary>
	/// The Socket.IO manager instance.
	/// </summary>
	private SocketManager Manager;

	#endregion

	#region Unity Events

	void Start ()
	{

		SocketOptions options = new SocketOptions ();
		options.AutoConnect = true;
		Manager = new SocketManager (new Uri ("ws://192.168.1.22:1234/socket.io/"), options);
		Manager.Socket.Emit (EventConfig.REQUEST_SOCKET, "");
		Manager.Socket.On (EventConfig.RESPONSE_BUCKET, OnLogin);
		Manager.Options.AutoConnect = true;
		Manager.Open ();

	}

	void OnDestroy ()
	{
		// Leaving this sample, close the socket
		Manager.Close ();
	}


	#endregion

	void OnLogin (Socket socket, Packet packet, params object[] args)
	{
		Debug.Log ("on RESPONSE_BUCKET is ok");

	}
}