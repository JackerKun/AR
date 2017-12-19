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
	public UIMode curUIMode;
	Transform itemsRootTran;
	DialogBox dialogBox;
	HoloGrid holoGrid;
	//面板离自己前方的距离
	float zDist = 250f;
	List<InspectionItem> items = new List<InspectionItem>();
	//当前界面状态
	public enum UIMode
	{
		Off,
		ButtonList,
		DialogBox
	}
	public Vector3 ContentPosition
	{
		get { return Items.transform.position; }
	}
	public Quaternion ContentRotation
	{
		get { return Items.transform.rotation; }
	}
	// Use this for initialization
	void Start()
	{
		dialogBox = GameObject.Find("Canvas3D/DialogBox").GetComponent<DialogBox>();
		holoGrid = Items.GetComponent<HoloGrid>();
		itemsRootTran = Items.transform;
	}

	// Update is called once per frame
	void Update()
	{
		//if (Input.GetKeyDown(KeyCode.Z))
		//{
		//	ShowContents();
		//}
		//else if (Input.GetKeyDown(KeyCode.X))
		//{
		//	HideContents();
		//}
	}

	//显示提示用户抬头的箭头
	public void ShowArrow()
	{

	}

	#region 巡检项内容
	public void UpdateItemsLayout()
	{
		List<RectTransform> itemTrans = new List<RectTransform>();
		foreach (var item in items)
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
		curUIMode = UIMode.ButtonList;
	}

	public void HideItems()
	{
		Items.DOFade(0f, .2f).SetEase(Ease.OutSine);
		curUIMode = UIMode.Off;
	}
	#endregion

	#region 巡检项

	//显示对话内容面板
	public void ShowOptionDialog(InspectionItem item)
	{
		//dialogBox.ShowDialog(info, itemsRootTran.position, itemsRootTran.rotation);
		dialogBox.ShowDialog(item.Info, item.transform.position, item.transform.rotation);
		curUIMode = UIMode.DialogBox;
	}

	public void HideOptionDialog()
	{
		dialogBox.HideDialog();
		curUIMode = UIMode.ButtonList;
	}

	// 清除巡检项
	public void ResetItems()
	{
		for (int i = 0; i < items.Count; i++)
		{
			Destroy(items[i].gameObject);
		}
		items = new List<InspectionItem>();
	}

	public void InsertItem(string summary, string dialog)
	{
		InspectionItem item = Instantiate(ItemPrefab, Vector3.zero, Quaternion.identity, itemsRootTran);
		item.Init(summary, dialog);
		items.Add(item);
	}

	public void ShowItem()
	{
		Items.DOFade(1f, .5f).SetEase(Ease.InSine);
	}

	//隐藏巡检项
	public void HideItem()
	{
		Items.DOFade(0f, .2f).SetEase(Ease.OutSine);
	}
	#endregion
}
