using UnityEngine;
using System.Collections;
using System.IO;

/**
 * Create By Keefor On 11/05/2017
 */

namespace glasslive
{
#if UNITY_ANDROID

	public class OnFrameEndHandler : MonoBehaviour {

		IEnumerator OnPostRender() {
			yield return new WaitForEndOfFrame();
		#if (!UNITY_5_6_OR_NEWER)
			if (ShareRECImpl.IsRecordGUILayer()) {
				ShareRECImpl.OnPostRender();
			}
		#endif
		}

		void OnRenderImage(RenderTexture src, RenderTexture dest) {
			GlassLiveImpl.AddCameraRecord(src);
		#if (UNITY_5_6_OR_NEWER)
			GlassLiveImpl.OnPostRender();
		#else
			if (!ShareRECImpl.IsRecordGUILayer()) {
				ShareRECImpl.OnPostRender();
			}
		#endif
		}
	}

#endif
}