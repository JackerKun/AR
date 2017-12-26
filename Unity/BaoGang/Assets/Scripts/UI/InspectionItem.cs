using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class InspectionItem : Ryan3DButton
{
	bool selectStatus;
	RectTransform rectTran;
	public RectTransform RectTran
	{
		get { return rectTran; }
	}
	// 点开后显示的描述文字
	string dialogContent;
	public string id;
	public string Info
	{
		get { return dialogContent; }
	}
	public SelectStatus curSltStatus;
	public enum SelectStatus
	{
		Null = 1,
		No,
		Yes
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
	}

	#region 实现按钮类

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

	public override void MouseSelect()
	{
		InspectionUIMgr.Instance.ShowOptionDialog(this);
	}
	#endregion
}
