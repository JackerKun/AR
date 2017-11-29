using System;

namespace AR.Model
{
	public class Tank
	{
		public Tank (float liquidHeight, float limitLevel, float highestLevel, bool valveStatus, bool blowerStatus)
		{
			this.liquidHeight = liquidHeight;
			this.limitLevel = limitLevel;
			this.highestLevel = highestLevel;
			this.valveStatus = valveStatus;
			this.blowerStatus = blowerStatus;
		}

		public override string ToString ()
		{
			return string.Format (liquidHeight + " >> " + limitLevel + " >> " + highestLevel + " >> " + valveStatus + " >> " + blowerStatus);
		}

		//液位高度
		public float liquidHeight;
		//限制高度
		public float limitLevel;
		//最大高度
		public float highestLevel;
		//阀门状态
		public bool valveStatus;
		//风扇状体
		public bool blowerStatus;
	}
}

