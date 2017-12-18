using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HopeRun.Message
{
	public class MessageLibrary
	{
		/// <summary>
		/// 根据服务器端传来的编码消息，输出对应消息内容
		/// </summary>
		/// <returns>The message.</returns>
		/// <param name="msgCode">Message code.</param>
		public static string GetMessage (string msgCode)
		{
			switch (msgCode) {
			case "2":
				{
					return "确认戴好防护面罩和手套";
				}
				break;
			case "3":
				{
					return "请在辅助设备上选择拍照";
				}
				break;
			case "4":
				{
					return "请在辅助设备上输入车牌号码";
				}
				break;
			case "5":
				{
					return "请在辅助设备上确认车辆安全配置";
				}
				break;
			case "6":
				{
					return "请在辅助设备上选择拍照";
				}
				break;
			case "7":
				{
					return "请在辅助设备上输入承运人姓名和供应商";
				}
				break;
			case "8":
				{
					return "确认戴好防护面罩";
				}
				break;
			case "9":
				{
					return "请在辅助设备上进行加药前的签名确认";
				}
				break;
			case "14":
				{
					return "请在辅助设备上完成签名";
				}
				break;
			case "15":
				{
					return "请确认关闭进药阀门";
				}
				break;
			case "16":
				{
					return "请在辅助设备端选择拍照";
				}
			case "17":
				{
					return "请确认脱开进药管、封号盲板";
				}
			case "18":
				{
					return "请进行现场冲洗，并在辅助设备上确认";
				}
				break;
			case "19":
				{
					MainSceneMgr.LazyQuit = true;
					return "加药结束";
				}
				break;
			default:
				{
					if (msgCode.StartsWith ("ARTank_")) {
						return "进入AR识别模式，请站在指定区域看向" + msgCode.Replace ("ARTank_", "") + "罐";
					}
					return msgCode;
				}
				break;
			}
		}
	}
}