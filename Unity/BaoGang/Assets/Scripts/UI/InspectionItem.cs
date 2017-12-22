using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class InspectionItem : MonoBehaviour, Ryan3DButton
{
	Vector3 initScale;
	Vector3 targetScale;
	bool isHover = false;
	bool selectStatus;
	RectTransform rectTran;
	public RectTransform RectTran
	{
		get { return rectTran; }
	}
	// 点开后显示的描述文字
	string dialogContent;
	string id;
	public string Info
	{
		get { return dialogContent; }
	}
	public SelectStatus curSltStatus;
	public enum SelectStatus
	{
		Yes,
		No,
		Null
	}
	//显示的文字内容
	Text textContent;
	Image itemBG;
	//当前的选项
	Option curOption;
	public enum Option
	{
		Yes,
		No
	};
	//选择值
	string optionStr;

	//打开对话框后显示的内容
	public void Init(string id, string summary, string dialogContent)
	{
		rectTran = GetComponent<RectTransform>();
		textContent = transform.Find("Text").GetComponent<Text>();
		this.id = id;
		textContent.text = summary;
		itemBG = transform.GetComponent<Image>();
		this.dialogContent = dialogContent;
		curSltStatus = SelectStatus.Null;
		MouseInit();
	}

	#region 实现按钮类
	public void MouseExit()
	{
		transform.DOScale(initScale, .2f).SetEase(Ease.OutSine);
		isHover = false;
	}

	internal void CheckStatus(bool isOK)
	{

		if (isOK)
		{
			BtnSelect(Option.Yes);
		}
		else
		{
			BtnSelect(Option.No);
		}
		InspectionUIMgr.curUIMode = InspectionUIMgr.UIMode.ItemList;
	}

	public void BtnSelect(Option option)
	{
		if (option == Option.Yes)
		{
			curSltStatus = SelectStatus.Yes;
			itemBG.color = Color.green;
		}
		else if (option == Option.No)
		{
			curSltStatus = SelectStatus.No;
			itemBG.color = Color.red;
		}
	}

	public void MouseHover()
	{
		if (!isHover)
		{
			transform.DOScale(targetScale, .2f).SetEase(Ease.OutSine);
			isHover = true;
		}
	}

	public void MouseInit()
	{
		initScale = transform.localScale;
		targetScale = initScale * .8f;
	}

	public void MouseSelect()
	{
	}
	#endregion
}
