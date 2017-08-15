using UnityEngine;
using System.Collections;
using HopeRun;
using AR.Model;

namespace Vuforia
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>
	public class TankTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
	{
		#region PRIVATE_MEMBER_VARIABLES

		private TrackableBehaviour mTrackableBehaviour;
		private VuMarkBehaviour mVuMarkBehaviour;

		#endregion // PRIVATE_MEMBER_VARIABLES



		#region UNTIY_MONOBEHAVIOUR_METHODS

		void Start ()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour> ();
			if (mTrackableBehaviour) {
				mTrackableBehaviour.RegisterTrackableEventHandler (this);
			}
			mVuMarkBehaviour = GetComponent<VuMarkBehaviour> ();
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
			Debug.Log ("Trackable " + mTrackableBehaviour.TrackableName + " found");
			InitDrumObj ();
			StartCoroutine (UpdateData ());
		}

		//获取网络数据
		void InitDrumObj ()
		{
			Debug.Log ("init socket");
			//获取识别物体的ID
			string _id = mVuMarkBehaviour.VuMarkTarget.InstanceId.StringValue;
			GlobalManager.CURRENT_DRUM = GlobalManager.InitDrumPanel (_id);
			GlobalManager.CURRENT_DRUM.InitUI ();
			WebManager.Instance.onScaning (_id, scanCallback);
		}

		void scanCallback (Tank data)
		{
			UIManager.ChangeValveState (data.IsValveOpen ? ValveState.ON : ValveState.OFF);
			UIManager.UpdateLiquidHeight (data.LiquidLevel);
			Debug.Log ("scanCallback:" + data);
		}

		//更新数据
		IEnumerator UpdateData ()
		{
			yield return 0;
		}

		private void OnTrackingLost ()
		{
			//获取识别物体的ID
			if (mVuMarkBehaviour && mVuMarkBehaviour.VuMarkTarget != null) {
				string _id = mVuMarkBehaviour.VuMarkTarget.InstanceId.StringValue;
				WebManager.Instance.onLostScaning (_id);
			}
			GlobalManager.Deposit (GlobalManager.CURRENT_DRUM);

			Debug.Log ("Trackable " + mTrackableBehaviour.TrackableName + " lost");
		}

		#endregion // PRIVATE_METHODS
	}
}
