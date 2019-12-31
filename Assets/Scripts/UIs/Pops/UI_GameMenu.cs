using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameMenu : PopBase
{
    public override void Init()
    {
        base.Init();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        if (!GameManager.Instance.m_CurGameSpeed.Equals(GameSpeed.Stop))
            GameManager.Instance.ToggleGamePause();
    }

    public override void OnClose()
    {
        if (GameManager.Instance.m_CurGameSpeed.Equals(GameSpeed.Stop))
            GameManager.Instance.ToggleGamePause();
        base.OnClose();
    }

    public void BtnBack()
    {
        OnClose();
    }

    public void BtnMainPage()
    {
        Mgr_UI.Instance.ToUI<UI_Start>(UIName.UI_Start);
    }

    public void BtnOption()
    {
        Mgr_UI.Instance.ToUI<UI_Option>(UIName.UI_Option);
    }
}
