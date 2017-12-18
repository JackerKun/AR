using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HopeRun;

public class MainSceneMgr : MonoBehaviour
{
	static Image screenEdge;
	public static MainSceneMgr MainMgr;
	public static bool LazyQuit = false;
	InputField ipInputField;

	bool isIpFieldShow = true;

	void OnGUI()
	{
		GUILayout.Label(SystemInfo.deviceName);
		GUILayout.Label(SystemInfo.deviceModel);
		GUILayout.Label("Net State: " + Application.internetReachability);
	}
	// Use this for initialization
	void Start()
	{
		//屏幕常量
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Application.runInBackground = true;
		if (MainMgr == null)
		{
			MainMgr = this;
			DontDestroyOnLoad(GameObject.Find("MainCanvas"));
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(GameObject.Find("MainCanvas"));
			Destroy(gameObject);
		}

		Init();
		SceneManager.activeSceneChanged += ((first, second) =>
		{
			//记录上一个场景号
			Debug.LogError(first.name + " --->> " + second.name);
		});
	}

	void Init()
	{
		screenEdge = GameObject.Find("MainCanvas/other/edge").GetComponent<Image>();
		ipInputField = GameObject.Find("SettingCanvas/IPInputField").GetComponent<InputField>();
		Switch();
		ipInputField.text = GlobalManager.IP;
		//		WebManager.Init ();
		//
		//		WebErrorProccess.Init ();
		//		StartCoroutine (RefreshSocket ());
	}

	public void Switch()
	{
		isIpFieldShow = !isIpFieldShow;
		ipInputField.gameObject.SetActive(isIpFieldShow);
	}

	IEnumerator RefreshSocket()
	{
		while (true)
		{
			WebManager.IsConnect = false;
			yield return new WaitForSeconds(3);
			//断开了连接，重连
			if (WebManager.IsConnect)
			{
				ChangeScreenEdgeColor(Color.white);
			}
			else
			{
				Debug.Log(WebManager.IsConnect);
				ShowError(WebManager.IsConnect);
			}
		}
	}

	public void FirstLoadScene(string sceneName)
	{
		//释放场景资源
		GlobalManager.IP = ipInputField.text;
		if (sceneName == "Tank")
		{
			GlobalManager.PORTAL = ":1234";
		}
		else if (sceneName == "Inspection")
		{
			GlobalManager.PORTAL = ":1235";
		}
		Debug.Log(GlobalManager.IP + GlobalManager.PORTAL);

		WebManager.Init(sceneName.ToLower());

		WebErrorProccess.Init();
		StartCoroutine(RefreshSocket());

		LoadScene(sceneName);
	}

	public void LoadScene(string sceneName)
	{
		GlobalManager.LAST_LOADED_SCENE = SceneManager.GetActiveScene().name;
		//载入场景
		SceneManager.LoadScene(sceneName);
	}

	void ChangeScreenEdgeColor(Color color)
	{
		if (screenEdge.color != color)
		{
			screenEdge.color = color;
		}
	}

	void ShowError(bool connect)
	{
		UIManager.ShowErrorMessage("网络已断开！");
		ChangeScreenEdgeColor(Color.red);
	}

	void Update()
	{
		//		if (Input.GetKeyDown (KeyCode.Alpha0)) {
		//			LoadScene ("Tank");
		//		} else if (Input.GetKeyDown (KeyCode.Alpha1)) {
		//			LoadScene ("WorkFlow");
		//		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		else if (LazyQuit)
		{
			StartCoroutine(CalLazyQuit());
		}
	}

	IEnumerator CalLazyQuit()
	{
		yield return new WaitForSeconds(5f);
		Debug.Log("Quit!");
		LazyQuit = false;
		Application.Quit();
	}
}
