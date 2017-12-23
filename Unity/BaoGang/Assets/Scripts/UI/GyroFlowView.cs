using System.Collections;
using System.Collections.Generic;
using HopeRun;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 视口中心液位显示
/// </summary>
public class GyroFlowView : MonoBehaviour
{
	private ScrollRect _scrollView;
    private GameObject viewGO;
    private Gyroscope myGyro;
    private float curHorizontalV = .5f;
    private float curHorizontalH = .5f;
	// Use this for initialization

    private static GyroFlowView instance;

    public static GyroFlowView Inst
    {
        get
        {
            return instance ??
                   (instance = Instantiate(((GameObject) Resources.Load("Prefabs/2Dyewei")).GetComponent<GyroFlowView>()));
        }
    }

    void OnDestroy()
    {
        instance = null;
    }

    void Awake ()
	{
		myGyro = Input.gyro;
		myGyro.enabled = true;
		_scrollView = GetComponentInChildren<ScrollRect> ();
        text = _scrollView.transform.Find("Viewport/Content/Text").GetComponent<Text>();
        colorImg = _scrollView.transform.Find("Viewport/Content/RawImage").GetComponent<RawImage>();
		viewGO = _scrollView.gameObject;
		_scrollView.content.sizeDelta = new Vector2 (Screen.width, Screen.height);
        //SetFlowPanelActive (false);
	}

	void Update ()
	{
		curHorizontalV = Mathf.Clamp01 (curHorizontalV - myGyro.rotationRate.y * .05f);
		curHorizontalH = Mathf.Clamp01 (curHorizontalH + myGyro.rotationRate.x * .05f);
		_scrollView.horizontalNormalizedPosition = curHorizontalV;
		_scrollView.verticalNormalizedPosition = curHorizontalH;
	}

	public void SetFlowPanelActive (bool active)
	{
		if (viewGO) {
			viewGO.SetActive (active);
		}
	}
    public void UpdateFloatingPanel(float liquidH, float limitLevel)
    {
        if (text && colorImg)
        {
            text.text = string.Format("{0:00.00}%", liquidH);
            colorImg.color = GlobalManager.GetWarnColor(liquidH / limitLevel);
        }
    }

    private Text text;

    private RawImage colorImg;
}
