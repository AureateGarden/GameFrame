using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelName
{
    LevelMain,
    Level0 = 10,
}

public class Mgr_Level : MonoBehaviour
{
    private static Mgr_Level _instance;
    public static Mgr_Level Instance
    {
        get
        {
            if (_instance == null) _instance = GameManager.Instance.gameObject.AddComponent<Mgr_Level>();
            return _instance;
        }
    }

    public LevelBase m_LevelNow { get; private set; }

    public void Init()
    {

    }

    public void OpenLevel<T>(LevelName levelName) where T : LevelBase
    {
        if (m_LevelNow != null) CloseLevel();
        GameObject Level = Mgr_AssetBundle.Instance.LoadAsset<GameObject>(ABTypes.prefab, levelName.ToString());
        m_LevelNow = Instantiate(Level).GetComponent<T>() as LevelBase;
        m_LevelNow.transform.SetParent(GameManager.Instance.m_MainGame.m_Level, false);
        m_LevelNow.Init();
        m_LevelNow.OnOpen();
    }

    private void CloseLevel()
    {
        m_LevelNow.OnClose();
        m_LevelNow = null;
    }
}