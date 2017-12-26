
using UnityEngine;
using Vuforia;

/*
*Create By Keefor On 12/22/2017
*/

public class BaseTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    private bool isfound;
    protected bool isFound
    {
        get { return isfound; }
    }

    protected string trackName
    {
        get { return mTrackableBehaviour.TrackableName; }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            if (isfound) return;
            isfound = true;
            OnTrackingFound();
        }
        else
        {
            if (!isfound) return;
            isfound = false;
            OnTrackingLost();
        }
    }

    void Awake()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        Init();
    }

    protected virtual void Init()
    {
    }

    protected virtual void OnTrackingLost()
    {
        Debug.Log("Trackable " + trackName + " Lost");
    }

    protected virtual void OnTrackingFound()
    {
        Debug.Log("Trackable " + trackName + " Found");
    }
}
