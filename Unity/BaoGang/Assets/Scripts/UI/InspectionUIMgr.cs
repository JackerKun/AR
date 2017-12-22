using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InspectionUIMgr : MonoBehaviour
{
	public InspectionItem ItemPrefab;
	public CanvasGroup Items;
	public Transform GyroUICamera;
	[HideInInspector]
	public static UIMode curUIMode;
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
		initSubmitPose = submitBtn.localPosition;
		holoGrid = Items.GetComponent<HoloGrid>();
		itemsRootTran = Items.transform;
	}

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
		submitBtn.position = Items.transform.position + Vector3.down * 10f;
		submitBtn.forward = -submitBtn.position;
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
		dialogBox.ShowDialog(item.Info, item);
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
}
