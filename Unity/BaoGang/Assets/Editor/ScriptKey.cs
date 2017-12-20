/*
*Created by keefor on 7/25/2016.
*/
using UnityEngine;
using UnityEditor;
using System.Collections;

public class HEScriptKey : AssetModificationProcessor
{

    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        int index = path.LastIndexOf('.');
        string file = path.Substring(index);
        if (!file.EndsWith(".cs")) return;

        index = Application.dataPath.LastIndexOf("Assets", System.StringComparison.Ordinal);
        path = Application.dataPath.Substring(0, index) + path;
        file = System.IO.File.ReadAllText(path);

        file = file.Replace("#CTIME#", System.DateTime.Now.ToString("d"));

        System.IO.File.WriteAllText(path, file, System.Text.Encoding.UTF8);
        AssetDatabase.Refresh();
    }
}