using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankMgr : MonoBehaviour
{
	public RawImage flowBGColor;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		flowBGColor.color = TankObj.CurTankColor;
	}
}
