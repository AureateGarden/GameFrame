using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : UIBase
{
    public Button m_btn;
    public override void Init()
    {
        base.Init();
        m_btn.onClick.AddListener(BtnMoveCamera);
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }

    public override void OnClose()
    {
        GameManager.Instance.SetGameRunningSpeed(GameSpeed.Normal);
        base.OnClose();
    }

    public void BtnMoveCamera()
    {
        GameManager.Instance.m_MainGame.m_CameraMove.MoveToTarget(Mgr_Level.Instance.m_LevelNow.m_PlayerSpownPoint);
    }
}
