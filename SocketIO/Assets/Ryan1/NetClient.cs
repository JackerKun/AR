using UnityEngine;
using UnityEngine.Networking;

public class NetClient:MonoBehaviour
{
	NetworkClient myClient;

	public void OnConnected (NetworkMessage netMsg)
	{
		Debug.Log ("Connected to server");
	}

	public void OnDisconnected (NetworkMessage netMsg)
	{
		Debug.Log ("Disconnected from server");
	}

	public void OnError (NetworkMessage netMsg)
	{
		//		TMsg errorMsg = reader.ReadMessage<TMsg> ();
		//		Debug.Log ("Error connecting with code " + errorMsg.errorCode);
		Debug.Log ("Error connecting with code ");
	}

	public void Start ()
	{
		myClient = new NetworkClient ();

		myClient.RegisterHandler (MsgType.Connect, OnConnected);
		myClient.RegisterHandler (MsgType.Disconnect, OnDisconnected);
		myClient.RegisterHandler (MsgType.Error, OnError);

//		myClient.Connect ("http://192.168.1.22:3000/socket.io/", 3000);
//		myClient.Connect ("http://127.0.0.1:3000/socket.io/", 3000);
	}
}