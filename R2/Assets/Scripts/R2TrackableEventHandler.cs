/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;

namespace Vuforia
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>
	public class R2TrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
	{
		#region PRIVATE_MEMBER_VARIABLES

		AnchorPoint[] anchorList;
		GameObject trackingObj;
		private TrackableBehaviour mTrackableBehaviour;

		#endregion // PRIVATE_MEMBER_VARIABLES

		void Awake ()
		{
			trackingObj = GameObject.Find ("Canvas/TrackingObj");
		}

		#region UNTIY_MONOBEHAVIOUR_METHODS

		void Start ()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour> ();
			anchorList = GetComponentsInChildren<AnchorPoint> ();
			if (mTrackableBehaviour) {
				mTrackableBehaviour.RegisterTrackableEventHandler (this);
			}
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
			if (trackingObj) {
				trackingObj.SetActive (true);
				RandomValue[] rvs = trackingObj.GetComponentsInChildren<RandomValue> ();
				foreach (RandomValue v in rvs) {
					v.PlayValue ();
				}
				foreach (var v in anchorList) {
					v.StartTracking ();
				}
			}
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer> (true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider> (true);

			// Enable rendering:
			foreach (Renderer component in rendererComponents) {
				component.enabled = true;
			}

			// Enable colliders:
			foreach (Collider component in colliderComponents) {
				component.enabled = true;
			}

			Debug.Log ("Trackable " + mTrackableBehaviour.TrackableName + " found");
		}


		private void OnTrackingLost ()
		{
			if (trackingObj) {
				RandomValue[] rvs = trackingObj.GetComponentsInChildren<RandomValue> ();
				foreach (RandomValue v in rvs) {
					v.StopValue ();
				}
				foreach (var v in anchorList) {
					v.StopTracking ();
				}
				trackingObj.SetActive (false);
			}
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer> (true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider> (true);

			// Disable rendering:
			foreach (Renderer component in rendererComponents) {
				component.enabled = false;
			}

			// Disable colliders:
			foreach (Collider component in colliderComponents) {
				component.enabled = false;
			}

			Debug.Log ("Trackable " + mTrackableBehaviour.TrackableName + " lost");
		}

		#endregion // PRIVATE_METHODS
	}
}
