using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Container : MonoBehaviour
{

	//灌满时的液位高度
	protected float totalAmount;
	protected float currentAmount;

	public abstract void Deposit ();
}
