using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Ryan3DButton : MonoBehaviour, IRyan3DButton
{
	Vector3 initScale;
	Vector3 targetScale;
	bool isHover = false;
	#region 实现按钮类
	public void MouseExit()
	{
		transform.DOScale(initScale, .2f).SetEase(Ease.OutSine);
		isHover = false;
	}

	public void MouseHover()
	{
		if (!isHover)
		{
			transform.DOScale(targetScale, .2f).SetEase(Ease.OutSine);
			isHover = true;
		}
	}

	public void Awake()
	{
		initScale = transform.localScale;
		targetScale = initScale * .8f;
	}

	abstract public void MouseSelect();
	#endregion
}
