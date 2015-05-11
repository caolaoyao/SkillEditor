using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

public class AssetEditor 
{
    public static void IsDirectoryExit(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public static void CreateAsset(ScriptableObject asset, string path, string name)
    {
        IsDirectoryExit(path);
        AssetDatabase.CreateAsset(asset, path + "/" + name +".asset");
    }
}
