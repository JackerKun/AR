using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HopeRun;

public class WelcomeMgr : MonoBehaviour
{
	private InputField ipInputField;

	private Transform canvas;
	private TDButtonItem[] btns;
	private Text tiptext;

	bool isIpFieldShow = true;

	// Use this for initialization
	IEnumerator Start()
	{
		Init();
		yield return new WaitForSeconds(0.2f);
		GyroInput.CorrecteUIPostion(canvas);
	}

	void Init()
	{
		btns = new TDButtonItem[3];
		canvas = GameObject.Find("Canvas").transform;
		btns[0] = canvas.Find("btns/btn_jiayao").GetComponent<TDButtonItem>();
		btns[1] = canvas.Find("btns/btn_xunjian").GetComponent<TDButtonItem>();
		btns[2] = canvas.Find("btns/btn_guandao").GetComponent<TDButtonItem>();
		tiptext = canvas.Find("textbg/name").GetComponent<Text>();
		ipInputField = GameObject.Find("SettingCanvas/IPInputField").GetComponent<InputField>();
		Switch();
		ipInputField.text = GlobalManager.IP;
		btns[0].OnClick += () => { FirstLoadScene("Tank"); };
		btns[1].OnClick += () => { FirstLoadScene("Inspection"); };
		btns[0].OnOver += () => { tiptext.text = "加药"; };
		btns[1].OnOver += () => { tiptext.text = "巡检"; };

		btns[0].OnOut += () => { tiptext.text = "请选择场景"; };
		btns[1].OnOut += () => { tiptext.text = "请选择场景"; };
	}


	#region 显示输入框操作
	private bool isready = true;
	public void Switch()
	{
		if (!isready) return;
		isready = false;
		isIpFieldShow = !isIpFieldShow;
		if (isIpFieldShow)
		{
			ipInputField.transform.DOMoveX(ipInputField.transform.position.x - 600, 0.2f).OnComplete(() => { isready = true; });
		}
		else
		{
			ipInputField.transform.DOMoveX(ipInputField.transform.position.x + 600, 0.2f).OnComplete(() => { isready = true; });
		}
	}
	#endregion

	void CorrecteUIPostion()
	{
		GyroInput.CorrecteUIPostion(canvas);
	}

	public void FirstLoadScene(string sceneName)
	{

		GlobalManager.CURRENT_SCENE_SERVICE = sceneName;
		//释放场景资源
		GlobalManager.IP = ipInputField.text;
		if (sceneName == "Tank")
		{
			GlobalManager.PORTAL = ":1234";
			WebManager.Instance.Init(TankSocketService.Instance);
		}
		else if (sceneName == "Inspection")
		{
			GlobalManager.PORTAL = ":1235";
			WebManager.Instance.Init(InspectionSocketService.Instance);
		}
		Debug.Log(GlobalManager.IP + GlobalManager.PORTAL);
		GlobalManager.LoadScene(sceneName);
	}
}
