using System;
using DG.Tweening;
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
    private Vector3 initScale;
    private Vector3 targetScale;

    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        initScale = transform.localScale;
        targetScale = initScale * .8f;
    }

    public bool IsOver
    {
        get { return m_IsOver; }
    }

    public virtual void MouseHover()
    {
        if(m_IsOver)return;
        m_IsOver = true;
        initScale = transform.localScale;
        transform.DOScale(targetScale, .2f).SetEase(Ease.OutSine);
        //Debug.Log("鼠标悬停");
        if (OnOver != null)
            OnOver();
    }

    public virtual void MouseSelect()
    {
        //Debug.Log("鼠标选中");
        if (OnClick != null)
            OnClick();
    }

    public virtual void MouseExit()
    {
        transform.DOScale(initScale, .2f).SetEase(Ease.OutSine);
        m_IsOver = false;
        //Debug.Log("鼠标离开");
        if (OnOut != null)
            OnOut();
    }
}
