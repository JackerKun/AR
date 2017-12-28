using System.Collections;
using UnityEngine;
using HopeRun;
[RequireComponent(typeof(glasslive.GlassLive))]
public class OpenLive : MonoBehaviour
{

	public string liveurl = "rtmp://192.168.120.86:1935/live/test";
	// Use this for initialization
	IEnumerator Start()
	{
		liveurl = "rtmp://" + GlobalManager.IP + ":1935/live/test";
		yield return new WaitForSeconds(0.1f);
		glasslive.GlassLive.SetLiveURL(liveurl);
		glasslive.GlassLive.UseGLES30API();
		glasslive.GlassLive.StartRecorder();
	}

	void OnDestroy()
	{
		glasslive.GlassLive.StopRecorder();
	}
}
