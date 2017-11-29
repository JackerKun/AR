using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MessageDialog : MonoBehaviour
{
	Text text;
	RectTransform rectTran;
	// Use this for initialization
	void Start ()
	{
		rectTran = transform.GetComponent<RectTransform> ();
		DestroyDialog ();
	}
	
	// Update is called once per frame
	public void InitText (string str)
	{
		text = transform.Find ("Message").GetComponent<Text> ();
		text.text = str;
	}

	public void DestroyDialog ()
	{
		rectTran.DOAnchorPosY (Screen.height * .5f, .5f).SetDelay (3f).SetEase (Ease.InSine).OnComplete (() => {
			Destroy (gameObject);
		});
	}
}
