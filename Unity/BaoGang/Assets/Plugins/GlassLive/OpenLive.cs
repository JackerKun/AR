using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(glasslive.GlassLive))]
public class OpenLive : MonoBehaviour
{

	public string liveurl = "rtmp://192.168.120.86:1935/live/test";
	// Use this for initialization
	IEnumerator Start()
	{
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
