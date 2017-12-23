using UnityEngine;

/**
 * Create By Keefor On 11/05/2017
 */

namespace glasslive
{
#if UNITY_ANDROID

	public class OnFrameBeginHandler : MonoBehaviour {

		void OnPreRender() {
			GlassLiveImpl.OnPreRender();
		}

	}

#endif
}