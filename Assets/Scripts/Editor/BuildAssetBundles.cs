using UnityEngine;
using UnityEditor;
using System.IO;

public class BuildAssetBundles
{
    [MenuItem(AppConst.KAssetBundleManager + AppConst.KSimulationMode, true)]
    public static bool ToggleSimulationModeAndSave()
    {
        Menu.SetChecked(AppConst.KAssetBundleManager + AppConst.KSimulationMode, Mgr_AssetBundle.SimulationMod);
        return true;
    }

    [MenuItem(AppConst.KAssetBundleManager + AppConst.KSimulationMode)]
    public static void ToggleSimulationMode()
    {
        Mgr_AssetBundle.SimulationMod = !Mgr_AssetBundle.SimulationMod;
    }

    [MenuItem(AppConst.KAssetBundleManager + AppConst.KBuildAssetBundles)]
    public static void BuildAllAssetBundle()
    {
        string path = Path.Combine(AppConst.AssetBundleFodlerName, Mgr_AssetBundle.GetPlatform());
        Debug.Log(path);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        else
        {
            if (EditorUtility.DisplayDialog("Log", "path is exists, do you want to cover it?", "yes", "no"))
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
            else return;
        }
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        Debug.Log("Build AssetBundles Completed!");
    }

    [MenuItem(AppConst.KAssetBundleManager + AppConst.KBuildABToStreamingPath)]
    public static void BuildAllAssetBundleToStreamingPath()
    {
        string path = Path.Combine(Application.streamingAssetsPath, AppConst.AssetBundleFodlerName);
        path = Path.Combine(path, Mgr_AssetBundle.GetPlatform());
        Debug.Log(path);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        else
        {
            if (EditorUtility.DisplayDialog("Log", "path is exists, do you want to coer it?", "yes", "no"))
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
            else return;
        }
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        Debug.Log("Build AssetBundles Completed!");
    }
}