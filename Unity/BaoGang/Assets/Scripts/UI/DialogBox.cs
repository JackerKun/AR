using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogBox : MonoBehaviour, IRyanDialog
{
	Text content;
	CanvasGroup curPanel;

	// Use this for initialization
	void Start()
	{
		content = transform.Find("Text").GetComponent<Text>();
		curPanel = GetComponent<CanvasGroup>();
		curPanel.alpha = 0;
	}

	public void ShowDialog(string info, Vector3 position, Quaternion rotation)
	{
		transform.position = position;
		transform.rotation = rotation;
		Vector3 targetPose = transform.localPosition - transform.forward * 100f;
		content.text = info;

		Sequence mySque = DOTween.Sequence();
		mySque.Append(curPanel.DOFade(1, .3f).SetEase(Ease.InOutSine));
		mySque.Join(transform.DOLocalMove(targetPose, .3f));
		mySque.Append(transform.DOPunchPosition(Vector3.one * .1f, 1f));
	}

	public void HideDialog()
	{
		curPanel.alpha = 0;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
