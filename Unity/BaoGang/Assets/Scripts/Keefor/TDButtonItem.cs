using System;
using UnityEngine;

/*
*Create By Keefor On 12/19/2017
*/

public class TDButtonItem : MonoBehaviour
{
    public event Action OnOver;
    public event Action OnOut;
    public event Action OnClick;

    protected bool m_IsOver;
    public bool IsOver
    {
        get { return m_IsOver; }
    }

    public void MouseHover()
    {
        m_IsOver = true;
        Debug.Log("鼠标悬停");
        if (OnOver != null)
            OnOver();
    }

    public void MouseSelect()
    {
        m_IsOver = false;
        Debug.Log("鼠标选中");
        if (OnClick != null)
            OnClick();
    }

    public void MouseExit()
    {
        Debug.Log("鼠标离开");
        if (OnOut != null)
            OnOut();
    }
}
