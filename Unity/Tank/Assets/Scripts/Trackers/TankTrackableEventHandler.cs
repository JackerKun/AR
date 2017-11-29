using UnityEngine;
using System.Collections;
using HopeRun;
using AR.Model;
using BestHTTP;
using BestHTTP.SocketIO;
using SimpleJSON;
using UnityEngine.UI;

namespace Vuforia
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>
	public class TankTrackableEventHandler : MonoBehaviour,
												ITrackableEventHandler
	{
		#region PRIVATE_MEMBER_VARIABLES

		Text floatingText;
		RawImage floatingBG;

		private TrackableBehaviour mTrackableBehaviour;
		//		private VuMarkBehaviour mVuMarkBehaviour;
		private bool isTargetFound = false;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region UNTIY_MONOBEHAVIOUR_METHODS

		void Start ()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour> ();
			if (mTrackableBehaviour) {
				mTrackableBehaviour.RegisterTrackableEventHandler (this);
			}
			isTargetFound = false;
			floatingText = GyroFlowView.text;
			floatingBG = GyroFlowView.colorImg;
			//			mVuMarkBehaviour = GetComponent<VuMarkBehaviour> ();
		}

		#endregion // UNTIY_MONOBEHAVIOUR_METHODS



		#region PUBLIC_METHODS

		/// <summary>
		/// Implementation of the ITrackableEventHandler function called when the
		/// tracking state changes.
		/// </summary>
		public void OnTrackableStateChanged (
			TrackableBehaviour.Status previousStatus,
			TrackableBehaviour.Status newStatus)
		{
			if (newStatus == TrackableBehaviour.Status.DETECTED ||
			    newStatus == TrackableBehaviour.Status.TRACKED ||
			    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
				OnTrackingFound ();
			} else {
				OnTrackingLost ();
			}
		}

		#endregion // PUBLIC_METHODS



		#region PRIVATE_METHODS

		private void OnTrackingFound ()
		{
			Debug.Log ("target found");
			//隐藏常驻液位页面
			isTargetFound = true;
			Debug.Log ("Trackable " + mTrackableBehaviour.TrackableName + " found");
			InitDrumObj ();
			StartCoroutine (UpdateData ());
		}

		//获取网络数据
		void InitDrumObj ()
		{
			Debug.Log ("init socket");
			//获取识别物体的ID
			//			string _id = mVuMarkBehaviour.VuMarkTarget.InstanceId.StringValue;
			GlobalManager.CURRENT_TANKID = mTrackableBehaviour.TrackableName;
			TankSocketService.Instance.onScaning (ScanCallback);
		}

		void ScanCallback (Tank data)
		{
			UIManager.ChangeValveState (data.valveStatus ? ValveState.ON : ValveState.OFF);
			UIManager.UpdateLiquidHeight (data.liquidHeight, data.limitLevel);
			if (GlobalManager.CURRENT_TANK == null && isTargetFound) {
				GlobalManager.CURRENT_TANK = GlobalManager.InitTankPanel (transform.Find ("Root"));
				GlobalManager.CURRENT_TANK.InitUI ();
			}
			//离开识别图后，判断是否显示常驻液位高度
			if (isTargetFound) {
				//隐藏常驻液位页面
				GyroFlowView.SetFlowPanelActive (false);
			} else {
				if (GlobalManager.IS_WORKFLOW) {
					//显示常驻液位页面
					GyroFlowView.SetFlowPanelActive (true);
					//更新漂浮液位页面的高度
					UpdateFloatingPanel (data.liquidHeight, data.limitLevel);
				}
			}
			Debug.Log ("scanCallback..");
		}

		void UpdateFloatingPanel (float liquidH, float limitLevel)
		{
			if (floatingText && floatingBG) {
				floatingText.text = string.Format ("{0:00.00}%", liquidH);
				floatingBG.color = GlobalManager.GetWarnColor (liquidH / limitLevel);
			}
		}

		//更新数据
		IEnumerator UpdateData ()
		{
			yield return 0;
		}

		private void OnTrackingLost ()
		{
			isTargetFound = false;
			//获取识别物体的ID
			if (mTrackableBehaviour) {
				TankSocketService.Instance.onLostScaning (mTrackableBehaviour.TrackableName);
			}

			GlobalManager.Deposit (GlobalManager.CURRENT_TANK);

			Debug.Log ("Trackable " + mTrackableBehaviour.TrackableName + " lost");
		}

		#endregion // PRIVATE_METHODS
	}
}
