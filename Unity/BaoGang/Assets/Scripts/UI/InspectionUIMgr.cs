using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class InspectionUIMgr : MonoBehaviour
{
	public InspectionItem ItemPrefab;
	public CanvasGroup Items;
	public Transform GyroUICamera;
	[HideInInspector]
	public static UIMode curUIMode;
	UIEffects mouseCountDown;
	Transform itemsRootTran;
	DialogBox dialogBox;
	Transform submitBtn;
	Vector3 initSubmitPose;
	HoloGrid holoGrid;
	//面板离自己前方的距离
	float zDist = 250f;
	static InspectionUIMgr instance;
	public static InspectionUIMgr Instance
	{
		get
		{
			return instance;
		}
	}
	//当前界面状态
	public enum UIMode
	{
		Off,
		ItemList,
		DialogBox
	}

	void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start()
	{
		dialogBox = GameObject.Find("Canvas3D/DialogBox").GetComponent<DialogBox>();
		submitBtn = GameObject.Find("Canvas3D/SubmitBtn").transform;
		mouseIcon2D = GameObject.Find("Canvas2D/MouseIcon");
		mouseIcon3D = GameObject.Find("Canvas3D/Mouse").transform;
		mouseCountDown = GameObject.Find("Canvas3D/Mouse/MouseCountDown").GetComponent<UIEffects>();
		initSubmitPose = submitBtn.localPosition;
		holoGrid = Items.GetComponent<HoloGrid>();
		itemsRootTran = Items.transform;
	}

	#region 鼠标

	public GameObject mouseIcon2D;
	public Transform mouseIcon3D;
	/// <summary>
	/// 切换鼠标样式2d/3d
	/// </summary>
	/// <param name="is2D">If set to <c>true</c> is2 d.</param>
	public void SetMouse2D(bool is2D)
	{
		if (is2D != InspectionInteractive.isMouse2D)
		{
			InspectionInteractive.isMouse2D = !InspectionInteractive.isMouse2D;
			mouseIcon2D.SetActive(is2D);
			mouseIcon3D.gameObject.SetActive(!is2D);
			//进入2D模式后，停止倒计时
			if (InspectionInteractive.isMouse2D)
			{
				StopFillImage();
			}
		}
	}

	#endregion

	//显示提示用户抬头的箭头
	public void ShowArrow()
	{

	}

	#region 巡检项内容
	public void UpdateItemsLayout()
	{
		List<RectTransform> itemTrans = new List<RectTransform>();
		foreach (var item in InspectionMgr.items)
		{
			itemTrans.Add(item.RectTran);
		}
		holoGrid.UpdateGridLayout(itemTrans, zDist);
	}

	public void ShowItems()
	{
		//获取视觉前方
		Vector3 tmpForward = Vector3.Cross(GyroUICamera.right, Vector3.up);
		Items.transform.position = GyroUICamera.position + tmpForward * zDist;
		Items.transform.forward = Items.transform.position - GyroUICamera.position;
		Items.DOFade(1f, .5f).SetEase(Ease.InSine);
		// 显示提交工单按钮
		submitBtn.position = itemsRootTran.position;
		submitBtn.DOMove(itemsRootTran.position + Vector3.down * 120f, .5f).SetEase(Ease.InCirc);
		submitBtn.forward = submitBtn.position;
		submitBtn.gameObject.SetActive(true);
		curUIMode = UIMode.ItemList;
	}

	public void HideItems()
	{
		Items.DOFade(0f, .2f).SetEase(Ease.OutSine);
		submitBtn.gameObject.SetActive(false);
		curUIMode = UIMode.Off;
	}
	#endregion

	#region 巡检项

	//显示对话内容面板
	public void ShowOptionDialog(InspectionItem item)
	{
		dialogBox.ShowDialog(item);
		curUIMode = UIMode.DialogBox;
	}

	public void HideOptionDialog()
	{
		dialogBox.HideDialog();
		curUIMode = UIMode.ItemList;
	}

	public void UpdateSliderValue()
	{
		#region 检测脸朝向对话框左面还是右面
		//右侧
		Vector3 right = Vector3.Cross(Vector3.up, GyroUICamera.forward);
		float turnValue = Vector3.Dot(right, -dialogBox.transform.forward);
		//转30度
		turnValue *= 180f / 30f;
		#endregion
		dialogBox.UpdateSlider(turnValue);
	}

	public void InsertItem(string id, string summary, string dialog)
	{
		InspectionItem item = Instantiate(ItemPrefab, Vector3.zero, Quaternion.identity, itemsRootTran);
		item.Init(id, summary, dialog);
		InspectionMgr.items.Add(item);
	}
	#endregion

	internal void StopFillImage()
	{
		mouseCountDown.StopFillImage();
	}

	internal void ShowMouseCountDown(Ryan3DButton tmpBtn)
	{
		mouseCountDown.StartFillImage(tmpBtn);
	}
}