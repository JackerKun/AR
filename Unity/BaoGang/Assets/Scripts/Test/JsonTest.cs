using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class JsonTest : MonoBehaviour
{
	#region json
	string myJSON = @"{
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
	void Start()
	{
		Debug.Log(JSON.Parse(myJSON));
		Debug.Log("Json is null?" + JSON.Parse(myJSON).IsNull);
	}
}
