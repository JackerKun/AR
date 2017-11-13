using System;

namespace HopeRun.Model
{
	public class TargetDeviceRequest
	{
		public TargetDeviceRequest (Boolean isOpen, String deviceID, string targetID)
		{
			this.isOpen = isOpen;
			this.deviceID = deviceID;
			this.targetID = targetID;
		}

		public override string ToString ()
		{
			return string.Format ("[DeviceRequest: IsOpen={0}, DeviceID={1}, TargetID={2}]", IsOpen, DeviceID, TargetID);
		}

		//0为停止传输，1为开启传输
		public bool isOpen;
		//眼镜的标识（返回给哪副眼镜）
		public string deviceID;
		//设备编号（每个识别的设备具有唯一编号）
		public string targetID;

		public bool IsOpen {
			get {
				return isOpen;
			}
			set {
				isOpen = value;
			}
		}

		public string DeviceID {
			get {
				return deviceID;
			}
			set {
				deviceID = value;
			}
		}

		public string TargetID {
			get {
				return targetID;
			}
			set {
				targetID = value;
			}
		}
	}
}

