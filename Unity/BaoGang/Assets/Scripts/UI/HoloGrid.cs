using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloGrid : MonoBehaviour
{
	float radiansX = 10f * Mathf.Deg2Rad;
	float radiansY = 10f * Mathf.Deg2Rad;
	//行数
	int yCount = 2;

	public void UpdateGridLayout(List<RectTransform> rectTrans, float dist)
	{
		float SizeX = dist * Mathf.Sin(radiansX);
		float SizeY = dist * Mathf.Sin(radiansY);
		//直径
		float radius = dist * 2f;
		// 总共的列数
		int xCount = Mathf.CeilToInt((float)rectTrans.Count / yCount);
		// 居中元素的下标
		float center = xCount / 2f;
		// 中心点的位置的下标
		Vector2 centerIndex = new Vector2((xCount - 1) / 2f, (yCount - 1) / 2f);
		for (int i = 0; i < rectTrans.Count; i++)
		{
			float x = dist * Mathf.Sin(radiansX * ((i % xCount) - centerIndex.x));
			float y = dist * Mathf.Sin(radiansY * -((i / xCount) - centerIndex.y));
			float z = dist * (Mathf.Cos(radiansX * ((i % xCount) - centerIndex.x)) - 1f);
			Vector3 tmpPose = new Vector3(x, y, z);
			rectTrans[i].localPosition = tmpPose;
			rectTrans[i].forward = rectTrans[i].position;
			rectTrans[i].sizeDelta = new Vector2(SizeX, SizeY);
			// 面板尺寸
			rectTrans[i].GetComponent<BoxCollider>().size = new Vector3(SizeX, SizeY, 1f);
			rectTrans[i].name = "Item." + i;
		}
	}
	/*

public void UpdateGridLayout(List<RectTransform> rectTrans)
{
	// 总共的列数
	int xCount = Mathf.CeilToInt((float)rectTrans.Count / yCount);
	// 居中元素的下标
	float center = xCount / 2f;
	// Transform的列间距
	float tranCellOffsetX = SizeX + SpaceX;
	// Transform的行间距
	float tranCellOffsetY = SizeY + SpaceY;
	// 中心点的位置的下标
	Vector2 centerIndex = new Vector2((xCount - 1) / 2f, (yCount - 1) / 2f);
	for (int i = 0; i < rectTrans.Count; i++)
	{
		float x = ((i % xCount) - centerIndex.x) * tranCellOffsetX;
		float y = -((i / xCount) - centerIndex.y) * tranCellOffsetY;
		Vector3 tmpPose = new Vector3(x, y, 0);

		rectTrans[i].sizeDelta = new Vector2(SizeX, SizeY);
		rectTrans[i].GetComponent<BoxCollider>().size = new Vector3(SizeX, SizeY, 1f);
		rectTrans[i].localPosition = tmpPose;
		rectTrans[i].localRotation = Quaternion.identity;
	}
	}
	 */
}
