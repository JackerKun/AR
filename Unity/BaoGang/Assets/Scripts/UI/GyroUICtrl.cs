using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GyroUICtrl : MonoBehaviour
{
	public GyroCamera MyGyro;
	public CanvasGroup UpArrow, MiddleUI;
	RectTransform UpRectTran;
	CanvasGroup curUI = null;
	float initUIDist;
	// Use this for initialization
	void Start ()
	{
		UpRectTran = UpArrow.GetComponent<RectTransform> ();
		UpArrow.alpha = 0;
		MiddleUI.alpha = 0;
		initUIDist = MiddleUI.transform.position.z;
	}

	void ShowUI (CanvasGroup tmpGroup)
	{
		if (curUI != tmpGroup) {
			if (tmpGroup == MiddleUI) {
				InitMiddleUIPose ();
			}
			CanvasGroup tmpUI = curUI;
			tmpUI.DOFade (0f, .5f).SetEase (Ease.OutSine);
			tmpGroup.DOFade (1f, .5f).SetEase (Ease.InSine);
			curUI = tmpGroup;
		}
	}

	void Update ()
	{
		if (MyGyro.horizonAngle < 35f) {
			ShowUI (MiddleUI);
		} else {
			ShowUI (UpArrow);
		}
		UpdateArrowPose ();
	}

	void InitMiddleUIPose ()
	{
		MiddleUI.transform.position = MyGyro.forward * initUIDist;
		MiddleUI.transform.forward = MyGyro.forward;
	}

	void UpdateArrowPose ()
	{
		Vector2 tmpPose = CalArrowPose ();
		UpRectTran.anchoredPosition = tmpPose;
		float zAngle = Mathf.Atan2 (tmpPose.y, tmpPose.x) * Mathf.Rad2Deg + 90f;
//		tmpRote.z = 45;
		UpRectTran.rotation = Quaternion.AngleAxis (zAngle, Vector3.forward);

	}

	Vector2 CalArrowPose ()
	{
		Vector3 angle = MyGyro.gyroAngle;
		//-90~90
		float x = (angle.x - 90f) / 90f;
		float z = (angle.z - 90f) / 90f;
		if (x < 0) {
			z *= -1f;
		}
		return new Vector2 (z * (Screen.width >> 1), x * (Screen.height >> 1));
	}
}