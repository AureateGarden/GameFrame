using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppConst
{
#if UNITY_EDITOR
    public const string KAssetBundleManager = "Tools/AssetBundleManager/";
    public const string KSimulationMode = "Simulation Mode";
    public const string KBuildAssetBundles = "BuildAssetBundles";
    public const string KBuildABToStreamingPath = "BuildABToStreaming";

    /// Exception info below

    public const string ErSameNameTitle = "Same Name Below : ";
    public const string ErNoAssetFound = "There is no Asset named : ";
    public const string ErHaveSameName = "There are some Assets with same name.";

#endif
    //AssetBundle
    public const string KABNameSuffix = ".rs";
    public const string AssetBundleFodlerName = "AssetBundles";
    public const string PlatformAndroid = "android";
    public const string PlatformWindows = "windows";
    public const string PlatformIOS = "ios";
    //AssetBundle end

    //Sound
    public const string SoundListObjName = "SoundList";
    //Sound end

    //UIModel
    public const string UIModelKeyCell = "KeyCell";
    public const string UIModelTypeLine = "TypeLine";
    //UIModel end

    //Display string
    public const string StrPlayerKeyBoradTitle = "Player";
    public const string StrOk = "Ok!";
    public const string StrYes = "Yes";
    public const string StrNo = "No";
    public const string StrIsntSaveOption = "Do you want to exit without save ?";
    //Display string end

    public const int DefaultDestroyUIDelay = 120;
    public const int BaseAssetBundleCount = 5;
    public const int PlayerCustomKeyIndex = 20;

    /// Exception info below
}
