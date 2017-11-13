using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TiLiHuo
{
	[MenuItem ("Ryan/Reset Position")]
	public static void ResetPosition ()
	{
		Transform[] trans = Selection.transforms;
		int column = 0; 
		int row = 0;
		for (int i = 0; i < trans.Length; i++) {
			trans [i].position = new Vector3 (i % 5, 0, row);
			if (++column == 5) {
				column = 0;
				++row;
			}

		}
	}
}
