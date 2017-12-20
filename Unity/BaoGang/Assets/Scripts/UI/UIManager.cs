using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HopeRun;

public class UIManager : MonoBehaviour
{
	private static Text MessageTxt;
    private static Image screenEdge;


    private static MessageDialog dialog;
    void Start()
    {
        screenEdge = GameObject.Find("MainCanvas/other/edge").GetComponent<Image>();
		MessageTxt = GameObject.Find("MainCanvas/StayMessage").GetComponent<Text>();
		//		initFov = mainCamera.fieldOfView;
	}


	public static void ShowStayMessage(string strMsg)
	{
		MessageTxt.text = strMsg;
	}


	public static void ShowErrorMessage(string str)
	{
		if (dialog == null)
		{
			dialog = Instantiate(Resources.Load<MessageDialog>("Prefabs/ErrorMessage"), GameObject.Find("MainCanvas").transform);
			dialog.InitText(str);
		}
	}

	public static void ShowMessage(string str)
	{
		if (dialog == null)
		{
			dialog = Instantiate(Resources.Load<MessageDialog>("Prefabs/Message"), GameObject.Find("MainCanvas").transform);
			dialog.InitText(str);
		}
	}

    public static void ChangeScreenEdgeColor(Color color)
    {
        if (screenEdge.color != color)
        {
            screenEdge.color = color;
        }
    }
}
