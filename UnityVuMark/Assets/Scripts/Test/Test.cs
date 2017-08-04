using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HopeRun;

public class Test : MonoBehaviour
{
	UIManager uiMgr;
	// Use this for initialization
	void Start ()
	{
		uiMgr = GetComponent<UIManager> ();
		StartCoroutine (SwitchValveState ());
	}

	// Update is called once per frame
	void Update ()
	{
		//		InitCanvas ();
		UIManager.UpdateLiquidHeight ((Mathf.Sin (Time.time * .2f) + 1f) * 5f);
		if (Input.GetKeyDown (KeyCode.J)) {
			UIManager.ChangeValveState (ValveState.ON);
		} else if (Input.GetKeyDown (KeyCode.K)) {
			UIManager.ChangeValveState (ValveState.OFF);
		}
	}

	ValveState curValveState;

	IEnumerator SwitchValveState ()
	{
		while (true) {
			curValveState = (curValveState == ValveState.ON) ? ValveState.OFF : ValveState.ON;
			UIManager.ChangeValveState (curValveState);
			yield return new WaitForSeconds (2f);
		}
	}
}
