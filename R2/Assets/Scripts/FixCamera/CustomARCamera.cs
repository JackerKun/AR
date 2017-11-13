/*
 * 10/19/2017 Create By Keefor
 * 加载保存的校正数据，并且对ARCamera 投影矩阵进行修正
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomARCamera : MonoBehaviour
{
    public bool fixCamera = false;
    public bool isRec = false;
    public bool ShowBg = true;
    private Camera[] camlist;
    // Use this for initialization
    IEnumerator Start()
    {
        while (!Vuforia.VuforiaARController.Instance.HasStarted)
        {
            yield return 0;
        }
        yield return new WaitForSeconds(0.5f);
        camlist = transform.GetComponentsInChildren<Camera>();
        var cu = camlist[0].projectionMatrix;
#if UNITY_EDITOR
        var path = "F:/ARConfig.txt";
#elif UNITY_ANDROID
        var path = "/storage/emulated/0/ARConfig.txt";
#endif
        if (System.IO.File.Exists(path))
            cu = ReadDate(path);
        for (int i = 0; i < camlist.Length; i++)
        {
            var vbb = camlist[i].gameObject.GetComponent<Vuforia.VideoBackgroundBehaviour>();
            if (vbb == null)
            {
                camlist[i] = null;
            }
        }

        camlist[0].transform.GetChild(0).gameObject.layer = 5;
        foreach (var camera1 in camlist)
        {
            if (camera1 == null)
                continue;
            if (fixCamera)
            {
                camera1.projectionMatrix = cu;
            }
            if (!ShowBg)
            {
                camera1.cullingMask &= ~(1 << 5);

            }
        }
        if (isRec)
        {

            var obj = new GameObject("VedioCamera");
            var camObj = obj.AddComponent<Camera>();
            camObj.nearClipPlane = camlist[0].nearClipPlane;
            camObj.farClipPlane = camlist[0].farClipPlane;
            camObj.fieldOfView = camlist[0].fieldOfView;
            camObj.depth = -10;


            camObj.transform.SetParent(this.transform);
            camObj.transform.rotation = Quaternion.identity;
            camObj.transform.localPosition = Vector3.zero;
            camObj.clearFlags = CameraClearFlags.SolidColor;
            camObj.backgroundColor = Color.black;
            camObj.gameObject.layer = 5;
        }
    }


    private Matrix4x4 ReadDate(string path)
    {

        var data = System.IO.File.ReadAllText(path);
        string[] list = data.Split('\t', '\n');
        foreach (var s in list)
        {
            Debug.Log(s);
        }
        var mat = new Matrix4x4
            {
                m00 = float.Parse(list[0]),
                m01 = float.Parse(list[1]),
                m02 = float.Parse(list[2]),
                m03 = float.Parse(list[3]),
                m10 = float.Parse(list[4]),
                m11 = float.Parse(list[5]),
                m12 = float.Parse(list[6]),
                m13 = float.Parse(list[7]),
                m20 = float.Parse(list[8]),
                m21 = float.Parse(list[9]),
                m22 = float.Parse(list[10]),
                m23 = float.Parse(list[11]),
                m30 = float.Parse(list[12]),
                m31 = float.Parse(list[13]),
                m32 = float.Parse(list[14]),
                m33 = float.Parse(list[15])
            };
        return mat;
    }
}
