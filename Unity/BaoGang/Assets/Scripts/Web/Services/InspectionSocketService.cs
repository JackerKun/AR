using UnityEngine;
using AR.Configs;
using AR.Model;
using SimpleJSON;
using HopeRun;

public class InspectionSocketService : IRegistServer
{
	//private SocketService socketService;
	//private Socket socket;

	//private System.Action<Tank> myCallback;

	static InspectionSocketService _instance;

	public static InspectionSocketService Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new InspectionSocketService();
			}
			return _instance;
		}
	}

	public void RegistServices()
	{
		#region 旧的代码
		//socketService.InitScene("inspection",
		//    (socket, packet, args) =>
		//    {
		//        Debug.LogError("Init Scene.." + packet.Payload);
		//        DealState(packet.Payload, true);
		//    });
		//监听流程
		//socketService.AddListener(EventConfig.AR_CHECKPOINT,
		//    (socket, packet, args) =>
		//    {
		//        Debug.Log(packet.Payload);
		//        DealState(packet.Payload);
		//    });
		#endregion
		WebManager.Instance.Connect("inspection", node =>
		{
			DealInspectionMsg(node[0]);
		});
		WebManager.Instance.On(EventConfig.AR_INSPECTIONCOMMIT, node =>
		{
			Debug.Log("返回首页");
			GlobalManager.LoadScene("Welcome");
		});
		WebManager.Instance.On(EventConfig.AR_BLUETOOTHCHECKPOINT, node =>
		{
			InspectionMgr.LastMsgT = Time.time;
			DealInspectionMsg(node[0]);
		});
	}

	// 提交工单
	public void SubmitWorkOrder(JSONNode node)
	{
		WebManager.Instance.Emit(EventConfig.CHECKRESULTSUBMIT, node.ToString());
	}

	public void DealInspectionMsg(JSONNode jn)
	{
		if (string.IsNullOrEmpty(jn.ToString()) || jn.IsNull)
		{
			return;
		}
		// 创建工单
		InspectionMgr.Instance.UpdateWorkOrder(jn);
		// 保存巡检项数据
		InspectionMgr.Instance.UpdateItemsData(jn["checkResultData"]);
	}
}

