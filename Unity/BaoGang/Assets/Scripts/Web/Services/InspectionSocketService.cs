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
		WebManager.Instance.On(EventConfig.AR_CHECKPOINT, node =>
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

	public void onScaning(System.Action<Tank> callback)
	{
		//		myCallback = callback;
		//		Debug.Log("onScaning:" + EventConfig.RESPONSE_TANK);
		//		socketService.Subscribe(EventConfig.RESPONSE_TANK, OnResponseTank);

		WebManager.Instance.StartRequestData(EventConfig.TANK, EventConfig.RESPONSE_TANK, node =>
		{
			var tank = new Tank(node[0]);
			GlobalManager.IS_WORKFLOW = (tank.sceneName.Equals("workflow"));
			callback(tank);
		});
	}

	public void onLostScaning(string targetID)
	{
		//socketService.DisSubscribe(EventConfig.RESPONSE_TANK);
		WebManager.Instance.CancleRequestData(EventConfig.RESPONSE_TANK);
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

	//void DealState(string payload, bool isOnline = false)
	//{
	//    Debug.Log(JSON.Parse(payload));
	//    JSONNode jn = JSON.Parse(payload)[1];
	//    if (jn["status"] == "error")
	//    {
	//        UIManager.ShowErrorMessage(jn["message"]);
	//    }
	//    else
	//    {
	//        SceneMsgDealer.DealInspectionMsg(jn);
	//    }
	//}


	//public InspectionSocketService()
	//{
	//    socketService = WebManager.Instance.socket;
	//}
	////连接成功后回调
	//private void OnResponseTank(Socket socket, Packet packet, params object[] args)
	//{
	//    Debug.Log("Connect...");
	//    JSONNode jRoot = JSON.Parse(packet.Payload)[1];

	//    if (jRoot["status"] == "success")
	//    {
	//        Debug.Log(packet.Payload);
	//        //TODO; 数据的转换
	//        JSONNode data = jRoot["data"];
	//        Tank bucket = new Tank(data);
	//        GlobalManager.IS_WORKFLOW = (data["sceneName"] == "workflow");
	//        myCallback(bucket);
	//    }
	//    else
	//    {
	//        UIManager.ShowErrorMessage(jRoot["message"]);
	//    }
	//}
}

