using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    public Transform[] m_Backgrounds;
    public Transform[] m_Foregrounds;
    public Transform m_CharacterMap;

    public Transform m_PlayerSpownPoint;

    private MainPlayer mainPlayer;

    public virtual void Init()
    {
        mainPlayer = Mgr_Player.Instance.m_MainPlayer;
        mainPlayer.transform.SetParent(m_CharacterMap, false);
        mainPlayer.transform.position = m_PlayerSpownPoint.position;
        mainPlayer.gameObject.SetActive(true);
        mainPlayer.OnSpown();
    }

    public virtual void OnOpen()
    {

    }

    public virtual void OnClose()
    {
        mainPlayer.transform.SetParent(GameManager.Instance.m_MainGame.m_Loaded, false);
        mainPlayer.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public virtual void SpownPlayer()
    {

    }
}
