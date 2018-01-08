using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/*
 * Create By Keefor On 12/19/2017
 */

/// <summary>
/// 3D 鼠标输入
/// </summary>
public class TDMouseInput : MonoBehaviour
{
    private static TDMouseInput tdInput;

    public static TDMouseInput inst
    {
        get
        {
            if (tdInput == null)
                tdInput = FindObjectOfType<TDMouseInput>();
            if (tdInput == null)
                tdInput = new GameObject("TDInput").AddComponent<TDMouseInput>();
            return tdInput;
        }
    }

    Camera _mCamera;
    Vector2 screenCenter;

    GameObject mouseIcon2D;
    Transform mouseIcon3D;
    Image mouseCountDown;

    bool isMouse2D;

    private bool invisible;

    void Awake()
    {
        var obj = Resources.Load<GameObject>("Mouse2D");
        mouseIcon2D = Instantiate(obj).transform.GetChild(0).gameObject;
        obj = Resources.Load<GameObject>("Mouse3D");
        obj = Instantiate(obj);
        mouseIcon3D = obj.transform.Find("Mouse").transform;
        mouseCountDown = mouseIcon3D.Find("MouseCountDown").GetComponent<Image>();
        screenCenter = new Vector2(Screen.width >> 1, Screen.height >> 1);
        _mCamera = GetComponent<Camera>() ?? GetComponentInChildren<Camera>();
        if (_mCamera == null)
            throw new Exception("TDMouseInput mCamera is null");
    }


    // Update is called once per frame
    void Update()
    {
        //开始检测交互
        UpdateInteractive();
    }

    void OnDestroy()
    {
        tdInput = null;
    }

    #region UI的交互操作

    TDButtonItem curBtn;

    void UpdateInteractive()
    {
        RaycastHit hit;
        if (Physics.Raycast(_mCamera.ScreenPointToRay(screenCenter), out hit))
        {
            Debug.DrawLine(Camera.main.transform.position, hit.point);
            if (hit.collider != null)
            {
                //显示3D鼠标
                if (!invisible)
                    SetMouse2D(false);
                mouseIcon3D.transform.position = hit.point;
                mouseIcon3D.forward = -hit.normal;
                TDButtonItem tmpBtn = hit.transform.GetComponent<TDButtonItem>();
                if (tmpBtn != null)
                {
                    //同一个按钮不作操作
                    if (curBtn == tmpBtn)
                    {
                        return;
                    }
                    //从一个按钮射到另一个按钮
                    if (curBtn != null && curBtn != tmpBtn)
                    {
                        //上一个按钮执行退出操作
                        curBtn.MouseExit();
                        //隐藏鼠标倒计时
                        StopFillImage();
                    }
                    curBtn = tmpBtn;
                    //鼠标经过操作
                    curBtn.MouseHover();
                    if (!invisible)
                    {
                        var dis = Vector3.Distance(_mCamera.transform.position, hit.point);
                        ShowMouseCountDown(tmpBtn.MouseSelect, dis);
                    }

                }
                else
                {
                    ClearBtnHover();
                }
            }
            else
            {
                ClearBtnHover();
            }
        }
        //显示鼠标
        else
        {
            ClearBtnHover();
        }
    }

    //显示鼠标倒计时
    void ShowMouseCountDown(Action act, float dis)
    {
        StartFillImage(act, dis);
    }

    void ClearBtnHover()
    {
        if (curBtn != null)
        {
            //上一个按钮执行退出操作
            curBtn.MouseExit();
            curBtn = null;
        }
        SetMouse2D(true);
    }

    /// <summary>
    /// 切换鼠标样式2d/3d
    /// </summary>
    /// <param name="is2D">If set to <c>true</c> is2 d.</param>
    void SetMouse2D(bool is2D)
    {
        if (is2D != isMouse2D)
        {
            isMouse2D = !isMouse2D;
            mouseIcon2D.SetActive(is2D);
            mouseIcon3D.gameObject.SetActive(!is2D);
            //进入2D模式后，停止倒计时
            if (isMouse2D)
            {
                StopFillImage();
            }
        }
    }
    #endregion

    public void SetVisiable(bool isvis)
    {
        SetMouse2D(true);
        mouseIcon3D.gameObject.SetActive(isvis);
        invisible = !isvis;
    }


    /// <summary>
    /// 设置3D鼠标输入开启关闭
    /// </summary>
    /// <param name="isopen"></param>
    public void SetEnable(bool isopen)
    {
        this.enabled = isopen;
        SetVisiable(isopen);
    }


    #region CircleFill
    Sequence mySque;
    void StartFillImage(Action callback, float dis)
    {
        mySque = DOTween.Sequence();
        mouseCountDown.fillAmount = 1f;
        mouseCountDown.transform.localScale = Vector3.one * 0.1f;
        mySque.Append(mouseCountDown.transform.DOScale(Vector3.one * 0.3f, .2f).SetEase(Ease.OutSine));
        //弹出界面
        mySque.Append(mouseCountDown.DOFillAmount(0, 3f));
        mySque.OnComplete(callback.Invoke);
    }
    void StopFillImage()
    {
        mySque.Kill();
    }
    #endregion
}
