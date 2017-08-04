/// <summary>
/// Client Script.
/// Created By 蓝鸥3G 2014.08.23
/// </summary>
 
using UnityEngine;
using System.Collections;

public class ClientScript: MonoBehaviour
{
	string msg = "";
	// Use this for initialization
	 
	LOSocket client;

	void Start ()
	{
		client = LOSocket.GetSocket (LOSocket.LOSocketType.CLIENT);
		client.InitClient ("192.168.1.22:3000/socket.io/", 3000, ((string content) => {
			//收到服务器的回馈信息
		}));
	}

	void OnGUI ()
	{
		msg = GUI.TextField (new Rect (0, 0, 500, 40), msg);
		if (GUI.Button (new Rect (0, 50, 100, 30), "Send")) {
			 
			client.SendMessage (msg);
		}
	}
}