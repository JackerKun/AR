using UnityEngine;
using AR.Configs;
using AR.Model;
using SimpleJSON;
using HopeRun;

public class InspectionSocketService
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
		Debug.LogError("Not Error");

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



	#region json样本
	string jsonStr = @"{
  ""status"":""success"",
  ""message"":""获取巡检点内容成功"",
  ""data"":{
          ""jobNumber"":""2017123123123123"",
          ""checkPoint"":{checkPointID:""0001"",checkPointTitle:""酸性中和池""},
          ""checkContent"":[{checkContentID:""000101"",nickname:""chaolvji"",checkContent:""一级中和槽"",checkDesc:""酸性中和池PH值是否正常""},
                        {checkContentID:""000102"",nickname:""xunhuanshuixiang1"",checkContent:""高密管"",checkDesc:""酸性高密度污泥罐石灰投加阀无异常""}],
          ""checkResultData"" : {
                        ""clientID"": ""GLASS_1293124"", // 设备号
                        ""gaomiguan"":{
                            ""deviceID"":""000101"", // 巡检项编号（高密管）
                            ""name"": ""gaomiguan"",// 巡检项英文名（高密管）
                            ""valveScale"": ""0"", // 加药投加阀开度
                            ""blender1"": ""0"", // 搅拌器状态开关1
                            ""blender2"": ""0"", // 搅拌器状态开关2
                            ""blender3"": ""0"", // 搅拌器状态开关3
                            ""blender4"": ""0"" // 搅拌器状态开关4
                        },
                        ""zhonghecao1"":{
                            ""deviceID"":""000102"", // 巡检项编号（一级中和槽）
                            ""name"": ""zhonghecao1"",// 巡检项英文名（一级中和槽）
                            ""PH"": ""2"", // PH值
                            ""PHLowerLimit"": ""6.7"", // PH值下限
                            ""PHUpperLimit"": ""7.3"", // PH值上限
                            ""blender1"": ""0"", // 搅拌器状态开关1
                            ""blender2"": ""0"", // 搅拌器状态开关2
                            ""blender3"": ""0"", // 搅拌器状态开关3
                            ""blender4"": ""0"" // 搅拌器状态开关4
                        },
                        ""zhonghecao2A"":{
                            ""deviceID"":""000103"", // 巡检项编号（二级中和槽A）
                            ""name"": ""zhonghecao2A"",// 巡检项英文名(二级中和槽A)
                            ""PH"": ""2"", // PH值
                            ""PHLowerLimit"": ""6.7"", // PH值下限
                            ""PHUpperLimit"": ""7.3"", // PH值上限
                            ""blender1"": ""0"", // 搅拌器状态开关1
                            ""blender2"": ""0"", // 搅拌器状态开关2
                            ""blender3"": ""0"", // 搅拌器状态开关3
                            ""blender4"": ""0"", // 搅拌器状态开关4
                            ""valveScale"": ""0"" // 投加阀开度
                        },
                        ""zhonghecao2B"":{
                            ""deviceID"":""000104"", // 巡检项编号（二级中和槽B）
                            ""name"": ""zhonghecao2B"",// 巡检项英文名(二级中和槽B)
                            ""PH"": ""2"", // PH值
                            ""PHLowerLimit"": ""6.7"", // PH值下限
                            ""PHUpperLimit"": ""7.3"", // PH值上限
                            ""blender1"": ""0"", // 搅拌器状态开关1
                            ""blender2"": ""0"", // 搅拌器状态开关2
                            ""blender3"": ""0"", // 搅拌器状态开关3
                            ""blender4"": ""0"", // 搅拌器状态开关4
                            ""valveScale"": ""0"" // 投加阀开度
                        }
                    }
}";
	#endregion

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

