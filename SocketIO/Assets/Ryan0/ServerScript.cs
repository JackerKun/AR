/// <summary>
/// Server Script.
/// Created By 蓝鸥3G 2014.08.23
/// </summary>
/// 
/// 
using UnityEngine;
using System.Collections;

public class ServerScript : MonoBehaviour
{
	 
	private string receive_str;
	LOSocket server;
	// Use this for initialization
	void Start ()
	{
		server = LOSocket.GetSocket (LOSocket.LOSocketType.SERVER);
		//初始化服务器
		server.InitServer ((string content) => {
			receive_str = content;
		});
	}

	void OnGUI ()
	{
		if (receive_str != null) {
			GUILayout.Label (receive_str);
		}
	}
}