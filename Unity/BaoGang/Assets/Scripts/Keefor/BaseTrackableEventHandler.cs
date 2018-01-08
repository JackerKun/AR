
using UnityEngine;
using Vuforia;

/*
*Create By Keefor On 12/22/2017
*/

public class BaseTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    private bool isfound = true;
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

    protected void DisableRender()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        foreach (var component in rendererComponents)
            component.enabled = false;
    }

    protected void DisableCollider()
    {
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        foreach (var component in colliderComponents)
            component.enabled = false;
    }

    protected virtual void OnTrackingFound()
    {
        Debug.Log("Trackable " + trackName + " Found");

    }

    protected void EnableRender()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;
    }

    protected void EnableCollider()
    {
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

    }

}
