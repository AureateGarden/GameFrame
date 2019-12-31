using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Check : PopBase
{
    [Header("Custom")]
    public Text m_Info;
    public Button[] m_Btns;
    public Text[] m_BtnsText;

    public override void Init()
    {
        base.Init();
    }

    public override void OnOpen()
    {
        Reset();
        base.OnOpen();
    }

    public override void OnClose()
    {
        base.OnClose();
    }

    public void Reset()
    {
        foreach (var item in m_Btns) item.gameObject.SetActive(false);
        m_Info.text = string.Empty;
        foreach (var item in m_Btns) item.onClick.RemoveAllListeners();
        foreach (var item in m_BtnsText) item.text = string.Empty;
    }

    public void ShowInfo(string info)
    {
        m_Info.text = info;
        m_Btns[0].gameObject.SetActive(true);
        m_BtnsText[0].text = AppConst.StrOk;
        m_Btns[0].onClick.AddListener(() => OnClose());
    }

    public void ShowCheck(string info, Action action1 = null, Action action2 = null)
    {
        m_Info.text = info;
        m_Btns[0].gameObject.SetActive(true);
        m_Btns[1].gameObject.SetActive(true);
        m_BtnsText[0].text = AppConst.StrOk;
        m_BtnsText[1].text = AppConst.StrNo;
        m_Btns[0].onClick.AddListener(() =>
        {
            if (action1 != null) action1();
            OnClose();
        });
        m_Btns[1].onClick.AddListener(() =>
        {
            if (action2 != null) action2();
            OnClose();
        });
    }
}
