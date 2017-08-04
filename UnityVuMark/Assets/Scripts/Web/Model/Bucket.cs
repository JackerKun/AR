using System;

namespace AR.Model
{
	public class Tank
	{
		public Tank ()
		{
			this.isOpen = false;
			this.IsCurrentTarget = false;
			this.IsTrigger = false;
			this.liquidLevel = 0;
		}

		public Tank (bool isOpen, bool IsCurrentTarget, bool IsTrigger, float liquidLevel)
		{
			this.isOpen = isOpen;
			this.IsCurrentTarget = IsCurrentTarget;
			this.IsTrigger = IsTrigger;
			this.liquidLevel = liquidLevel;
		}

		public override string ToString ()
		{
			return string.Format ("[Bucket: IsOpen={0}, LiquidLevel={1}, IsCurrentTarget={2}, IsTrigger={3}]", IsValveOpen, LiquidLevel, IsCurrentTarget, IsTrigger);
		}

		private Boolean isOpen;
		//开关阀是否打开
		private float liquidLevel;
		//液位量
		private Boolean isCurrentTarget;
		//是需要灌入液体的目标桶
		private Boolean isTrigger;
		//是否是识别范围内的目标桶

		public Boolean IsValveOpen {
			get {
				return isOpen;
			}
			set {
				isOpen = value;
			}
		}

		public float LiquidLevel {
			get {
				return liquidLevel;
			}
			set {
				liquidLevel = value;
			}
		}

		public Boolean IsCurrentTarget {
			get {
				return isCurrentTarget;
			}
			set {
				isCurrentTarget = value;
			}
		}

		public Boolean IsTrigger {
			get {
				return isTrigger;
			}
			set {
				isTrigger = value;
			}
		}
	}
}

