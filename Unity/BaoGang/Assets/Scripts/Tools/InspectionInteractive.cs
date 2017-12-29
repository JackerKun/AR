using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//巡检场景交互管理
public class InspectionInteractive : MonoBehaviour
{
	GyroCamera gyroCameraCtrl;
	Camera gyroCamera;
	Vector2 screenCenter;
	InspectionUIMgr uiMgr;

	public static bool isMouse2D = true;
	//已经有过一次抬头或低头了，再次抬头低头可改变UI显隐
	public static bool CanChangeUI = true;

	// Use this for initialization
	void Start()
	{
		uiMgr = gameObject.GetComponent<InspectionUIMgr>();
		gyroCameraCtrl = FindObjectOfType<GyroCamera>();
		gyroCamera = gyroCameraCtrl.GetComponentInChildren<Camera>();
		screenCenter = new Vector2(Screen.width >> 1, Screen.height >> 1);
		//隐藏UI内容
		uiMgr.HideItems();
	}

	// Update is called once per frame
	void Update()
	{
		if (!InspectionMgr.isInspecting)
			return;
		//UI的显隐操作
		UpdateUIShow();
		//面板显示的时候可以交互
		if (InspectionUIMgr.curUIMode == InspectionUIMgr.UIMode.ItemList)
		{
			//开始检测交互
			UpdateInteractive();
		}
	}

	#region UI的显隐操作

	void UpdateUIShow()
	{
		//检测是否显隐UI
		if (CanChangeUI)
		{
			//显隐按钮内容
			if (gyroCameraCtrl.horizonAngle > 45f)
			{
				//面板未打开，打开面板
				if (InspectionUIMgr.curUIMode == InspectionUIMgr.UIMode.Off)
				{
					uiMgr.ShowItems();
				}
				else if (InspectionUIMgr.curUIMode == InspectionUIMgr.UIMode.ItemList)
				{
					uiMgr.HideItems();
				}
				else if (InspectionUIMgr.curUIMode == InspectionUIMgr.UIMode.DialogBox)
				{
					uiMgr.HideOptionDialog();
				}
				CanChangeUI = false;
			}
			//对话框打开着就可以转头做选择
			if (InspectionUIMgr.curUIMode == InspectionUIMgr.UIMode.DialogBox)
			{
				uiMgr.UpdateSliderValue();
			}
		}
		//重制状态，再次抬头可以继续显隐UI
		else if (gyroCameraCtrl.horizonAngle < 20f)
		{
			CanChangeUI = true;
		}
	}

	#endregion

	#region UI的交互操作

	Ryan3DButton curBtn;

	void UpdateInteractive()
	{
		RaycastHit hit;
		if (Physics.Raycast(gyroCamera.ScreenPointToRay(screenCenter), out hit))
		{
			if (hit.collider != null)
			{
				//显示3D鼠标
				InspectionUIMgr.Instance.SetMouse2D(false);
				InspectionUIMgr.Instance.mouseIcon3D.transform.position = hit.point;
				InspectionUIMgr.Instance.mouseIcon3D.forward = -hit.normal;
				Ryan3DButton tmpBtn = hit.transform.GetComponent<Ryan3DButton>();
				if (tmpBtn != null)
				{
					//同一个按钮不作操作
					if (curBtn == tmpBtn)
					{
						return;
					}
					//从一个按钮射到另一个按钮
					if (curBtn != null && curBtn != tmpBtn)
					{
						//上一个按钮执行退出操作
						curBtn.MouseExit();
						//隐藏鼠标倒计时
						InspectionUIMgr.Instance.StopFillImage();
					}
					curBtn = tmpBtn;
					//鼠标经过操作
					curBtn.MouseHover();
					InspectionUIMgr.Instance.ShowMouseCountDown(tmpBtn);
				}
				else
				{
					ClearBtnHover();
				}
			}
			else
			{
				ClearBtnHover();
			}
		}
		//显示鼠标
		else
		{
			ClearBtnHover();
		}
	}

	void ClearBtnHover()
	{
		if (curBtn != null)
		{
			//上一个按钮执行退出操作
			curBtn.MouseExit();
			curBtn = null;
			InspectionUIMgr.Instance.SetMouse2D(true);
		}
	}

	#endregion
}
