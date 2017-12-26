/* 
 * 10/17/2017 Create By Keefor
 * 针对返回按钮，进行输入扩展
 * 包括单击返回按钮，双击返回按钮
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInput : MonoBehaviour
{
    private static bool isReady = true;

    private static KeyInput inst;
    public static KeyInput Inst
    {
        get
        {
            if (inst == null)
            {
                inst = new GameObject().AddComponent<KeyInput>();
                inst.hideFlags = HideFlags.HideInInspector;
            }
            return inst;
        }

    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (key != -1 )
            StartCoroutine(ResetReady());
    }

    private IEnumerator ResetReady()
    {
        yield return new WaitForEndOfFrame();
        //isReady = true;
        key = -1;
    }

    private float timer = 0;
    public const int OncePress = 1;
    public const int DoublePress = 2;
    private const int ReadyPress = 0;
    private int PressState;

    private int key;

    public int GetKey()
    {
        if (key != -1)
            return key;
        int curPress = 0;
        switch (PressState)
        {
            case OncePress:
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    curPress = OncePress;
                    PressState = ReadyPress;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        curPress = DoublePress;
                        PressState = ReadyPress;
                    }
                }
                break;
            case ReadyPress:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PressState = OncePress;
                    timer = 0.25f;
                }
                break;
        }
        key = curPress;
        return curPress;
    }

}
