using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogBox : MonoBehaviour, IRyanDialog
{
	Text content;
	Slider slider;
	Image sliderFront;
	Image sliderBG;
	CanvasGroup curPanel;
	InspectionItem item;

	// Use this for initialization
	void Start()
	{
		content = transform.Find("Text").GetComponent<Text>();
		slider = transform.Find("YesOrNo").GetComponent<Slider>();
		sliderFront = slider.fillRect.GetComponent<Image>();
		sliderBG = slider.transform.Find("Background").GetComponent<Image>();
		curPanel = GetComponent<CanvasGroup>();
		curPanel.alpha = 0;
	}

	public void ShowDialog(InspectionItem item)
	{
		this.item = item;
		transform.position = item.transform.position;
		transform.rotation = item.transform.rotation;
		Vector3 targetPose = InspectionUIMgr.Instance.GyroUICamera.forward.normalized * 200f;
		content.text = item.Info;

		Sequence mySque = DOTween.Sequence();
		mySque.Append(curPanel.DOFade(1, .3f).SetEase(Ease.InOutSine));
		mySque.Join(transform.DOLocalMove(targetPose, .3f));
		mySque.Append(transform.DOPunchPosition(Vector3.one * .1f, 1f));
	}

	public void UpdateSlider(float v)
	{
		slider.value = v;
		sliderFront.color = new Color(1f - v, 1f, 1f - v, 1f);
		sliderBG.color = new Color(1f, 1f + v, 1f + v, 1f);
		//选择Yes
		if (v >= 1f)
		{
			CheckItem(true);
			Debug.Log("Yes");
		}
		else if (v <= -1f)
		{
			CheckItem(false);
			Debug.Log("No");
		}
	}

	//勾选巡检项
	void CheckItem(bool isOK)
	{
		item.CheckStatus(isOK);
		HideDialog();
		InspectionUIMgr.curUIMode = InspectionUIMgr.UIMode.ItemList;
	}

	public void HideDialog()
	{
		curPanel.alpha = 0;
	}
}
