using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public enum ABTypes
{
    font,
    material,
    prefab,
    sprite,
    sound,
}

public class AssetBundleAsyncOperation
{
    public System.Action m_CallBack;

#if UNITY_EDITOR
    public UnityEngine.Object m_SimulationModObject;

    public AssetBundleAsyncOperation(UnityEngine.Object simulationModObject)
    {
        m_SimulationModObject = simulationModObject;
    }

    public AssetBundleAsyncOperation(UnityEngine.Object simulationModObject, System.Action callback)
    {
        m_SimulationModObject = simulationModObject;
        m_CallBack = callback;
    }
#endif
    public AssetBundleRequest m_Request;
    public AssetBundleAsyncOperation(AssetBundleRequest request)
    {
        m_Request = request;
        m_CallBack = null;
    }

    public AssetBundleAsyncOperation(AssetBundleRequest request, System.Action callback)
    {
        m_Request = request;
        m_CallBack = callback;
    }

    public T GetAsset<T>() where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        return m_SimulationModObject as T;
#else
        return m_Request.asset as T;
#endif
    }

    public float GetProgress()
    {
#if UNITY_EDITOR
        return 1;
#else
        return m_Request.progress;
#endif
    }

    public bool IsDone()
    {
#if UNITY_EDITOR
        if (m_CallBack != null) m_CallBack();
        return true;
#else
        if (m_Request.isDone)
        {
            if (m_CallBack != null) m_CallBack();
            return true;
        }
        
        return false;
#endif
    }

}

public class Mgr_AssetBundle : MonoBehaviour
{
    private static Mgr_AssetBundle _instance;
    public static Mgr_AssetBundle Instance
    {
        get
        {
            if (_instance == null) _instance = GameManager.Instance.gameObject.AddComponent<Mgr_AssetBundle>();
            return _instance;
        }
    }
#if UNITY_EDITOR
    public static bool SimulationMod
    {
        get { return EditorPrefs.GetBool("SimulationMod", false); }
        set { EditorPrefs.SetBool("SimulationMod", value); }
    }
#endif

    private Dictionary<ABTypes, AssetBundle> m_LoadedAssetBundles;
    private Dictionary<ABTypes, AssetBundleCreateRequest> m_AssetBundleCreateRequests;
    private string m_LoadingAssetBundlePath = string.Empty;

    public void Init()
    {
#if UNITY_EDITOR
        if (SimulationMod) return;
#endif
        m_LoadedAssetBundles = new Dictionary<ABTypes, AssetBundle>();
        m_AssetBundleCreateRequests = new Dictionary<ABTypes, AssetBundleCreateRequest>();
        m_LoadingAssetBundlePath = Path.Combine(Application.streamingAssetsPath, AppConst.AssetBundleFodlerName);
        m_LoadingAssetBundlePath = Path.Combine(m_LoadingAssetBundlePath, GetPlatform()) + "/";
    }

    public static string GetPlatform()
    {
#if UNITY_EDITOR
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return AppConst.PlatformWindows;
            case BuildTarget.Android:
                return AppConst.PlatformAndroid;
            case BuildTarget.iOS:
                return AppConst.PlatformIOS;
            default:
                Debug.Log(EditorUserBuildSettings.activeBuildTarget + "is out of range of paking.");
                return string.Empty;
        }
#else
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
                return AppConst.PlatformWindows;
            case RuntimePlatform.Android:
                return AppConst.PlatformAndroid;
            case RuntimePlatform.IPhonePlayer:
                return AppConst.PlatformIOS;
            default:
                return string.Empty;
        }
#endif
    }

    public int LoadAssetBundle(ABTypes abType, string path = "", bool IsAsync = true, uint crc = 0, ulong offset = 0)
    {
        AssetBundleCreateRequest request = null;
#if UNITY_EDITOR
        if (SimulationMod) return 0;
#endif
        if (m_LoadedAssetBundles.ContainsKey(abType)) return 0;
        else if (m_AssetBundleCreateRequests.ContainsKey(abType)) return 1;
        if (path == "") path = m_LoadingAssetBundlePath + abType.ToString() + "/" + abType.ToString() + AppConst.KABNameSuffix;
        AssetBundle assetBundle = null;
        if (IsAsync)
        {
            request = AssetBundle.LoadFromFileAsync(path, crc, offset);
            m_AssetBundleCreateRequests.Add(abType, request);
            StartCoroutine(UpdateAssetBundleCreateRequest(abType, request));
        }
        else
        {
            assetBundle = AssetBundle.LoadFromFile(path, crc, offset);
            if (assetBundle == null) return -1;
            m_LoadedAssetBundles.Add(abType, assetBundle);
        }

        return 0;
    }

    public int LoadAssetBundle(ABTypes abType, byte[] data, bool IsAsync = true, uint crc = 0)
    {
        AssetBundleCreateRequest request = null;
#if UNITY_EDITOR
        if (SimulationMod) return 0;
#endif
        if (m_LoadedAssetBundles.ContainsKey(abType)) return 0;
        else if (m_AssetBundleCreateRequests.ContainsKey(abType)) return 1;
        AssetBundle assetBundle = null;
        if (IsAsync)
        {
            request = AssetBundle.LoadFromMemoryAsync(data, crc);
            m_AssetBundleCreateRequests.Add(abType, request);
            StartCoroutine(UpdateAssetBundleCreateRequest(abType, request));
        }
        else
        {
            AssetBundle.LoadFromMemory(data, crc);
            if (assetBundle == null) return -1;
            m_LoadedAssetBundles.Add(abType, assetBundle);
        }

        return 0;
    }

    public int LoadAssetBundle(ABTypes abType, Stream stream, bool IsAsync = true, uint crc = 0, uint bufferSize = 32)
    {
        AssetBundleCreateRequest request = null;
#if UNITY_EDITOR
        if (SimulationMod) return 0;
#endif
        if (m_LoadedAssetBundles.ContainsKey(abType)) return 0;
        else if (m_AssetBundleCreateRequests.ContainsKey(abType)) return 1;
        AssetBundle assetBundle = null;
        if (IsAsync)
        {
            request = AssetBundle.LoadFromStreamAsync(stream, crc, bufferSize);
            m_AssetBundleCreateRequests.Add(abType, request);
            StartCoroutine(UpdateAssetBundleCreateRequest(abType, request));
        }
        else
        {
            assetBundle = AssetBundle.LoadFromStream(stream, crc, bufferSize);
            if (assetBundle == null) return -1;
            m_LoadedAssetBundles.Add(abType, assetBundle);
        }

        return 0;
    }

    IEnumerator UpdateAssetBundleCreateRequest(ABTypes abType, AssetBundleCreateRequest request)
    {
        while (!request.isDone) yield return new WaitForEndOfFrame();
        m_LoadedAssetBundles.Add(abType, request.assetBundle);
        m_AssetBundleCreateRequests.Remove(abType);
    }

    public T LoadAsset<T>(ABTypes abType, string assetName) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        if (SimulationMod)
        {
            string path = abType.ToString() + "/" + abType.ToString() + AppConst.KABNameSuffix;
            string[] assetpath = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(path, assetName);
            if (assetpath.Length == 0) throw new GameException(AppConst.ErNoAssetFound + assetName);
            else if (assetpath.Length > 1)
            {
                string same = AppConst.ErSameNameTitle;
                foreach (string name in assetpath)
                {
                    same += "\n";
                    same += name;
                }
                throw new GameException(AppConst.ErHaveSameName + same);
            }
            return AssetDatabase.LoadAssetAtPath<T>(assetpath[0]);
        }
#endif
        AssetBundle assetBundle;
        if (m_LoadedAssetBundles.TryGetValue(abType, out assetBundle)) return assetBundle.LoadAsset<T>(assetName);

        return default(T);
    }

    public AssetBundleAsyncOperation LoadAssetAsync<T>(ABTypes abType, string assetName, System.Action callback = null) where T : UnityEngine.Object
    {
        AssetBundleAsyncOperation Operation = null;
#if UNITY_EDITOR
        if (SimulationMod)
        {
            string path = abType.ToString() + "/" + abType.ToString() + AppConst.KABNameSuffix;
            string[] assetpath = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(path, assetName);
            if (assetpath.Length == 0) throw new GameException(AppConst.ErNoAssetFound + assetName);
            else if (assetpath.Length > 1)
            {
                string same = AppConst.ErSameNameTitle;
                foreach (string name in assetpath)
                {
                    same += "\n";
                    same += name;
                }
                throw new GameException(AppConst.ErHaveSameName + same);
            }

            Operation = new AssetBundleAsyncOperation(AssetDatabase.LoadAssetAtPath<T>(assetpath[0]), callback);
            return Operation;
        }
#endif
        AssetBundle assetBundle = null;
        if (m_LoadedAssetBundles.TryGetValue(abType, out assetBundle)) Operation = new AssetBundleAsyncOperation(assetBundle.LoadAssetAsync<T>(assetName), callback);

        return Operation;
    }

    public bool IsBaseAssetBunleLoadFinished()
    {
#if UNITY_EDITOR
        if (SimulationMod) return true;
#endif
        int count = 0;
        for (int i = 0; i < AppConst.BaseAssetBundleCount; ++i)
        {
            if (m_LoadedAssetBundles.ContainsKey((ABTypes)i)) ++count;
        }
        if (count == AppConst.BaseAssetBundleCount) return true;
        return false;
    }
}
