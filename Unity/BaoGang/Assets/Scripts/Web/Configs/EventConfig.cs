using System;

namespace AR.Configs
{
	public static class EventConfig
	{
		//请求socket连接
		public const string TANK = "BUCKET";
		//流程消息
        public const string AR_WORKFLOW = "AR_WORKFLOW";
		//结束流程
        public const string AR_INSPECTIONCOMMIT = "AR_INSPECTIONCOMMIT";


		/// <summary>
		/// The ar checkpoint.
		/// </summary>
        public const string AR_CHECKPOINT = "AR_CHECKPOINT";
        public const string AR_BLUETOOTHCHECKPOINT = "AR_BLUETOOTHCHECKPOINT";
		//监听巡检点事件
        public const string CHECKRESULTSUBMIT = "CHECKRESULTSUBMIT";
		//订阅返回桶的信息
        public const string RESPONSE_TANK = "AR_BUCKET";
		//订阅返管道的信息
        public const string RESPONSE_PIPE = "RESPONSE_PIPE";
		//订阅返专家的信息
        public const string RESPONSE_EXPERT = "RESPONSE_EXPERT";
		//订阅返专家的信息
        public const string REQUEST_CAPTURE_REGISTER = "REQUEST_CAPTURE_REGISTER";
        public const string RESPONSE_CAPTURE_START = "RESPONSE_CAPTURE_START";
        public const string REQUEST_CAPTURE = "REQUEST_CAPTURE";
        public const string RESPONSE_CAPTURE = "RESPONSE_CAPTURE";

		//请求远程看是否网络还保持连接
		public const string REQUEST_HEART = "REQUEST_HEART";
		public const string RESPONSE_HEART = "RESPONSE_HEART";

		public const string WARN_MESSAGE = "WARN_MESSAGE";

		public const string ONLINE = "ONLINE";
		public const string AR_ONLINE = "AR_ONLINE";
		public const string OPEN_CAMERA = "OPEN_CAMERA";
		public const string PHOTO = "PHOTO";
		public const string AR_PHOTO = "AR_PHOTO";

		public const string AR_DISCONNECT = "AR_DISCONNECT";

	};
}

