using System;
using HopeRun;

namespace AR.Model
{
	public class PhotoObj
	{
		//设备号
		public string deviceID;
		//照片base64
		public string photoBase64;
		//场景号
		public string scene;
		//session
		public string status;

		public PhotoObj ()
		{
			this.deviceID = GlobalManager.DeviceID;
			this.photoBase64 = GlobalManager.CURRENT_PHOTO_BASE64;
		}

		public PhotoObj (string scene, string status)
		{
			this.deviceID = GlobalManager.DeviceID;
			this.photoBase64 = GlobalManager.CURRENT_PHOTO_BASE64;
			this.scene = scene;
			this.status = status;
		}

		public override string ToString ()
		{
			return string.Format ("[PhotoObj]");
		}
	}
}

