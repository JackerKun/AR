using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Create by keefor on 12/11/2017
 * ~~~~~~~~~~~~~~~~~~~~~~~~~~~
*/
using UnityEngine.Profiling;


/// <summary>
/// FPS 及内存管理
/// </summary>
public class FPSMono : MonoBehaviour
{
    private Texture2D txt_bg;
    private int txt_w = 500, txt_h = 300;

    private AndroidJavaObject currentActivity;
    void Start()
    {
#if UNITY_ANDROID&&!UNITY_EDITOR
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
#endif

        timeleft = updateInterval;
        txt_bg = new Texture2D(txt_w, txt_h);
        Color[] tdc = new Color[txt_w * txt_h];
        for (int i = 0; i < txt_w * txt_h; i++)
        {
            //所用像素点颜色应相同
            tdc[i] = Color.white;
        }
        txt_bg.SetPixels(0, 0, txt_w, txt_h, tdc);
    }
    void Update()
    {
        UpdateUsed();
        UpdateFPS();
    }
    //Memory
    private string sUserMemory;
    private string s;
    public bool OnMemoryGUI;
    private uint MonoUsedM;
    private uint AllMemory;
    [Range(0, 100)]
    public int MaxMonoUsedM = 50;
    [Range(0, 400)]
    public int MaxAllMemory = 200;
    void UpdateUsed()
    {
        sUserMemory = "";



#if UNITY_EDITOR
        MonoUsedM = (uint)(Profiler.GetMonoUsedSize() / m_KBSize);
        AllMemory = (uint)(Profiler.GetTotalAllocatedMemory() / m_KBSize);

        sUserMemory += "MonoUsed:" + MonoUsedM + "M\nAllMemory:" + AllMemory + "M\n";
#elif UNITY_ANDROID
        using (AndroidJavaClass jc = new AndroidJavaClass("com.keefor.tool.DebugTools"))
        {

            AllMemory = (uint)(jc.CallStatic<long>("getTotalMemory")/1024);
            MonoUsedM = (uint)(jc.CallStatic<long>("getUsedMemory")/1024);
            sUserMemory += "TotalMemory:" + AllMemory + "K\nUseMemory:" + MonoUsedM + "K\n";
        }
#endif
        sUserMemory += "UnUsedReserved:" + Profiler.GetTotalUnusedReservedMemory() / m_KBSize + "M" + "\n";

    }


    //FPS
    float updateInterval = 0.5f;
    private float accum = 0.0f;
    private float frames = 0;
    private float timeleft;
    private float fps;
    private string FPSAAA;
    [Range(0, 150)]
    public int MaxFPS;
    void UpdateFPS()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;


        if (timeleft <= 0.0)
        {
            fps = accum / frames;
            FPSAAA = "FPS: " + fps.ToString("f2");
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
    void OnGUI()
    {
        if (OnMemoryGUI)
        {
            int height = 300;
            var myStyle = new GUIStyle { fontSize = 40 };
            GUI.DrawTexture(new Rect(0, 0 + height, txt_w, txt_h), txt_bg);
            GUI.color = new Color(1, 0, 0);
            GUI.Label(new Rect(10, 10 + height, 450, 120), sUserMemory, myStyle);
            GUI.Label(new Rect(10, 130 + height, 400, 40), FPSAAA, myStyle);
            if (MonoUsedM > MaxMonoUsedM)
            {
                GUI.backgroundColor = new Color(1, 0, 0);
                GUI.Button(new Rect(0, 170 + height, 400, 40), "MonoUsedM Waming!!内存不足", myStyle);
            }
            if (AllMemory > MaxAllMemory)
            {
                GUI.backgroundColor = new Color(1, 0, 1);
                GUI.Button(new Rect(0, 210 + height, 400, 40), "AllMemory Waming!!内存堪忧", myStyle);
            }
            if (fps > MaxFPS)
            {
                GUI.backgroundColor = new Color(1, 0.4f, 0.5f);
                GUI.Button(new Rect(0, 250 + height, 400, 40), "FPS Waming!!", myStyle);
            }
        }


    }

    public const float m_KBSize = 1024.0f * 1024.0f;


    /////FPS简便算法
    //  void UpDateFPS()
    //        {
    //            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;//完成最后一帧的时间秒-上一帧时间时间==这一帧时间秒 /1000=毫秒
    //            PFpsStr = (1.0f / deltaTime).ToString("f2"); //每帧ms的倒数=频率
    //        }
}

////FPS官方写法
// private void Update()
//        {
//            // measure average frames per second
//            m_FpsAccumulator++;
//            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
//            {
//                m_CurrentFps = (int) (m_FpsAccumulator/fpsMeasurePeriod);
//                m_FpsAccumulator = 0;
//                m_FpsNextPeriod += fpsMeasurePeriod;
//                m_Text.text = string.Format(display, m_CurrentFps);
//            }
//        }

