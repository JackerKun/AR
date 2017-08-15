using System.Collections;
using System.Collections.Generic;
using UnityEngine.Collections;
using UnityEngine;

namespace HopeRun
{
	public enum ValveState
	{
		ON,
		OFF}

	;

	public enum PipeStatus
	{
		Normal,
		Abnormal}

	;

	public class GlobalManager:MonoBehaviour
	{
		//嘉琛
		//		public static  string apiURL = "http://192.168.120.207:1234/socket.io/";
		//服务器
		//		public static  string apiURL = "http://192.168.110.43:1234/socket.io/";
		//		public static  string apiURL = "http://192.168.110.43:1234/socket.io/";
		public static  string apiURL = "http://192.168.0.111:1234/socket.io/";
		//		public static  string apiURL = "http://192.168.1.22:1234/socket.io/";
		//		public static  string apiURL = "http://192.168.110.43:1234/socket.io/";
		//		public static  string apiURL = "http://192.168.120.179:1234/socket.io/";
		//		public static  string apiURL = "http://localhost:1234/socket.io/";

		public static TankObj CURRENT_DRUM;
		public static Transform CURRENT_TAG_CANVAS;

		public static TankObj  InitDrumPanel (string id)
		{
			Transform canvasRoot = GameObject.Find ("TankTarget/Root").transform;
//			TankObj drum = Instantiate<TankObj> ((Resources.Load ("Prefabs/TankPanel") as GameObject).GetComponent<TankObj> (), canvasRoot);
			TankObj drum = Instantiate<TankObj> ((Resources.Load ("Prefabs/TankPanelNoBG") as GameObject).GetComponent<TankObj> (), canvasRoot);
			drum.InitParam (id);
			return drum;
		}

		public static Transform InitFloatingPanel (Transform rootTran, List<Transform> imageTran)
		{
			Transform canvasRoot = GameObject.Find ("TagCanvas").transform;
//			Transform tagCanvas = Instantiate (Resources.Load ("Prefabs/TagCanvas") as GameObject, rootTran).transform;
//			tagCanvas.parent = rootTran;
//			tagCanvas.localScale = Vector3.one;
//			Transform canvasRoot = tagCanvas.Find ("Tags").transform;
			Transform tagRoot = new GameObject (rootTran.name + "_root").transform;
			tagRoot.parent = canvasRoot;
			tagRoot.localScale = Vector3.one;
//			tagRoot.localEulerAngles = Vector3.zero;
			tagRoot.localRotation = Quaternion.identity;
			List<FloatingTag> tags = new List<FloatingTag> ();
			for (int i = 0; i < imageTran.Count; i++) {
				FloatingTag tag = Instantiate<FloatingTag> ((Resources.Load ("Prefabs/Tag") as GameObject).GetComponent<FloatingTag> (), tagRoot);
				tag.Init (imageTran [i]);
				tags.Add (tag);
			}
			return tagRoot;
		}

		public static void Deposit (Container container)
		{
			if (container != null) {
				container.Deposit ();
			}
		}

		public static void Deposit (Transform tran)
		{
			if (tran != null) {
				Destroy (tran.gameObject);
			}
		}
	}
}