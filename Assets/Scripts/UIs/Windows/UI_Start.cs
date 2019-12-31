using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Start : UIBase
{
    public override void Init()
    {
        base.Init();
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }

    public override void OnClose()
    {
        base.OnClose();
    }

    public void BtnStart()
    {
        Mgr_UI.Instance.ToUI<UI_Game>(UIName.UI_Game);
        Mgr_Level.Instance.OpenLevel<LevelMain>(LevelName.LevelMain);
        OnClose();
    }

    public void BtnOption()
    {
        Mgr_UI.Instance.ToUI<UI_Option>(UIName.UI_Option);
    }

    public void BtnExit()
    {
        Application.Quit();
    }
}
