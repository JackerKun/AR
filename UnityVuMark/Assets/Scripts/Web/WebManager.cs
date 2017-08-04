using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AR.Model;

public class WebManager
{
	static TankSceneService _instance;

	public static TankSceneService Instance {
		get { 
			if (_instance == null) {
				_instance = new TankSceneService ();
			}
			return _instance;
		}
	}
}