using UnityEngine;
using System;
using System.Collections;

/**
 * Create By Keefor On 11/05/2017
 */

namespace glasslive {
	public class GlassLive : MonoBehaviour {
	#if UNITY_ANDROID
		private const int STATE_IDLE = 0;
		private const int STATE_STARTING = 1;
		private const int STATE_STARTED = 2;
		private const int STATE_PAUSING = 3;
		private const int STATE_PAUSED = 4;
		private const int STATE_RESUMING = 5;
		private const int STATE_RESUMED = STATE_STARTED;
		private const int STATE_STOPPING = 7;
		private const int STATE_STOPPED = STATE_IDLE;

		private const int RETURN_FROM_SHARE = -100;
		private const int STATE_LIVE_STRT = 101;//录屏过程中直播开启
	    private const int STATE_LIVE_STOP = 102;//录屏过程中直播中断

		public string AppKey = "moba6b6c6d6";
		public string AppSecret = "b89d2427a3bc7ad1aea1e1e8c1d36bf3";
		public LevelMaxFrameSize MaxFrameSize = LevelMaxFrameSize.LEVEL_1280_720;
		public LevelVideoQuality VideoQuality = LevelVideoQuality.LEVEL_HIGH;
		public int MinDuration = 4;
		public bool RecordAudioFromMic = true;
		public bool SoftwareAudioEncoder = false;
		public bool SoftwareVideoEncoder = false;
		#if (!UNITY_5_6_OR_NEWER)
		public bool RecordGUILayer = true;
		#endif
		public string CacheFolder = null;

		private static OnFrameBeginHandler beginHanlder;
		private static OnFrameEndHandler endHanlder;
		private int curAction;

		public static OnRecorderStarting OnRecorderStartingHandler;
		public static OnRecorderStarted OnRecorderStartedHandler;
		public static OnRecorderPausing OnRecorderPausingHandler;
		public static OnRecorderPaused OnRecorderPausedHandler;
		public static OnRecorderResuming OnRecorderResumingHandler;
		public static OnRecorderResumed OnRecorderResumedHandler;
		public static OnRecorderStopping OnRecorderStoppingHandler;
		public static OnRecorderStopped OnRecorderStoppedHandler;
		
		public static OnReturnFromShare OnReturnFromShareHandler;



        void Awake() {
			try {
				GlassLiveImpl.Init(AppKey, AppSecret, gameObject.name, (int) MaxFrameSize);
				GlassLiveImpl.SetVideoQuality((int) VideoQuality);
				GlassLiveImpl.SetForceSoftwareEncoding(SoftwareVideoEncoder, SoftwareAudioEncoder);
				GlassLiveImpl.SetCacheFolder(CacheFolder);
				GlassLiveImpl.SetMinDuration(MinDuration);
                
				if (RecordAudioFromMic) {
					GlassLiveImpl.SetRecordAudioFromMic();
				}
			#if (!UNITY_5_6_OR_NEWER)
				if (RecordGUILayer) {
					ShareRECImpl.SetRecordGUILayer();
				}
			#endif
			} catch (Exception e) {}
			InitializeFrontMostCamera();
			InitializeBackMostCamera();
		}

		private void InitializeFrontMostCamera() {
			if (GameObject.Find("FrontMostCamera") == null) {
				GameObject cameraObject = new GameObject();
				Camera camera = cameraObject.AddComponent<Camera>();
				camera.name = "FrontMostCamera";
				camera.clearFlags = CameraClearFlags.Nothing;
				camera.cullingMask = 0;
				camera.depth = Single.MinValue;
				beginHanlder = camera.gameObject.AddComponent<OnFrameBeginHandler>();
				beginHanlder.enabled = false;
				cameraObject.SetActive(true);
				DontDestroyOnLoad(cameraObject);
			}
		}
		
		private void InitializeBackMostCamera() {
			if (GameObject.Find("BackMostCamera") == null) {
				GameObject cameraObject = new GameObject();
				Camera camera = cameraObject.AddComponent<Camera>();
				camera.name = "BackMostCamera";
				camera.clearFlags = CameraClearFlags.Nothing;
				camera.cullingMask = 0;
			    camera.depth = 10;//Single.MaxValue;
				endHanlder = camera.gameObject.AddComponent<OnFrameEndHandler>();
				endHanlder.enabled = false;
				cameraObject.SetActive(true);
				DontDestroyOnLoad(cameraObject);
			}
		}

		private void setUnityRenderEvent(string eventID) {
			#if (UNITY_4_5 || UNITY_4_6)
				int[] iparameters = new int[1]{0};
				string[] parameters = eventID.Split('|');
				int i = 0; 
				
				foreach (string parameter in parameters){
					if (!Int32.TryParse(parameter, out iparameters[i])) {
						return;
					}
					i++;
				}
				ShareRECImpl.setFBOInfo(iparameters[0]);
			#endif
		}
		
		private void onStateChange(string action) {
			int iAction = -1;
			if (!Int32.TryParse(action, out iAction)) {
				return;
			}
			
			switch (iAction) {
				case STATE_STARTING: {
					if (OnRecorderStartingHandler != null) {
						OnRecorderStartingHandler();
					}
				} break;
				case STATE_STARTED: {
					if (curAction == STATE_RESUMING) {
						if (OnRecorderResumedHandler != null) {
							OnRecorderResumedHandler();
						}
					} else if (OnRecorderStartedHandler != null) {
						OnRecorderStartedHandler();
					}
				} break;
				case STATE_PAUSING: {
					if (OnRecorderPausingHandler != null) {
						OnRecorderPausingHandler();
					}
				} break;
				case STATE_PAUSED: {
					if (OnRecorderPausedHandler != null) {
						OnRecorderPausedHandler();
					}
				} break;
				case STATE_RESUMING: {
					if (OnRecorderResumingHandler != null) {
						OnRecorderResumingHandler();
					}
				} break;
				case STATE_STOPPING: {
					if (OnRecorderStoppingHandler != null) {
						OnRecorderStoppingHandler();
					}
				} break;
				case STATE_STOPPED: {
					beginHanlder.enabled = false;
					endHanlder.enabled = false;
					GlassLiveImpl.ReleaseRenderTexture();
					if (OnRecorderStoppedHandler != null) {
						OnRecorderStoppedHandler();
					}
				} break;
                case STATE_LIVE_STRT:
                    break;
                case STATE_LIVE_STOP:
			        break;
			}
			
			curAction = iAction;
		}


		
		private void onReturnGame(string action) {
			int iAction = 0;
			if (!Int32.TryParse(action, out iAction)) {
				return;
			}
			
			switch (iAction) {
				case RETURN_FROM_SHARE: {
					if (OnReturnFromShareHandler != null) {
						OnReturnFromShareHandler();
					}
				} break;
			}
		}



        /// <summary>
        /// 此方法在录制模块启动时被调用(This method will be called when the recorder module is starting.)
        /// </summary>
        public delegate void OnRecorderStarting();
		
		/// <summary>
		/// 此方法在录制模块启动后被调用(This method will be called when the recorder module is started.)
		/// </summary>
		public delegate void OnRecorderStarted();
		
		/// <summary>
		/// 此方法在录制模块暂停时被调用(This method will be called when the recorder module is pausing.)
		/// </summary>
		public delegate void OnRecorderPausing();
		
		/// <summary>
		/// 此方法在录制模块暂停后被调用(This method will be called when the recorder module is paused.)
		/// </summary>
		public delegate void OnRecorderPaused();
		
		/// <summary>
		/// 此方法在录制模块恢复时被调用(This method will be called when the recorder module is resuming.)
		/// </summary>
		public delegate void OnRecorderResuming();
		
		/// <summary>
		/// 此方法在录制模块恢复后被调用(This method will be called when the recorder module is resumed.)
		/// </summary>
		public delegate void OnRecorderResumed();
		
		/// <summary>
		/// 此方法在录制模块停止时被调用(This method will be called when the recorder module is stopping.)
		/// </summary>
		public delegate void OnRecorderStopping();
		
		/// <summary>
		/// 此方法在录制模块停止后被调用(This method will be called when the recorder module is stopped.)
		/// </summary>
		public delegate void OnRecorderStopped();
		
		/// <summary>
		/// 此方法在分享页面关闭后调用(This method will be called after the Sharing Page is closed)
		/// </summary>
		public delegate void OnReturnFromShare();
		


		/// <summary>
		/// 启用GLES30
		/// </summary>
		public static void UseGLES30API() {
			GlassLiveImpl.useGLES30API();
		}


        public static void SetLiveURL(string url)
        {
            GlassLiveImpl.SetCacheFolder(url);
        }
        // =======================================

		
		/// <summary>
		/// 添加视频的自定义属性(Adds the custom attributes of the video.)
		/// </summary>
		public static void AddCustomAttr(string key, string value) {
			GlassLiveImpl.AddCustomAttr(key, value);
		}

		// =======================================

		/// <summary>
		/// 判断ShareRec是否支持当前的设备(Determines whether ShareRec is available for the current device.)
		/// </summary>
		public static bool IsAvailable() {
			return GlassLiveImpl.IsAvailable();
		}

		/// <summary>
		/// 启动录制模块 (Start the recorder module)
		/// </summary>
		public static void StartRecorder() {
			if (GlassLiveImpl.CanStart()) {
				beginHanlder.enabled = true;
				endHanlder.enabled = true;

				GlassLiveImpl.InitRenderTexture();
				GlassLiveImpl.Start();
			}
		}

		/// <summary>
		/// 暂停录制模块(Pauses the recorder module)
		/// </summary>
		public static void PauseRecorder() {
			GlassLiveImpl.Pause();
		}

		/// <summary>
		/// 恢复录制(Resumes the recorder module)
		/// </summary>
		public static void ResumeRecorder() {
			GlassLiveImpl.Resume();
		}

		/// <summary>
		/// 停止录制模块 (Stop the recorder module)
		/// </summary>
		public static void StopRecorder() {
			GlassLiveImpl.Stop();
		}


		/// <summary>
		/// 通过缓存的视频ID获取本地路径(Gets the local video path by its ID.)
		/// </summary>
		public static string GetLocalVideoPath(long videoId) {
			return GlassLiveImpl.GetLocalVideoPath(videoId);
		}


		/// <summary>
		/// 添加要录屏的cmaera(add record camera.)
		/// </summary>
		public static void addCameraRecord(RenderTexture src) {
			GlassLiveImpl.AddCameraRecord (src);
		}

		/// <summary>
		/// 不使用默认的record进行录音，使用自定义的录音方式录音  Prepares the sound copying.
		/// 注意，需要再 执行 StartRecorder（）方法之前调用，否则无效
		/// </summary>
		public static void PrepareSoundCopying(int channelCount, int sampleRate, int maxBufferSizeInBytes){
			GlassLiveImpl.PrepareSoundCopying (channelCount,sampleRate, maxBufferSizeInBytes);
		}


		/// <summary>
		/// 将录音得到的 数据添加到录制的视频数据里 Offers the sample.
		/// </summary>
		public static void OfferSample(byte[] sample, int offset, int len){
			GlassLiveImpl.OfferSample (sample,offset,len);
		}

	#endif
	}
}
