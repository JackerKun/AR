using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Collections;
using UnityEngine;
using AR.Common;
using AR.Model;
using UnityEngine.SceneManagement;

namespace HopeRun
{
	public enum ValveState
	{
		ON,
		OFF
	}

	;

	public enum PipeStatus
	{
		Normal,
		Abnormal
	}

	;

	public class GlobalManager : MonoBehaviour
	{
		public static string DeviceID = SystemInfo.deviceUniqueIdentifier;
		public static string DeviceName = SystemInfo.deviceModel;
        //public static string CURRENT_PHOTO_BASE64;
		public static string LAST_LOADED_SCENE;
		public static string CURRENT_TANKID;
		public static string CURRENT_SCENE_SERVICE;
		public static bool IS_WORKFLOW = false;
		//服务器
		//		public static string apiURL = "http://192.168.1.22:1234/socket.io/";
		public static string PORTAL;
		public static string IP
		{
			set
			{
				PlayerPrefs.SetString("IP", value);
			}
			get
			{
				string str = PlayerPrefs.GetString("IP");
				if (String.IsNullOrEmpty(str))
				{
					str = "192.168.110.82";
				}
				return str;
			}
		}
		//现法
		//		public static  string apiURL = "http://192.168.110.89:1234/socket.io/";
		// 客户
		//		public static  string apiURL = "http://192.168.0.111:1234/socket.io/";


		//		public static  string apiURL = "http://192.168.110.43:1234/socket.io/";
		//		public static  string apiURL = "http://192.168.120.179:1234/socket.io/";
		//		public static  string apiURL = "http://localhost:1234/socket.io/";

		public static Transform CURRENT_TAG_CANVAS;
        
		public static Transform InitFloatingPanel(Transform rootTran, List<Transform> imageTran)
		{
			Transform canvasRoot = GameObject.Find("TagCanvas").transform;
			//			Transform tagCanvas = Instantiate (Resources.Load ("Prefabs/TagCanvas") as GameObject, rootTran).transform;
			//			tagCanvas.parent = rootTran;
			//			tagCanvas.localScale = Vector3.one;
			//			Transform canvasRoot = tagCanvas.Find ("Tags").transform;
			Transform tagRoot = new GameObject(rootTran.name + "_root").transform;
			tagRoot.parent = canvasRoot;
			tagRoot.localScale = Vector3.one;
			//			tagRoot.localEulerAngles = Vector3.zero;
			tagRoot.localRotation = Quaternion.identity;
			var tags = new List<FloatingTag>();
			for (int i = 0; i < imageTran.Count; i++)
			{
				FloatingTag tag = Instantiate(((GameObject)Resources.Load("Prefabs/Tag")).GetComponent<FloatingTag>(), tagRoot);
				tag.Init(imageTran[i]);
				tags.Add(tag);
			}
			return tagRoot;
		}


	    public static void LoadScene(string sceneName)
        {
            LAST_LOADED_SCENE = SceneManager.GetActiveScene().name;
            //载入场景
            SceneManager.LoadScene(sceneName);
        }


	    public static Color GetWarnColor(float percent)
		{
			float r = Mathf.Clamp01((percent - 1f / 3f) * 3f);
			float g = Mathf.Max(0, (1f - 3f * percent));
			float b = 1f - Mathf.Max(0, (percent - 2f / 3f) * 3f);
			return new Color(r, g, b);
		}


		public static void Deposit(Transform tran)
		{
			if (tran != null)
			{
				Destroy(tran.gameObject);
			}
		}
	}
}