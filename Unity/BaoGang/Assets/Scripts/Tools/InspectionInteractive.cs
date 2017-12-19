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
	GameObject mouseIcon2D;
	Transform mouseIcon3D;
	UIEffects mouseCountDown;

	bool isMouse2D = true;
	//已经有过一次抬头或低头了，再次抬头低头可改变UI显隐
	bool canChangeUI = true;

	// Use this for initialization
	void Start()
	{
		uiMgr = gameObject.GetComponent<InspectionUIMgr>();
		gyroCameraCtrl = FindObjectOfType<GyroCamera>();
		gyroCamera = gyroCameraCtrl.GetComponent<Camera>();
		mouseIcon2D = GameObject.Find("Canvas2D/MouseIcon");
		mouseIcon3D = GameObject.Find("Canvas3D/Mouse").transform;
		mouseCountDown = GameObject.Find("Canvas3D/Mouse/MouseCountDown").GetComponent<UIEffects>();
		screenCenter = new Vector2(Screen.width >> 1, Screen.height >> 1);
		//隐藏UI内容
		uiMgr.HideItems();
	}

	// Update is called once per frame
	void Update()
	{
		//UI的显隐操作
		UpdateUIShow();
		//面板显示的时候可以交互
		if (uiMgr.curUIMode == InspectionUIMgr.UIMode.ButtonList)
		{
			//开始检测交互
			UpdateInteractive();
		}
	}

	#region UI的显隐操作

	void UpdateUIShow()
	{
		//检测是否显隐UI
		if (canChangeUI)
		{
			if (gyroCameraCtrl.horizonAngle > 45f)
			{
				//面板未打开，打开面板
				if (uiMgr.curUIMode == InspectionUIMgr.UIMode.Off)
				{
					uiMgr.ShowItems();
				}
				else if (uiMgr.curUIMode == InspectionUIMgr.UIMode.ButtonList)
				{
					uiMgr.HideItems();
				}
				else if (uiMgr.curUIMode == InspectionUIMgr.UIMode.DialogBox)
				{
					uiMgr.HideOptionDialog();
				}
				canChangeUI = false;
			}
		}
		//重制状态，再次抬头可以继续显隐UI
		else if (gyroCameraCtrl.horizonAngle < 20f)
		{
			canChangeUI = true;
		}
	}

	#endregion

	#region UI的交互操作

	InspectionItem curBtn;

	void UpdateInteractive()
	{
		RaycastHit hit;
		if (Physics.Raycast(gyroCamera.ScreenPointToRay(screenCenter), out hit))
		{
			if (hit.collider != null)
			{
				//显示3D鼠标
				SetMouse2D(false);
				mouseIcon3D.transform.position = hit.point;
				mouseIcon3D.forward = -hit.normal;
				InspectionItem tmpBtn = hit.transform.GetComponent<InspectionItem>();
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
						mouseCountDown.StopFillImage();
					}
					curBtn = tmpBtn;
					//鼠标经过操作
					curBtn.MouseHover();
					ShowMouseCountDown(uiMgr.ShowOptionDialog, tmpBtn);
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

	//显示鼠标倒计时
	void ShowMouseCountDown(Action<InspectionItem> act, InspectionItem item)
	{
		mouseCountDown.StartFillImage(act, item);
	}

	void ClearBtnHover()
	{
		if (curBtn)
		{
			//上一个按钮执行退出操作
			curBtn.MouseExit();
			curBtn = null;
			SetMouse2D(true);
		}
	}

	/// <summary>
	/// 切换鼠标样式2d/3d
	/// </summary>
	/// <param name="is2D">If set to <c>true</c> is2 d.</param>
	void SetMouse2D(bool is2D)
	{
		if (is2D != isMouse2D)
		{
			isMouse2D = !isMouse2D;
			mouseIcon2D.SetActive(is2D);
			mouseIcon3D.gameObject.SetActive(!is2D);
			//进入2D模式后，停止倒计时
			if (isMouse2D)
			{
				mouseCountDown.StopFillImage();
			}
		}
	}

	#endregion
}
