using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HopeRun;

public class MainSceneMgr : MonoBehaviour
{
	public static MainSceneMgr MainMgr;
	public static bool LazyQuit = false;
	private InputField ipInputField;

	private Transform canvas;
	private TDButtonItem[] btns;
	private Text tiptext;

	bool isIpFieldShow = true;

	//    void OnGUI()
	//    {
	//        GUILayout.Label(SystemInfo.deviceName);
	//        GUILayout.Label(SystemInfo.deviceModel);
	//        GUILayout.Label("Net State: " + Application.internetReachability);
	//    }

	// Use this for initialization
	IEnumerator Start()
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
		SceneManager.activeSceneChanged += ((first, second) =>
		{
			//记录上一个场景号
			Debug.LogError(first.name + " --->> " + second.name);
		});

		GyroInput.inst.AddNodListener(CorrecteUIPostion);
		yield return new WaitForSeconds(0.2f);

		GlobalManager.LoadScene("Welcome");
	}

	void CorrecteUIPostion()
	{
		GyroInput.CorrecteUIPostion(canvas);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		else if (LazyQuit)
		{
			StartCoroutine(CalLazyQuit());
		}
		else if (Input.GetKeyDown(KeyCode.X))
		{
			FindObjectOfType<WelcomeMgr>().FirstLoadScene("Inspection");
		}
		else if (Input.GetKeyDown(KeyCode.Z))
		{
			FindObjectOfType<WelcomeMgr>().FirstLoadScene("Tank");
		}
		else if (Input.GetKeyDown(KeyCode.J))
		{
			GlobalManager.LoadScene("Welcome");
		}
	}

	IEnumerator CalLazyQuit()
	{
		LazyQuit = false;
		yield return new WaitForSeconds(5f);
		Debug.Log("Quit!");
		Application.Quit();
	}
}
