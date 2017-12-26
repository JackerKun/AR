using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Config
{
#if UNITY_EDITOR_OSX 
    public const string ConfigPath = "User/ArConfig.txt";
#elif UNITY_EDITOR
    public const string ConfigPath = "F:/ARConfig.txt";
#elif UNITY_ANDROID
    public const string ConfigPath = "/storage/emulated/0/ARConfig.txt";
#endif


#if UNITY_EDITOR_OSX 
    public const string EyePath = "User/eyeConfig.txt";
#elif UNITY_EDITOR
    public const string EyePath = "F:/eyeConfig.txt";
#elif UNITY_ANDROID
    public const string EyePath = "/storage/emulated/0/eyeConfig.txt";
#endif
}
