using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgr_Player : MonoBehaviour
{
    private static Mgr_Player _instance;
    public static Mgr_Player Instance
    {
        get
        {
            if (_instance == null) _instance = GameManager.Instance.gameObject.AddComponent<Mgr_Player>();
            return _instance;
        }
    }

    public MainPlayer m_MainPlayer;

    public void Init()
    {
        GameObject Player = Mgr_AssetBundle.Instance.LoadAsset<GameObject>(ABTypes.prefab, "MainPlayer");
        m_MainPlayer = Instantiate(Player).GetComponent<MainPlayer>();
        m_MainPlayer.transform.SetParent(GameManager.Instance.m_MainGame.m_Loaded, false);
        m_MainPlayer.Init();
        m_MainPlayer.gameObject.SetActive(false);
    }
}
