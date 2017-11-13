using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomValue : MonoBehaviour
{
	public bool IsRandom = false;
	Slider slider;
	Text text;
	float curValue;
	float step;
	string origText;

	// Use this for initialization
	void Start ()
	{
		text = transform.Find ("Text").GetComponent<Text> ();
		origText = text.text;
		slider = transform.Find ("Slider").GetComponent<Slider> ();
		curValue = Random.Range (slider.minValue, slider.maxValue);
		step = (slider.maxValue - slider.minValue) / Random.Range (20f, 30f);
		PlayValue ();
	}
	
	// Update is called once per frame
	public void PlayValue ()
	{
		if (IsRandom) {
			StartCoroutine (UpdateRandomValue ());
		} else {
			StartCoroutine (UpdateRaiseValue ());
		}
	}

	public void StopValue ()
	{
		StopCoroutine (UpdateRandomValue ());
		StopCoroutine (UpdateRaiseValue ());
	}

	IEnumerator UpdateRandomValue ()
	{
		while (true) {
			yield return new WaitForSeconds (Random.Range (1f, 2f));
			curValue = Random.Range (slider.minValue, slider.maxValue);
			text.text = origText.Replace ("#", string.Format ("{0:N2}", curValue));
			slider.value = curValue;
		}
	}

	IEnumerator UpdateRaiseValue ()
	{
		while (true) {
			yield return new WaitForSeconds (Random.Range (.5f, 1f));
			curValue += step;
			if (curValue > slider.maxValue) {
				curValue = slider.minValue;
			}
			text.text = origText.Replace ("#", string.Format ("{0:N2}", curValue));
			slider.value = curValue;
		}
	}
}
