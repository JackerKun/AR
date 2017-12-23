using UnityEngine;

public class Submit3DButton : Ryan3DButton
{
	public override void MouseSelect()
	{
		InspectionMgr.Instance.Submit();
		Debug.LogError("I was Select!");
	}
}
