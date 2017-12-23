using UnityEngine;
using HopeRun;
using AR.Model;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class TankTrackableEventHandler : BaseTrackableEventHandler
    {
        private TankObj UIView3D;
        private TankObj prefab;
        protected override void Init()
        {
            base.Init();
            prefab = ((GameObject) Resources.Load("Prefabs/TankPanel")).GetComponent<TankObj>();
        }

        protected override void OnTrackingFound()
        {
            base.OnTrackingFound();
            GlobalManager.CURRENT_TANKID = trackName;
            TankSocketService.Instance.onLostScaning();
            TankSocketService.Instance.onScaning(ScanCallback);
        }
        protected override void OnTrackingLost()
        {
            base.OnTrackingLost();
            if (UIView3D != null) UIView3D.Deposit();
        }

        void ScanCallback(Tank data)
        {

            if (!data.chemicalId.Equals(trackName))
            {
                Debug.Log("当前ID" + trackName + ":正确" + data.chemicalId);
                return;
            }

            var gfview = GyroFlowView.Inst;
            gfview.gameObject.SetActive(false);


            if (isFound)
            {
                Debug.Log("更新3DUI数据");
                if (UIView3D == null)
                {
                    var canvasRoot = transform.Find("Root");
                    UIView3D = Instantiate(prefab, canvasRoot);
                    UIView3D.InitParam(trackName);
                    UIView3D.InitUI();
                }
                //识别图跟随显示
                UIView3D.ChangeValveState(data.valveStatus ? ValveState.ON : ValveState.OFF);
                UIView3D.UpdateHeight(data.liquidHeight, data.limitLevel);
            }
            else
            {
                //屏幕中心出现东西
                if (GlobalManager.IS_WORKFLOW)
                {
                    Debug.Log("更新2DUI数据");
                    //显示常驻液位页面
                    gfview.gameObject.SetActive(true);
                    //更新漂浮液位页面的高度
                    gfview.UpdateFloatingPanel(data.liquidHeight, data.limitLevel);
                }
            }
        }


    }
}
