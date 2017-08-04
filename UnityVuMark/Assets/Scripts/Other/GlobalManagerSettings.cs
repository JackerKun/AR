using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HopeRun
{
	public enum ValveState
	{
		ON,
		OFF}

	;

	public class GlobalManager:MonoBehaviour
	{
		//嘉琛
		//		public static  string apiURL = "http://192.168.120.207:1234/socket.io/";
		//服务器
		public static  string apiURL = "http://192.168.1.22:1234/socket.io/";
		//		public static  string apiURL = "http://192.168.120.179:1234/socket.io/";
		//		public static  string apiURL = "http://localhost:1234/socket.io/";

		public static TankObj CURRENT_DRUM;

		public static TankObj  InitDrumPanel (string id, float curH, float totalH)
		{
			Transform canvasRoot = GameObject.Find ("TankTarget/Root").transform;
//			TankObj drum = Instantiate<TankObj> ((Resources.Load ("Prefabs/TankPanel") as GameObject).GetComponent<TankObj> (), canvasRoot);
			TankObj drum = Instantiate<TankObj> ((Resources.Load ("Prefabs/TankPanelNoBG") as GameObject).GetComponent<TankObj> (), canvasRoot);
			drum.InitParam (id, curH, totalH);
			return drum;
		}

		public static void Deposit (Container container)
		{
			if (container != null) {
				container.Deposit ();
			}
		}
	}
}