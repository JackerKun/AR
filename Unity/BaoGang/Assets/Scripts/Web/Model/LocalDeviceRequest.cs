using System;
using UnityEngine.SceneManagement;

namespace HopeRun.Model
{
	public class LocalDeviceRequest
	{
		public LocalDeviceRequest(string scene)
		{
			this.deviceID = GlobalManager.DeviceID;
			this.deviceName = GlobalManager.DeviceName;
			this.deviceType = "1";
			this.scene = scene;
		}

		//设备ID
		public string deviceID;
		//眼镜的标识
		public string deviceName;
		//设备编号（每个识别的设备具有唯一编号）
		public string deviceType;
		//设备编号（每个识别的设备具有唯一编号）
		public string scene;
	}
}

