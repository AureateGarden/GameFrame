using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameSpeed
{
    Stop,
    Slow,
    Normal,
    Fast,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public MainUI m_MainUI;
    public MainGame m_MainGame;
    public float m_GameSlowSpeed { get; private set; }
    public float m_GameNormalSpeed { get; private set; }
    public float m_GameFastSpeed { get; private set; }
    public GameSpeed m_CurGameSpeed { get; private set; }
    public GameSpeed m_CacheGameSpeed { get; private set; }
    private void Awake()
    {
        Instance = this;
        m_CurGameSpeed = GameSpeed.Normal;
        m_CacheGameSpeed = GameSpeed.Stop;
        Mgr_AssetBundle.Instance.Init();
        LoadBaseAsset();
    }

    private void LoadBaseAsset()
    {
        for (int i = 0; i < AppConst.BaseAssetBundleCount; ++i) Mgr_AssetBundle.Instance.LoadAssetBundle((ABTypes)i);
        StartCoroutine(WaitForAssetBundleLoadFinished());
    }

    public void SetGameRunningSpeed(GameSpeed speed)
    {
        if (!m_CurGameSpeed.Equals(GameSpeed.Stop))
            m_CacheGameSpeed = m_CurGameSpeed;
        switch (speed)
        {
            case GameSpeed.Fast:
                Time.timeScale = 1.5f;
                break;
            case GameSpeed.Normal:
                Time.timeScale = 1f;
                break;
            case GameSpeed.Slow:
                Time.timeScale = 0.5f;
                break;
            case GameSpeed.Stop:
                Time.timeScale = 0;
                break;
        }
        m_CurGameSpeed = speed;
    }

    public void ToggleGamePause()
    {
        if (m_CurGameSpeed.Equals(GameSpeed.Stop))
        {
            SetGameRunningSpeed(m_CacheGameSpeed);
        }
        else SetGameRunningSpeed(GameSpeed.Stop);
    }

    IEnumerator WaitForAssetBundleLoadFinished()
    {
        while (!Mgr_AssetBundle.Instance.IsBaseAssetBunleLoadFinished()) yield return new WaitForEndOfFrame();
        Mgr_DataSave.Instance.Init();
        Mgr_UI.Instance.Init();
        Mgr_Input.Instance.Init();
        Mgr_Level.Instance.Init();
        Mgr_Sound.Instance.Init();
        Mgr_Player.Instance.Init();
        m_MainGame.Init();
        m_MainUI.Init();
        Mgr_UI.Instance.ToUI<UI_Start>(UIName.UI_Start).Init();
    }
}
