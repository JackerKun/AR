using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PipeUIUtility:MonoBehaviour
{
	static PipeUIUtility _instance;

	public static PipeUIUtility Instance {
		get {
			return _instance;
		}
	}
	//识别对齐相机点的参考的目标点
	public Image RefTarget;
	// Use this for initialization
	void Start ()
	{
		_instance = GetComponent< PipeUIUtility > ();
		RefTarget.color = Color.red;
	}

	public void RefTargetFound ()
	{
		RefTarget.rectTransform.localScale = Vector3.one;
		RefTarget.DOColor (Color.green, 1f);
//		RefTarget.color = Color.green;
		RefTarget.DOFade (0, 1f).SetDelay (1f).SetDelay (1f);
		RefTarget.rectTransform.DOScale (Vector3.one * 2f, 1f).SetDelay (1f);
	}

	public void RefTargetLost ()
	{
		DOTween.Clear ();
		RefTarget.rectTransform.localScale = Vector3.one;
		RefTarget.color = Color.red;
	}
	
	//	// Update is called once per frame
	//	void Update ()
	//	{
	//		if (Input.GetKeyDown (KeyCode.J)) {
	//			PipeUIUtility.Instance.RefTargetLost ();
	//		} else if (Input.GetKeyDown (KeyCode.K)) {
	//			PipeUIUtility.Instance.RefTargetFound ();
	//		}
	//	}
}
