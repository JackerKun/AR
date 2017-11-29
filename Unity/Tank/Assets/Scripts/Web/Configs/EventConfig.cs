using System;

namespace AR.Configs
{
	public static class  EventConfig
	{
		//请求socket连接
		public readonly static string TANK = "BUCKET";
		//流程消息
		public readonly static string AR_WORKFLOW = "AR_WORKFLOW";
		//订阅返回桶的信息
		public readonly static string RESPONSE_TANK = "AR_BUCKET";
		//订阅返管道的信息
		public readonly static string RESPONSE_PIPE = "RESPONSE_PIPE";
		//订阅返专家的信息
		public readonly static string RESPONSE_EXPERT = "RESPONSE_EXPERT";
		//订阅返专家的信息
		public readonly static string REQUEST_CAPTURE_REGISTER = "REQUEST_CAPTURE_REGISTER";
		public readonly static string RESPONSE_CAPTURE_START = "RESPONSE_CAPTURE_START";
		public readonly static string REQUEST_CAPTURE = "REQUEST_CAPTURE";
		public readonly static string RESPONSE_CAPTURE = "RESPONSE_CAPTURE";

		//请求远程看是否网络还保持连接
		public readonly static string REQUEST_HEART = "REQUEST_HEART";
		public readonly static string RESPONSE_HEART = "RESPONSE_HEART";

		public readonly static string WARN_MESSAGE = "WARN_MESSAGE";

		public readonly static string ONLINE = "ONLINE";
		public readonly static string AR_ONLINE = "AR_ONLINE";
		public readonly static string OPEN_CAMERA = "OPEN_CAMERA";
		public readonly static string PHOTO = "PHOTO";
		public readonly static string AR_PHOTO = "AR_PHOTO";

		public readonly static string AR_DISCONNECT = "AR_DISCONNECT";
	};
}

