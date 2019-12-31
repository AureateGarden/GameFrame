using UnityEngine;

public class PopBase : UIBase
{
    public override void Init()
    {
        base.Init();
        m_Type = UIType.PopUp;
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}