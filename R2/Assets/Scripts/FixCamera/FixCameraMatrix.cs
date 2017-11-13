/*
 * 10/19/2017 Create By Keefor
 * 通过修改投影矩阵进行变换，代替原来的新建相机
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCameraMatrix : MonoBehaviour
{
    delegate void FixOperationFunc();

    private Camera[] camlist;
    private Matrix4x4 cu, cu1;

    private int FixState;
    private int UIState;
    private int OpertionState;
    private const int FNState = 1;
    private const int LRState = 2;
    private const int UDState = 3;
    private const int NormalState = 0;
    private const int CloseBGState = 4;
    private const int SaveState = 5;
    private string[] stateName = { "正常", "调整远近", "调整左右", "调整上下", "开关背景", "保存"};

    private Texture2D txt_bg;
    private int txt_w = 350, txt_h = 300;

    // Use this for initialization
    IEnumerator Start()
    {
        txt_bg = new Texture2D(txt_w, txt_h);
        Color[] tdc = new Color[txt_w * txt_h];
        for (int i = 0; i < txt_w * txt_h; i++)
        {
            tdc[i] = Color.white;
        }
        txt_bg.SetPixels(0, 0, txt_w, txt_h, tdc);
        while (!Vuforia.VuforiaARController.Instance.HasStarted)
        {
            yield return 0;
        }
        yield return new WaitForSeconds(0.5f);
        camlist = transform.GetComponentsInChildren<Camera>();

        camlist[0].transform.GetChild(0).gameObject.layer = 5;

        var count = 0;

        _isToMax = false;
        cu = camlist[0].projectionMatrix;
        var far = camlist[0].farClipPlane + 2000;
        var near = camlist[0].nearClipPlane;
        cu.m22 = -(far + near) / (far - near);
        cu.m23 = -2.0f * far * near / (far - near);
        cu1 = cu;


        //SourcePos = camlist[0].transform;
        for (int i = 0; i < camlist.Length; i++)
        {
            var vbb = camlist[i].gameObject.GetComponent<Vuforia.VideoBackgroundBehaviour>();
            if (vbb == null)
            {
                camlist[i] = null; continue;
            }
            camlist[i].projectionMatrix = cu;
            count++;
        }
        var _camlist = new Camera[count];
        count = -1;
        for (int i = 0; i < _camlist.Length; i++)
        {
            do
            {
                count++;
            } while (camlist[count] == null);
            _camlist[i] = camlist[count];
        }
        camlist = _camlist;
    }

    private void OnGUI()
    {
        var myStyle = new GUIStyle { fontSize = 40 };
        var myStyle1 = new GUIStyle { fontSize = 80 };

        GUI.DrawTexture(new Rect(0, 0, txt_w, txt_h), txt_bg);
        if (UIState != NormalState)
        {
            for (var i = 1; i <= stateName.Length - 1; i++)
            {
                GUILayout.Label(stateName[i], i == UIState ? myStyle1 : myStyle);
            }
        }
        else
        {
            GUILayout.Label(stateName[FixState], myStyle1);
        }
    }




    // Update is called once per frame
    void Update()
    {
        SelectOpertion();
    }

    private void SelectOpertion()
    {
        var _state = FixState;
        switch (FixState)
        {
            case FNState:
                OperationFunc(FnMatrixOpertion);
                break;
            case LRState:
                OperationFunc(LrMatrixOpertion);
                break;
            case UDState:
                OperationFunc(UdMatrixOpertion);
                break;
            case NormalState:
            case CloseBGState:
                NullOperationState();
                break;
        }
        if (_state != FixState)
        {
            cu = camlist[0].projectionMatrix;
            switch (FixState)
            {
                case CloseBGState:
                    CloseBg();
                    break;
                case SaveState:
                    SaveDate();
                    break;
                default:
                    OpenBg();
                    break;
            }
        }
    }

    private void NullOperationState()
    {
        int keycode = KeyInput.Inst.GetKey();
        if (UIState == NormalState)
        {
            if (keycode == KeyInput.DoublePress)
            {
                UIState = FNState;
            }
        }
        else
        {
            if (keycode == KeyInput.OncePress)
            {
                UIState++;
                if (UIState > stateName.Length - 1)
                {
                    UIState = 1;
                }
            }
            if (keycode == KeyInput.DoublePress)
            {
                if (UIState == CloseBGState && FixState == CloseBGState)
                {
                    FixState = NormalState;
                }
                else
                {
                    FixState = UIState;
                }
                UIState = NormalState;

            }
        }
    }


    private void OperationFunc(FixOperationFunc fixFunc)
    {
        switch (OpertionState)
        {
            case 0:
                if (KeyInput.Inst.GetKey() == KeyInput.OncePress)
                {
                    OpertionState = 1;
                }
                if (KeyInput.Inst.GetKey() == KeyInput.DoublePress)
                {
                    FixState = NormalState;
                }
                break;
            case 1:
                fixFunc();
                //int key = KeyInput();
                if (KeyInput.Inst.GetKey() == KeyInput.OncePress)
                {
                    OpertionState = 0;
                }
                if (KeyInput.Inst.GetKey() == KeyInput.DoublePress)
                {
                    OpertionState = 0;
                    FixState = NormalState;
                }
                //if (KeyInput.Inst.GetKey() == KeyInput.DoublePress)
                //{
                //    //isToMax = !isToMax;
                //    state = 2;
                //    __max = camlist[0].fieldOfView;
                //    __min = __max - 2 < _min ? _min : __max - 2;
                //    __max = __max + 2 > _max ? _max : __max + 2;
                //}
                break;
            case 2:
                fixFunc();
                //int key = KeyInput();
                if (KeyInput.Inst.GetKey() == KeyInput.OncePress)
                {
                    OpertionState = 0;
                }
                //if (KeyInput.Inst.GetKey() == KeyInput.DoublePress)
                //{
                //    isToMax = !isToMax;
                //}
                break;
        }
    }

    private void SaveDate()
    {
#if UNITY_EDITOR
        var path = "F:/ARConfig.txt";
#elif UNITY_ANDROID
        var path = "/storage/emulated/0/ARConfig.txt";
#endif
        var ang = camlist[0].projectionMatrix;
        System.IO.File.WriteAllText(path, ang.ToString());

        FixState = NormalState;
    }


    private void CloseBg()
    {
        for (int i = 0; i < camlist.Length; i++)
        {
            camlist[i].cullingMask &= ~(1 << 5);
        }
    }

    private void OpenBg()
    {
        for (int i = 0; i < camlist.Length; i++)
        {
            camlist[i].cullingMask |= (1 << 5);
        }
    }



    private float _dis = 1;
    private float _lrPos;
    private float _udPos;
    private bool _isToMax;

    private void LrMatrixOpertion()
    {
        _lrPos = ValueOffset(0.03f, -0.4f, 0.4f, _lrPos);
        var v1 = new Vector4(Mathf.Cos(_lrPos), 0, Mathf.Sin(_lrPos), 0);
        var v2 = new Vector4(0, 1, 0, 0);
        var v3 = new Vector4(-Mathf.Sin(_lrPos), 0, Mathf.Cos(_lrPos), 0);
        var v4 = new Vector4(0, 0, 0, 1);
        var qu = new Matrix4x4(v1, v2, v3, v4);
        SetMatrix(qu);
    }

    private void UdMatrixOpertion()
    {
        _udPos = ValueOffset(0.03f, -0.25f, 0.25f, _udPos);

        var v1 = new Vector4(1, 0, 0, 0);
        var v2 = new Vector4(0, Mathf.Cos(_udPos), -Mathf.Sin(_udPos), 0);
        var v3 = new Vector4(0, Mathf.Sin(_udPos), Mathf.Cos(_udPos), 0);
        var v4 = new Vector4(0, 0, 0, 1);
        var qu = new Matrix4x4(v1, v2, v3, v4);
        SetMatrix(qu);
    }

    private void FnMatrixOpertion()
    {
        _dis = ValueOffset(0.1f, 1f, 2, _dis);
        var v1 = new Vector4(_dis, 0, 0, 0);
        var v2 = new Vector4(0, _dis, 0, 0);
        var v3 = new Vector4(0, 0, 1, 0);
        var v4 = new Vector4(0, 0, 0, 1);
        var qu = new Matrix4x4(v1, v2, v3, v4);
        SetMatrix(qu);
    }

    private void SetMatrix(Matrix4x4 offset)
    {
        //Debug.Log(dis);
        foreach (var camera1 in camlist)
        {
            camera1.projectionMatrix = cu * offset;
        }
    }


    private float ValueOffset(float offset, float min, float max, float sour)
    {
        float cur = sour;
        if (_isToMax)
            cur += Time.deltaTime * offset;
        else
            cur -= Time.deltaTime * offset;

        if (cur <= min)
        {
            _isToMax = true;
            return sour;
        }
        if (cur >= max)
        {
            _isToMax = false;
            return sour;
        }
        return cur;
    }


}
